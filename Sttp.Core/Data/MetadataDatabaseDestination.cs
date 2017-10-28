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
    public class MetadataDatabaseDestination
    {
        private Dictionary<string, int> m_tableLookup;
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
        public void Process(MetadataDecoder decoder)
        {
            IMetadataParams command;
            while ((command = decoder.NextCommand()) != null)
            {
                switch (command.Command)
                {
                    case MetadataCommand.AddTable:
                        {
                            var cmd = command as MetadataAddTableParams;
                            var table = new MetadataTableDestination(cmd.TableName, cmd.TableIndex, cmd.TableFlags);
                            m_tableLookup.Add(table.TableName, cmd.TableIndex);
                            while (m_tables.Count <= table.TableIndex)
                            {
                                m_tables.Add(null);
                            }
                            m_tables[cmd.TableIndex] = table;
                        }
                        break;
                    case MetadataCommand.AddColumn:
                        m_tables[(command as MetadataAddColumnParams).TableIndex].ProcessCommand(command);
                        break;
                    case MetadataCommand.AddValue:
                        m_tables[(command as MetadataAddValueParams).TableIndex].ProcessCommand(command);
                        break;
                    case MetadataCommand.DeleteRow:
                        m_tables[(command as MetadataDeleteRowParams).TableIndex].ProcessCommand(command);
                        break;
                    case MetadataCommand.AddRow:
                        m_tables[(command as MetadataAddRowParams).TableIndex].ProcessCommand(command);
                        break;
                    case MetadataCommand.DatabaseVersion:
                        var db = command as MetadataDatabaseVersionParams;
                        MajorVersion = db.MajorVersion;
                        MinorVersion = db.MinorVersion;
                        break;
                    case MetadataCommand.GetTable:
                    case MetadataCommand.Invalid:
                    case MetadataCommand.Clear:
                    case MetadataCommand.GetQuery:
                    case MetadataCommand.SyncDatabase:
                    case MetadataCommand.SyncTableOrQuery:
                    case MetadataCommand.GetDatabaseSchema:
                    case MetadataCommand.GetDatabaseVersion:
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
