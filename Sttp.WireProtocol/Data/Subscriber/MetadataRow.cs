using System;
using System.Collections.Generic;

namespace Sttp.Data.Subscriber
{
    public class MetadataRow
    {
        public int RecordID;
        public Dictionary<int, MetadataField> Fields;

        public MetadataRow(int recordID)
        {
            RecordID = recordID;
            Fields = new Dictionary<int, MetadataField>();
        }

        public void ApplyPatch(MetadataPatchDetails patch)
        {
            switch (patch.ChangeType)
            {
                case MetadataChangeType.AddField:
                    Fields[patch.ColumnID] = new MetadataField();
                    break;
                case MetadataChangeType.AddFieldValue:
                    Fields[patch.ColumnID].Value = patch.Data;
                    break;
                default:
                    throw new NotSupportedException("Invalid patch type:");
            }
        }
    }
}