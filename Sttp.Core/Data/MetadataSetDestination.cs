using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Sttp.IO;
using Sttp.WireProtocol;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;
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
        public void Fill(IMetadataDecoder decoder)
        {
            int tableIndex = -1;
            var method = decoder.NextCommand();
            switch (method)
            {
                case MetadataCommand.UseTable:
                {
                    decoder.UseTable(out tableIndex);
                }
                    break;
                case MetadataCommand.AddTable:
                    {
                        decoder.AddTable(out Guid majorVersion, out long minorVersion, out string tableName, out TableFlags tableFlags);
                        var table = new MetadataTableDestination(majorVersion, minorVersion, tableName, tableIndex, tableFlags);
                        m_tableLookup.Add(table.TableName, tableIndex);
                        while (m_tables.Count <= table.TableIndex)
                        {
                            m_tables.Add(null);
                        }
                        m_tables[tableIndex] = table;
                    }
                    break;
                case MetadataCommand.AddColumn:
                    {
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case MetadataCommand.AddValue:
                    {
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case MetadataCommand.DeleteRow:
                    {
                        m_tables[tableIndex].Fill(decoder);
                    }
                    break;
                case MetadataCommand.TableVersion:
                    {
                        decoder.TableVersion(out int tableIndex1, out Guid majorVersion, out long minorVersion);
                        m_tables[tableIndex1].Fill(decoder);
                    }
                    break;
                case MetadataCommand.AddRelationship:
                    {
                        decoder.AddRelationship(out int tableIndex1, out int columnIndex, out int foreignTableIndex);
                        m_tables[tableIndex1].Fill(decoder);
                    }
                    break;
                case MetadataCommand.GetTable:
                case MetadataCommand.SyncTable:
                case MetadataCommand.SelectAllTablesWithSchema:
                case MetadataCommand.GetAllTableVersions:
                    throw new NotSupportedException();
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
