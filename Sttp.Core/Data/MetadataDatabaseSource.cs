using System;
using System.Collections.Generic;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataDatabaseSource
    {
        private MetadataChangeLog m_changeLog = new MetadataChangeLog();

        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid MajorVersion => m_changeLog.MajorVersion;

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long MinorVersion => m_changeLog.MinorVersion;

        /// <summary>
        /// Gets/Sets if transaction logging is supported for changes made to this data set.
        /// It's recommended that this be turned off if large changes will occur to the set. 
        /// </summary>
        public bool TrackChanges
        {
            get => m_changeLog.TrackChanges;
            set => m_changeLog.TrackChanges = value;
        }

        private Dictionary<string, short> m_tableLookup;
        private List<MetadataTableSource> m_tables;

        public MetadataDatabaseSource()
        {
            m_tables = new List<MetadataTableSource>();
            m_tableLookup = new Dictionary<string, short>();
        }

        public MetadataTableSource this[string tableName]
        {
            get
            {
                return m_tables[m_tableLookup[tableName]];
            }
        }

        public void AddTable(string tableName, TableFlags flags)
        {
            if (!m_tableLookup.ContainsKey(tableName))
            {
                var tbl = new MetadataTableSource(m_changeLog, (short)m_tables.Count, tableName, flags);
                m_tables.Add(tbl);
                m_tableLookup[tableName] = tbl.TableIndex;
                m_changeLog.AddTable(tbl.TableIndex, tableName, flags);
            }
        }

        //public void AddTable(MetadataTableSource tableSource)
        //{
        //    if (tableSource == null)
        //        throw new ArgumentNullException(nameof(tableSource));

        //    m_tables.Add(tableSource);
        //    m_tableLookup.Add(tableSource.TableName, m_tables.Count - 1);
        //}

        /// <summary>
        /// Replies with all of the tables with their schema
        /// </summary>
        /// <returns></returns>
        public void RequestAllTablesWithSchema(WireProtocol.GetMetadataSchemaResponse.Encoder encoder)
        {
            encoder.DatabaseVersion(MajorVersion, MinorVersion);
            foreach (var table in m_tables)
            {
                encoder.DefineTable(table.TableName, table.TableFlags, null);
                //foreach (var column in table.Columns)
                //{
                //    encoder.DefineColumn(table.TableIndex, column.Index, column.Name, (byte)column.TypeCode);
                //}
            }
        }

        public void RequestTableData(WireProtocol.GetMetadataResponse.Encoder encoder, short tableIndex, Guid majorVersion = default(Guid), long minorVersion = 0, MetadataTableFilter permissionsFilter = null)
        {
            if (m_changeLog.TrySyncTableVersion(majorVersion, minorVersion, out List<MetadataChangeLogRecord> data))
            {
                foreach (var record in data)
                {
                    if (permissionsFilter == null || permissionsFilter.Permit(record))
                    {
                        record.Save(encoder);
                    }
                }
                encoder.DatabaseVersion(MajorVersion, MinorVersion);
                return;
            }
            m_tables[tableIndex].RequestTableData(encoder, permissionsFilter);
            encoder.DatabaseVersion(MajorVersion, MinorVersion);

        }


    }

}
