using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.Data;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
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
        public void Fill(MetadataDecoder decoder)
        {
            var method = decoder.NextCommand();
            switch (method)
            {
                case CommandCode3.MetadataClear:
                    decoder.Clear();
                    m_tables.Clear();
                    m_tableLookup.Clear();
                    break;
                case CommandCode3.MetadataAddTable:
                    {
                        decoder.AddTable(out Guid instanceID, out long transactionID, out string tableName, out int tableIndex, out bool isMappedToDataPoint);
                        var table = new MetadataTableDestination(instanceID, transactionID, tableName, tableIndex, isMappedToDataPoint);
                        m_tableLookup.Add(table.TableName, tableIndex);
                        while (m_tables.Count <= table.TableIndex)
                        {
                            m_tables.Add(null);
                        }
                        m_tables[tableIndex] = table;
                    }
                    break;
                case CommandCode3.MetadataDeleteTable:
                    {
                        decoder.DeleteTable(out int tableIndex);
                        m_tableLookup.Remove(m_tables[tableIndex].TableName);
                        m_tables[tableIndex] = null;
                    }
                    break;
                case CommandCode3.MetadataUpdateTable:
                    {
                        decoder.UpdateTable(out int tableIndex, out long transactionID);
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case CommandCode3.MetadataAddColumn:
                    {
                        decoder.AddColumn(out int tableIndex, out int columnIndex, out string columnName, out ValueType columnType);
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case CommandCode3.MetadataDeleteColumn:
                    {
                        decoder.DeleteColumn(out int tableIndex, out int columnIndex);
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case CommandCode3.MetadataAddValue:
                    {
                        decoder.AddValue(out int tableIndex, out int columnIndex, out int rowIndex, out byte[] value);
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case CommandCode3.MetadataDeleteRow:
                    {
                        decoder.DeleteRow(out int tableIndex, out int rowIndex);
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //int tableCount = stream.ReadInt32();
            //while (tableCount > 0)
            //{


            //    int columnCount = stream.ReadInt32();
            //    while (columnCount > 0)
            //    {
            //        int index = stream.ReadInt32();
            //        string name = stream.ReadString();
            //        ValueType type = (ValueType)stream.ReadNextByte();
            //        columnCount--;

            //        m_tables[tableIndex].AddColumn(index, name, type);
            //    }

            //    tableCount--;
            //}
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
