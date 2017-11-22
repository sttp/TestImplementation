using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sttp.Data
{
    public class MetadataTable
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName;

        /// <summary>
        /// Indicates that this table has 1 record for each measurement and can be used
        /// in filtering. False means this is a ancillary table that won't be understood by the
        /// API, but is used for the application layer.
        /// </summary>
        public TableFlags TableFlags;

        /// <summary>
        /// All possible columns that are defined for the table.
        /// </summary>
        public List<MetadataColumn> Columns;

        /// <summary>
        /// All possible rows.
        /// </summary>
        public Dictionary<SttpValue, MetadataRow> Rows;

        /// <summary>
        /// Contains pending changes to the table schema if the changes included adding or deleting rows.
        /// </summary>
        private Dictionary<SttpValue, MetadataRow> m_pendingChanges;

        public long LastModifiedRevision;

        public MetadataTable(string tableName, TableFlags tableFlags, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            TableName = tableName;
            TableFlags = tableFlags;
            Columns = new List<MetadataColumn>();
            Rows = new Dictionary<SttpValue, MetadataRow>();
            for (var index = 0; index < columns.Count; index++)
            {
                var item = columns[index];
                Columns.Add(new MetadataColumn(item.Item1, item.Item2));
            }
        }

        public void AddOrUpdateRow(SttpValue key, SttpValueSet fields)
        {
            //Check the existing value, only if the row has changed do we update the changes in the database.
            if (m_pendingChanges == null)
            {
                if (Rows.TryGetValue(key, out MetadataRow existing))
                {
                    existing.UpdateRow(fields);
                    return;
                }
                m_pendingChanges = new Dictionary<SttpValue, MetadataRow>(Rows);
            }
            else
            {
                if (m_pendingChanges.TryGetValue(key, out MetadataRow existing))
                {
                    existing.UpdateRow(fields);
                    return;
                }
            }
            m_pendingChanges.Add(key, new MetadataRow(key, fields));
        }

        public void DeleteRow(SttpValue primaryKey)
        {
            if (m_pendingChanges == null)
            {
                m_pendingChanges = new Dictionary<SttpValue, MetadataRow>(Rows);
            }
            m_pendingChanges.Remove(primaryKey);
        }

        public void RollbackChanges()
        {
            m_pendingChanges = null;
            foreach (var metadataRow in Rows.Values)
            {
                metadataRow.RollbackChanges();
            }
        }

        public bool CommitChanges(long revision, bool schemaRefresh)
        {
            bool hasChanged = schemaRefresh;

            if (m_pendingChanges != null)
            {
                hasChanged = true;
                Rows = m_pendingChanges;
                m_pendingChanges = null;
            }
            foreach (var metadataRow in Rows.Values)
            {
                hasChanged |= metadataRow.CommitChanges(revision, schemaRefresh);
            }
            if (hasChanged)
            {
                LastModifiedRevision = revision;
            }
            return hasChanged;
        }
    }
}
