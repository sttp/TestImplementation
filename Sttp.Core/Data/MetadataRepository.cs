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
        public Guid RuntimeID { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long VersionNumber { get; private set; }

        public List<MetadataSchemaTable> MetadataSchema { get; private set; }
        private Dictionary<string, int> m_tablesLookup;
        private List<MetadataTable> m_tables;
        private bool m_isReadOnly;

        public MetadataRepository()
        {
            RuntimeID = Guid.NewGuid();
            VersionNumber = 0;
            m_tablesLookup = new Dictionary<string, int>();
            m_tables = new List<MetadataTable>();
            MetadataSchema = new List<MetadataSchemaTable>();
            m_isReadOnly = false;
            IsReadOnly = true;
        }

        public MetadataRepository(DataSet schema)
        {
            RuntimeID = Guid.NewGuid();
            VersionNumber = 0;
            m_tablesLookup = new Dictionary<string, int>();
            m_tables = new List<MetadataTable>();
            m_isReadOnly = false;

            //--   Fill the schema
            //-------------------------------------

            foreach (DataTable table in schema.Tables)
            {
                var columns = new List<MetadataColumn>();
                foreach (DataColumn c in table.Columns)
                {
                    columns.Add(new MetadataColumn(c.ColumnName, SttpValueTypeCodec.FromType(c.DataType)));
                }
                m_tables.Add(new MetadataTable(table.TableName, columns));
                m_tablesLookup[table.TableName] = m_tables.Count - 1;
            }

            MetadataSchema = new List<MetadataSchemaTable>();
            foreach (var table in m_tables)
            {
                var t = new MetadataSchemaTable();
                t.TableName = table.TableName;
                t.LastModifiedVersionNumber = table.LastModifiedVersionNumber;
                foreach (var col in table.Columns)
                {
                    t.Columns.Add(col);
                }
                MetadataSchema.Add(t);
            }
        }

        public void FillData(string tableName, DataTable table)
        {
            if (m_isReadOnly)
                throw new Exception("Object is readonly");

            int indx = m_tablesLookup[tableName];
            m_tables[indx] = m_tables[indx].MergeDataSets(table, VersionNumber);
        }

        public void FillData(string tableName, DbDataReader table)
        {
            if (m_isReadOnly)
                throw new Exception("Object is readonly");

            int indx = m_tablesLookup[tableName];
            m_tables[indx] = m_tables[indx].MergeDataSets(table, VersionNumber);
        }

        private MetadataRepository(MetadataRepository other)
        {
            m_tablesLookup = other.m_tablesLookup;
            MetadataSchema = other.MetadataSchema.ToList();
            m_tables = other.m_tables.ToList();
            RuntimeID = other.RuntimeID;
            VersionNumber = other.VersionNumber + 1;
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
                    for (var x = 0; x < m_tables.Count; x++)
                    {
                        var table = m_tables[x];
                        var t = MetadataSchema[x];
                        if (t.LastModifiedVersionNumber != table.LastModifiedVersionNumber)
                        {
                            MetadataSchema[x] = t.Clone(table.LastModifiedVersionNumber);
                        }
                    }
                    m_isReadOnly = true;
                }
            }
        }

        public MetadataRepository CloneEditable()
        {
            return new MetadataRepository(this);
        }

        public bool ContainsTable(string tableName)
        {
            return m_tablesLookup.ContainsKey(tableName);
        }
    }

}
