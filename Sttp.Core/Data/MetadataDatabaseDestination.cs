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
            Sttp.WireProtocol.GetMetadataResponse.ICmd command;
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
                    case SubCommand.AddTable:
                        {
                            var cmd = command.AddTable;
                            var table = new MetadataTableDestination(cmd.TableName, cmd.TableIndex, cmd.TableFlags);
                            m_tableLookup.Add(table.TableName, cmd.TableIndex);
                            while (m_tables.Count <= table.TableIndex)
                            {
                                m_tables.Add(null);
                            }
                            m_tables[cmd.TableIndex] = table;
                        }
                        break;
                    case SubCommand.AddColumn:
                        m_tables[command.AddColumn.TableIndex].ProcessCommand(command);
                        break;
                    case SubCommand.AddRow:
                        m_tables[command.AddRow.TableIndex].ProcessCommand(command);
                        break;
                    case SubCommand.AddValue:
                        m_tables[command.AddValue.TableIndex].ProcessCommand(command);
                        break;
                    case SubCommand.DeleteRow:
                        m_tables[command.DeleteRow.TableIndex].ProcessCommand(command);
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
