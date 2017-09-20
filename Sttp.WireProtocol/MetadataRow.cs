using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.WireProtocol
{
    public class MetadataRow
    {
        public int RecordID;
        public Dictionary<string, MetadataField> Fields;

        public MetadataRow(int recordID)
        {
            RecordID = recordID;
            Fields = new Dictionary<string, MetadataField>();
        }

        public void FillData(MetadataColumn column, object value)
        {
            MetadataField field;
            if (!Fields.TryGetValue(column.ColumnName, out field))
            {
                field = new MetadataField();
                Fields[column.ColumnName] = field;
            }
            field.Value = column.Encode(value);
        }

        internal void FillData(int tableID, MetadataChangeLog changeLog, MetadataColumn column, object value)
        {
            MetadataField field;
            if (!Fields.TryGetValue(column.ColumnName, out field))
            {
                field = new MetadataField();
                Fields[column.ColumnName] = field;
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