using System;
using System.Collections.Generic;
using System.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Publisher
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSetSource
    {
        private Dictionary<string, int> m_tableLookup;
        private List<MetadataTableSource> m_tables;

        public MetadataSetSource()
        {
            m_tables = new List<MetadataTableSource>();
            m_tableLookup = new Dictionary<string, int>();
        }

        public MetadataTableSource this[string tableName]
        {
            get
            {
                return m_tables[m_tableLookup[tableName]];
            }
        }

        public void AddTable(MetadataTableSource tableSource)
        {
            m_tables.Add(tableSource);
            m_tableLookup.Add(tableSource.TableName, m_tables.Count - 1);
        }
    }
}
