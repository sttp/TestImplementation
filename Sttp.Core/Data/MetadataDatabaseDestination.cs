using System;
using System.Collections.Generic;
using System.Data;
using Sttp.WireProtocol;
using Sttp.WireProtocol.GetMetadataResponse;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataDatabaseDestination
    {
        private Dictionary<string, short> m_tableLookup;
        private List<MetadataTableDestination> m_tables;

        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid MajorVersion;

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long MinorVersion;

        public MetadataDatabaseDestination()
        {
            m_tables = new List<MetadataTableDestination>();
            m_tableLookup = new Dictionary<string, short>();
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
        public void Process(Sttp.WireProtocol.GetMetadataResponse.Decoder decoder)
        {
            Sttp.WireProtocol.GetMetadataResponse.Cmd command;
            while ((command = decoder.NextCommand()) != null)
            {
                switch (command.SubCommand)
                {
                    case SubCommand.Invalid:
                        break;
                    case SubCommand.DatabaseVersion:
                        var db = command.DatabaseVersion;
                        MajorVersion = db.MajorVersion;
                        MinorVersion = db.MinorVersion;
                        break;
                    case SubCommand.Clear:
                        break;
                    case SubCommand.DefineTable:
                        {
                            var cmd = command.DefineTable;
                            var table = new MetadataTableDestination(cmd.TableName, cmd.TableIndex, cmd.TableFlags);
                            m_tableLookup.Add(table.TableName, cmd.TableIndex);
                            while (m_tables.Count <= table.TableIndex)
                            {
                                m_tables.Add(null);
                            }
                            m_tables[cmd.TableIndex] = table;
                        }
                        break;
                    case SubCommand.DefineColumn:
                        m_tables[command.DefineColumn.TableIndex].ProcessCommand(command);
                        break;
                    case SubCommand.DefineRow:
                        m_tables[command.DefineRow.TableIndex].ProcessCommand(command);
                        break;
                    case SubCommand.DefineValue:
                        m_tables[command.DefineValue.TableIndex].ProcessCommand(command);
                        break;
                    case SubCommand.RemoveRow:
                        m_tables[command.RemoveRow.TableIndex].ProcessCommand(command);
                        break;
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
