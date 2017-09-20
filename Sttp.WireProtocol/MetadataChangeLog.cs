using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataChangeLog
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid InstanceID { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long TransactionID { get; private set; }

        private SortedList<long, MetadataPatchDetails> m_revisions = new SortedList<long, MetadataPatchDetails>();

        /// <summary>
        /// Gets/Sets if transaction logging is supported for changes made to this data set.
        /// It's recommended that this be turned off if large changes will occur to the set. 
        /// </summary>
        public bool LogRevisions
        {
            get
            {
                return InstanceID != Guid.Empty;
            }
            set
            {
                if (LogRevisions != value)
                {
                    if (value)
                    {
                        m_revisions.Clear();
                        InstanceID = Guid.NewGuid();
                        TransactionID = 0;
                    }
                    else
                    {
                        m_revisions.Clear();
                        InstanceID = Guid.Empty;
                    }
                }

            }
        }

       


        public bool TryBuildPatchData(Guid instanceID, long cachedVersionID, out List<MetadataPatchDetails> patchingDetails)
        {
            patchingDetails = null;
            if (InstanceID != instanceID || !LogRevisions)
                return false;

            int starting = m_revisions.IndexOfKey(cachedVersionID);
            if (starting >= 0)
            {
                patchingDetails = new List<MetadataPatchDetails>();
                for (int x = starting; x < m_revisions.Count; x++)
                {
                    patchingDetails.Add(m_revisions.Values[x]);
                }
                return true;
            }
            return false;
        }

        public void ClearRevisionHistory(long clearBeforeRevision)
        {
            while (m_revisions.Count > 0 && m_revisions.Keys[0] < clearBeforeRevision)
            {
                m_revisions.RemoveAt(0);
            }
        }


        public void AddTable(MetadataTable table)
        {

        }

        public void AddColumn(int tableID, MetadataColumn column)
        {
            throw new NotImplementedException();
        }

        public void AddRow(int tableId, MetadataRow row)
        {
            throw new NotImplementedException();
        }

        public void AddField(int tableID, int columnColumnID, int recordID)
        {
            throw new NotImplementedException();
        }

        public void AddFieldValue(int tableID, int columnColumnID, int recordID, byte[] encoding)
        {
            throw new NotImplementedException();
        }
    }
}