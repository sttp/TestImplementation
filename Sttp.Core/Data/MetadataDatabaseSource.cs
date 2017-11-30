using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sttp.Codec;
using Sttp.Core.Data;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataDatabaseSource
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid SchemaVersion { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long Revision { get; private set; }

        private Dictionary<string, int> m_tablesLookup;
        private List<MetadataTable> m_tables;

        private bool m_isReadOnly;
        private List<MetadataSchemaTables> m_metadataSchema;

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
                        metadataTable.SetIsReadOnly(Revision, TableLookup, LookupForeignKey);
                    }
                    RefreshSchema();
                    m_isReadOnly = true;
                }
            }
        }

        private int TableLookup(string arg)
        {
            if (m_tablesLookup.TryGetValue(arg, out int index))
            {
                return index;
            }
            return -1;
        }

        private int LookupForeignKey(int i, SttpValue sttpValue)
        {
            return m_tables[i].LookupRowIndex(sttpValue);
        }

        public MetadataTable LookupTable(int index)
        {
            return m_tables[index];
        }

        public MetadataDatabaseSource()
        {
            m_tablesLookup = new Dictionary<string, int>();
            m_tables = new List<MetadataTable>();
        }

        public MetadataDatabaseSource(DataSet dataSet)
        {
            m_tablesLookup = new Dictionary<string, int>();
            m_tables = new List<MetadataTable>();

            //-------------------------------------
            //--   Fill the schema
            //-------------------------------------
            Dictionary<string, string[]> primaryKeys = new Dictionary<string, string[]>();
            Dictionary<string, List<MetadataForeignKey>> foreignKeys = new Dictionary<string, List<MetadataForeignKey>>();

            foreach (DataTable table in dataSet.Tables)
            {
                if ((table.PrimaryKey?.Length ?? 0) == 0)
                    throw new Exception("All tables must have primary keys defined");

                primaryKeys.Add(table.TableName, table.PrimaryKey.Select(x => x.ColumnName).ToArray());
                foreignKeys.Add(table.TableName, new List<MetadataForeignKey>());
            }

            foreach (DataRelation relation in dataSet.Relations)
            {
                if (relation.ChildColumns.Length == 1 && relation.ParentColumns.Length == 1)
                {
                    //Foreign keys are only permitted with a single column primary key. This is an implementation issue, not a protocol issue.

                    string table = relation.ChildTable.TableName;
                    string column = relation.ChildColumns[0].ColumnName;
                    string foreignTable = relation.ParentTable.TableName;
                    string foreignColumn = relation.ParentColumns[0].ColumnName;

                    if (primaryKeys[foreignTable].Length == 1 && primaryKeys[foreignTable][0] == foreignColumn)
                    {
                        foreignKeys[table].Add(new MetadataForeignKey(column, foreignTable));
                    }
                }
            }

            foreach (DataTable table in dataSet.Tables)
            {
                var columns = new List<MetadataColumn>();
                foreach (DataColumn c in table.Columns)
                {
                    columns.Add(new MetadataColumn(c.ColumnName, SttpValueTypeCodec.FromType(c.DataType)));
                }

                AddOrReplaceTable(table.TableName, columns, foreignKeys[table.TableName]);
            }

            //-------------------------------------
            //--   Fill the data
            //-------------------------------------

            foreach (DataTable table in dataSet.Tables)
            {
                int index = m_tablesLookup[table.TableName];
                string[] pkey = primaryKeys[table.TableName];

                foreach (DataRow row in table.Rows)
                {
                    SttpValue key = new SttpValue();
                    SttpValueSet values = new SttpValueSet();
                    values.Values.AddRange(row.ItemArray.Select(x => new SttpValue(x)));
                    if (pkey.Length == 1)
                    {
                        key.SetValue(row[pkey[0]]);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    m_tables[index].AddOrUpdateRow(key, values, Revision);
                }
            }

            IsReadOnly = true;
        }

        private MetadataDatabaseSource(MetadataDatabaseSource other)
        {
            m_tablesLookup = new Dictionary<string, int>(other.m_tablesLookup);
            m_tables = other.m_tables.Select(x => x.CloneEditable()).ToList();
            SchemaVersion = other.SchemaVersion;
            Revision = other.Revision + 1;
        }

        public MetadataTable this[string tableName]
        {
            get
            {
                return m_tables[m_tablesLookup[tableName]];
            }
        }

        public void AddOrReplaceTable(string tableName, List<MetadataColumn> columns, List<MetadataForeignKey> tableRelationships)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            SchemaVersion = Guid.NewGuid();
            Revision = 0;
            if (!m_tablesLookup.ContainsKey(tableName))
            {
                m_tables.Add(new MetadataTable(tableName, columns, tableRelationships));
                m_tablesLookup[tableName] = m_tables.Count - 1;
            }
            else
            {
                int index = m_tablesLookup[tableName];
                m_tables[index] = new MetadataTable(tableName, columns, tableRelationships);
            }

        }

        public void AddOrReplaceRow(string tableName, SttpValue key, SttpValueSet fields)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            int index = m_tablesLookup[tableName];
            m_tables[index].AddOrUpdateRow(key, fields, Revision);
        }

        public void DeleteRow(string tableName, SttpValue key)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            int index = m_tablesLookup[tableName];
            m_tables[index].DeleteRow(key, Revision);
        }

        public MetadataDatabaseSource CloneEditable()
        {
            return new MetadataDatabaseSource(this);
        }

        private void RefreshSchema()
        {
            m_metadataSchema = new List<MetadataSchemaTables>();
            foreach (var table in m_tables)
            {
                var t = new MetadataSchemaTables();
                t.TableName = table.TableName;
                t.LastModifiedRevision = table.LastModifiedRevision;
                foreach (var col in table.Columns)
                {
                    t.Columns.Add(col);
                }
                foreach (var col in table.ForeignKeys)
                {
                    t.ForeignKeys.Add(new MetadataForeignKey(col.ColumnName, col.ForeignTableName));
                }
                m_metadataSchema.Add(t);
            }
        }

        public void ProcessCommand(CommandGetMetadata command, WireEncoder encoder)
        {
            if (command.SchemaVersion != SchemaVersion &&
                (command.AreUpdateQueries || command.SchemaVersion != Guid.Empty))
            {
                encoder.MetadataVersionNotCompatible();
                return;
            }

            foreach (var query in command.Queries)
            {
                var engine = new MetadataQueryExecutionEngine(this, command, encoder, query);
            }
        }

        public void ProcessCommand(CommandGetMetadataSchema command, WireEncoder encoder)
        {
            if (command.SchemaVersion != SchemaVersion)
            {
                encoder.MetadataSchema(SchemaVersion, Revision, m_metadataSchema);
            }
            else
            {
                List<MetadataSchemaTableUpdate> tableRevisions = new List<MetadataSchemaTableUpdate>();
                foreach (var tables in m_metadataSchema)
                {
                    if (tables.LastModifiedRevision > command.Revision)
                    {
                        tableRevisions.Add(new MetadataSchemaTableUpdate(tables.TableName, tables.LastModifiedRevision));
                    }
                }
                encoder.MetadataSchemaUpdate(SchemaVersion, Revision, command.Revision, tableRevisions);
            }
        }


    }

}
