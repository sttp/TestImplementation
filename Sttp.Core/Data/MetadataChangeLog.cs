using System;
using System.Collections.Generic;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.Data
{
    public class MetadataChangeLog
    {
        /// <summary>
        /// If logging is enabled, this is the Major version of the table. If disabled, this value is <see cref="Guid.Empty"/>.
        /// </summary>
        public Guid MajorVersion { get; private set; }

        /// <summary>
        /// Identifies the number of changes that have occurred since the major version was established.
        /// </summary>
        public long MinorVersion { get; private set; }

        private SortedList<long, MetadataChangeLogRecord> m_revisions = new SortedList<long, MetadataChangeLogRecord>();

        /// <summary>
        /// Gets/Sets if changes will be tracked so a client can patch their local metadata repository.
        /// </summary>
        public bool TrackChanges
        {
            get
            {
                return MajorVersion != Guid.Empty;
            }
            set
            {
                if (TrackChanges != value)
                {
                    if (value)
                    {
                        m_revisions.Clear();
                        MajorVersion = Guid.NewGuid();
                        MinorVersion = 0;
                    }
                    else
                    {
                        m_revisions.Clear();
                        MajorVersion = Guid.Empty;
                    }
                }

            }
        }


        /// <summary>
        /// Attempts to synchronize a client's version of the metadata tables with the current server version.
        /// </summary>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        /// <param name="changes"></param>
        /// <returns></returns>
        /// <remarks>
        /// This will fail if the revision history doesn't go back far enough to patch the metadata, or if the MajorVersion does not match.
        /// </remarks>
        public bool TrySyncTableVersion(Guid majorVersion, long minorVersion, out List<MetadataChangeLogRecord> changes)
        {
            changes = null;
            if (MajorVersion != majorVersion || !TrackChanges)
                return false;

            int starting = m_revisions.IndexOfKey(minorVersion);
            if (starting >= 0)
            {
                changes = new List<MetadataChangeLogRecord>();
                for (int x = starting; x < m_revisions.Count; x++)
                {
                    changes.Add(m_revisions.Values[x]);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all changes to a table before this minor version.
        /// </summary>
        /// <param name="clearBeforeMinorVersion"></param>
        public void ClearRevisionHistory(long clearBeforeMinorVersion)
        {
            //ToDo: Since minor version always indexes by 1, having an indexed queue as a change log will perform better.
            while (m_revisions.Count > 0 && m_revisions.Keys[0] < clearBeforeMinorVersion)
            {
                m_revisions.RemoveAt(0);
            }
        }

        public void AddColumn(short tableIndex, MetadataColumn column)
        {
            if (TrackChanges)
                m_revisions[++MinorVersion] = MetadataChangeLogRecord.AddColumn(tableIndex, column.Index, column.Name, column.Type);
        }

        public void AddValue(short tableIndex, short columnIndex, int recordIndex, byte[] value)
        {
            if (TrackChanges)
                m_revisions[++MinorVersion] = MetadataChangeLogRecord.AddValue(tableIndex, columnIndex, recordIndex, value);
        }

        public void DeleteRow(short tableIndex, int rowIndex)
        {
            if (TrackChanges)
                m_revisions[++MinorVersion] = MetadataChangeLogRecord.DeleteRow(tableIndex, rowIndex);
        }

        public void AddTable(short tableIndex, string tableName, TableFlags flags)
        {
            if (TrackChanges)
                m_revisions[++MinorVersion] = MetadataChangeLogRecord.AddTable(tableIndex, tableName, flags);
        }
    }
}