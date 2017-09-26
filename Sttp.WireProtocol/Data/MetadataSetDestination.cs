using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Sttp.IO;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data.Subscriber
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataSetDestination
    {
        private Dictionary<string, int> m_tableLookup;
        private List<MetadataTableDestination> m_tables;

        public MetadataSetDestination()
        {
            m_tables = new List<MetadataTableDestination>();
            m_tableLookup = new Dictionary<string, int>();
        }

        public MetadataTableDestination this[string tableName]
        {
            get
            {
                return m_tables[m_tableLookup[tableName]];
            }
        }

        /// <summary>
        /// Fills a new dataset with the results of RequestAllTablesWithSchema
        /// </summary>
        /// <returns></returns>
        public void FillDataSet(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            byte version = stream.ReadNextByte();
            if (version != 0)
                throw new VersionNotFoundException("Version not recognized");

            m_tables.Clear();
            m_tableLookup.Clear();

            int tableCount = stream.ReadInt32();
            while (tableCount > 0)
            {
                Guid instanceId = stream.ReadGuid();
                long transactionId = stream.ReadInt64();
                string tableName = stream.ReadString();
                int tableIndex = stream.ReadInt32();
                bool isMappedToDataPoint = stream.ReadBoolean();
                var table = new MetadataTableDestination(instanceId, transactionId, tableName, tableIndex, isMappedToDataPoint);
                m_tableLookup.Add(table.TableName, tableIndex);
                while (m_tables.Count <= table.TableIndex)
                {
                    m_tables.Add(null);
                }
                m_tables[tableIndex] = table;

                int columnCount = stream.ReadInt32();
                while (columnCount > 0)
                {
                    int index = stream.ReadInt32();
                    string name = stream.ReadString();
                    ValueType type = (ValueType)stream.ReadNextByte();
                    columnCount--;

                    m_tables[tableIndex].AddColumn(index, name, type);
                }

                tableCount--;
            }
        }

        public void FillTable(string tableName, byte[] patchDetails)
        {
            m_tables[m_tableLookup[tableName]].FillTable(patchDetails);
        }

        public DataSet CreateDataSet()
        {
            //takes the current metadata and makes a dataset from it.
            return null;
        }

        public DataTable CreateDataTable(string tableName)
        {
            //takes the current metadata and makes a table from it.
            return null;
        }

    }
}
