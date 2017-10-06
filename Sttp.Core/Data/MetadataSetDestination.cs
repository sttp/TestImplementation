using System;
using System.Collections.Generic;
using System.Data;
using Sttp.WireProtocol;
using Sttp.WireProtocol.Data;
using Sttp.WireProtocol.MetadataPacket;

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
        public void Process(IMetadataDecoder decoder)
        {
            int tableIndex = -1;

            IMetadataParams command;
            while ((command = decoder.NextCommand()) != null)
            {
                switch (command.Command)
                {
                    case MetadataCommand.UseTable:
                        tableIndex = (command as MetadataUseTableParams).TableIndex;
                        break;
                    case MetadataCommand.AddTable:
                    {
                        var cmd = command as MetadataAddTableParams;
                        var table = new MetadataTableDestination(cmd.MajorVersion, cmd.MinorVersion, cmd.TableName, tableIndex, cmd.TableFlags);
                        m_tableLookup.Add(table.TableName, tableIndex);
                        while (m_tables.Count <= table.TableIndex)
                        {
                            m_tables.Add(null);
                        }
                        m_tables[tableIndex] = table;
                    }
                        break;
                    case MetadataCommand.AddColumn:
                    case MetadataCommand.AddValue:
                    case MetadataCommand.DeleteRow:
                        m_tables[tableIndex].ProcessCommand(command);
                        break;
                    case MetadataCommand.TableVersion:
                        m_tables[(command as MetadataTableVersionParams).TableIndex].ProcessCommand(command);
                        break;
                    case MetadataCommand.AddRelationship:
                        throw new NotImplementedException();
                        break;
                    case MetadataCommand.GetTable:
                    case MetadataCommand.SyncTable:
                    case MetadataCommand.SelectAllTablesWithSchema:
                    case MetadataCommand.GetAllTableVersions:
                        throw new NotSupportedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
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
