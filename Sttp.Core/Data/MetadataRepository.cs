using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Sttp.Codec;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataRepository
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid SchemaVersion { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long SequenceNumber { get; private set; }

        public List<MetadataSchemaTable> MetadataSchema { get; private set; }
        private Dictionary<string, int> m_tablesLookup;
        private List<MetadataTable> m_tables;
        private bool m_isReadOnly;

        public MetadataRepository(DataSet schema)
        {
            SchemaVersion = Guid.NewGuid();
            SequenceNumber = 0;
            m_tablesLookup = new Dictionary<string, int>();
            m_tables = new List<MetadataTable>();
            m_isReadOnly = false;

            //--   Fill the schema
            //-------------------------------------
            Dictionary<string, string> primaryKeys = new Dictionary<string, string>();
            Dictionary<string, List<MetadataForeignKeyMapping>> foreignKeys = new Dictionary<string, List<MetadataForeignKeyMapping>>();
            List<string> tableNames = new List<string>();

            foreach (DataTable table in schema.Tables)
            {
                if (table.PrimaryKey == null)
                    throw new Exception("All tables must have 1 primary keys defined");
                if (table.PrimaryKey.Length != 1)
                    throw new Exception("All tables must have 1 primary keys defined");
                tableNames.Add(table.TableName);
                primaryKeys.Add(table.TableName, table.PrimaryKey[0].ColumnName);
                foreignKeys.Add(table.TableName, new List<MetadataForeignKeyMapping>());
            }

            foreach (DataRelation relation in schema.Relations)
            {
                if (relation.ChildColumns.Length == 1 && relation.ParentColumns.Length == 1)
                {
                    //Foreign keys are only permitted with a single column primary key. This is an implementation issue, not a protocol issue.

                    string table = relation.ChildTable.TableName;
                    string column = relation.ChildColumns[0].ColumnName;
                    string foreignTable = relation.ParentTable.TableName;
                    string foreignColumn = relation.ParentColumns[0].ColumnName;

                    if (primaryKeys[foreignTable] == foreignColumn)
                    {
                        var fk = new MetadataForeignKeyMapping(column, foreignTable);
                        fk.LocalColumnIndex = relation.ChildTable.Columns.IndexOf(column);
                        fk.TableIndex = tableNames.IndexOf(fk.ForeignTableName);

                        foreignKeys[table].Add(fk);
                    }
                }
            }

            foreach (DataTable table in schema.Tables)
            {
                var columns = new List<MetadataColumn>();
                foreach (DataColumn c in table.Columns)
                {
                    columns.Add(new MetadataColumn(c.ColumnName, SttpValueTypeCodec.FromType(c.DataType)));
                }
                m_tables.Add(new MetadataTable(table.TableName, table.PrimaryKey[0].ColumnName, columns, foreignKeys[table.TableName]));
                m_tablesLookup[table.TableName] = m_tables.Count - 1;
            }

            MetadataSchema = new List<MetadataSchemaTable>();
            foreach (var table in m_tables)
            {
                var t = new MetadataSchemaTable();
                t.TableName = table.TableName;
                t.LastModifiedSequenceNumber = table.LastModifiedSequenceNumber;
                foreach (var col in table.Columns)
                {
                    t.Columns.Add(col);
                }
                foreach (var col in table.ForeignKeys)
                {
                    t.ForeignKeys.Add(new MetadataForeignKey(col.ColumnName, col.ForeignTableName));
                }
                MetadataSchema.Add(t);
            }
        }

        public void FillData(string tableName, DataTable table)
        {
            if (m_isReadOnly)
                throw new Exception("Object is readonly");

            int indx = m_tablesLookup[tableName];
            m_tables[indx] = m_tables[indx].MergeDataSets(table, SequenceNumber);
        }

        public void FillData(string tableName, DbDataReader table)
        {
            if (m_isReadOnly)
                throw new Exception("Object is readonly");

            int indx = m_tablesLookup[tableName];
            m_tables[indx] = m_tables[indx].MergeDataSets(table, SequenceNumber);
        }

        private MetadataRepository(MetadataRepository other)
        {
            m_tablesLookup = other.m_tablesLookup;
            MetadataSchema = other.MetadataSchema.ToList();
            m_tables = other.m_tables.ToList();
            SchemaVersion = other.SchemaVersion;
            SequenceNumber = other.SequenceNumber + 1;
        }

        public MetadataTable this[string tableName] => m_tables[m_tablesLookup[tableName]];

        /// <summary>
        /// Gets/Sets the readonly nature of this class.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return m_isReadOnly;
            }
            set
            {
                if (m_isReadOnly && !value) //True, changing to false
                    throw new Exception("This class is immutable");
                if (!m_isReadOnly && value) //False changing to true
                {
                    foreach (var metadataTable in m_tables)
                    {
                        metadataTable.LookupForeignKeys(LookupForeignKey);
                    }

                    for (var x = 0; x < m_tables.Count; x++)
                    {
                        var table = m_tables[x];
                        var t = MetadataSchema[x];
                        if (t.LastModifiedSequenceNumber != table.LastModifiedSequenceNumber)
                        {
                            MetadataSchema[x] = t.Clone(table.LastModifiedSequenceNumber);
                        }
                    }
                    m_isReadOnly = true;
                }
            }
        }

        private int LookupForeignKey(int i, SttpValue sttpValue)
        {
            return m_tables[i].LookupRowIndex(sttpValue);
        }

        public MetadataTable LookupTable(int index)
        {
            return m_tables[index];
        }

        public MetadataRepository CloneEditable()
        {
            return new MetadataRepository(this);
        }
    }

}
