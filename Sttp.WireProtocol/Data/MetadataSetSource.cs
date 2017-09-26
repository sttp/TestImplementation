using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using Sttp.WireProtocol;
using ValueType = Sttp.WireProtocol.ValueType;

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
        public byte[] RequestAllTablesWithSchema()
        {
            MemoryStream stream = new MemoryStream();
            stream.Write((byte)0); //Version Code
            stream.Write(m_tables.Count); //Number of tables
            foreach (var table in m_tables)
            {
                stream.Write(table.InstanceID);
                stream.Write(table.TransactionID);
                stream.Write(table.TableName);
                stream.Write(table.TableIndex);
                stream.Write(table.IsMappedToDataPoint);
                stream.Write(table.Columns.Count);
                foreach (var column in table.Columns)
                {
                    stream.Write(column.Index);
                    stream.Write(column.Name);
                    stream.Write((byte)column.Type);
                }
            }
            return stream.ToArray();
        }

        public byte[] RequestTableData(string tableName, Guid cachedInstanceID = default(Guid), long transaction = 0, dynamic permissionsFilter = null)
        {
            return m_tables[m_tableLookup[tableName]].RequestTableData(cachedInstanceID, transaction, permissionsFilter);
        }


    }
   
}
