using System;
using System.Collections.Generic;
using Sttp.WireProtocol.Data;

namespace Sttp.Data
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
            if (tableSource == null)
                throw new ArgumentNullException(nameof(tableSource));

            m_tables.Add(tableSource);
            m_tableLookup.Add(tableSource.TableName, m_tables.Count - 1);
        }

        /// <summary>
        /// Replies with all of the tables with their schema
        /// </summary>
        /// <returns></returns>
        public void RequestAllTablesWithSchema(IMetadataEncoder encoder)
        {
            foreach (var table in m_tables)
            {
                encoder.UseTable(table.TableIndex);
                encoder.AddTable(table.MajorVersion, table.MinorVersion, table.TableName, table.TableFlags);
                foreach (var column in table.Columns)
                {
                    encoder.AddColumn(column.Index, column.Name, column.Type);
                }
            }
        }

        public byte[] RequestTableData(IMetadataEncoder encoder, string tableName, Guid majorVersion = default(Guid), long minorVersion = 0, dynamic permissionsFilter = null)
        {
            return m_tables[m_tableLookup[tableName]].RequestTableData(encoder, majorVersion, minorVersion, permissionsFilter);
        }


    }
   
}
