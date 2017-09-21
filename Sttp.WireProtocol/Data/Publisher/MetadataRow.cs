using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Data.Publisher
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

        internal void FillData(int tableID, MetadataChangeLog changeLog, MetadataColumn column, object value)
        {
            MetadataField field;
            if (!Fields.TryGetValue(column.ColumnID, out field))
            {
                field = new MetadataField();
                Fields[column.ColumnID] = field;
                changeLog.AddField(tableID, column.ColumnID, RecordID);
            }
            byte[] encoding = column.Encode(value);
            if (!field.Value.SequenceEqual(encoding))
            {
                changeLog.AddFieldValue(tableID, column.ColumnID, RecordID, encoding);
                field.Value = encoding;
            }
        }
    }
}