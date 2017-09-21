using System;
using System.Collections.Generic;
using System.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSet
    {
        private Dictionary<string, int> m_tableLookup;
        private List<MetadataTable> m_tables;

        public MetadataSet()
        {
            m_tables = new List<MetadataTable>();
            m_tableLookup = new Dictionary<string, int>();
        }

        public MetadataTable this[string tableName]
        {
            get
            {
                return m_tables[m_tableLookup[tableName]];
            }
        }

        public void AddTable(MetadataTable table)
        {
            m_tables.Add(table);
            m_tableLookup.Add(table.TableName, m_tables.Count - 1);
        }
    }
}
