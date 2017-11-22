using System;
using System.Collections.Generic;
using System.Linq;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    public class MetadataRow
    {
        public SttpValue Key;
        public SttpValueSet Fields;
        private SttpValueSet m_changes;
        public MetadataRow[] ForeignKeys;
        public long Revision;

        public MetadataRow(SttpValue key, SttpValueSet fields)
        {
            Key = key;
            Fields = fields;
            ForeignKeys = new MetadataRow[fields.Values.Count];
        }

        public void UpdateRow(SttpValueSet fields)
        {
            if (m_changes == null)
            {
                if (Fields != fields)
                {
                    m_changes = fields;
                }
            }
            else
            {
                m_changes = fields;
            }
        }

        public void RollbackChanges()
        {
            m_changes = null;
        }

        public bool CommitChanges(long revision, bool refreshSchema)
        {
            if (refreshSchema)
            {
                Revision = revision;
            }
            if (m_changes != null)
            {
                Fields = m_changes;
                Revision = revision;
                m_changes = null;
                return true;
            }
            return false;

        }

    }
}
