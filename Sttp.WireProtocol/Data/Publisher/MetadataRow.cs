using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Data.Publisher
{
    public class MetadataRow
    {
        public int RecordID;
        public List<MetadataField> Fields;

        public MetadataRow(int recordID)
        {
            RecordID = recordID;
            Fields = new List<MetadataField>();
        }

        internal void FillData(int tableID, MetadataChangeLog changeLog, MetadataColumn column, object value)
        {
            MetadataField field;

            while (Fields.Count < column.ColumnIndex)
            {
                Fields.Add(null);
            }
            field = Fields[column.ColumnIndex];
            if (field == null)
            {
                field = new MetadataField();
                Fields[column.ColumnIndex] = field;
                changeLog.AddField(column.DataSetColumnIndex, RecordID);
            }
            byte[] encoding = column.Encode(value);
            if (!field.Value.SequenceEqual(encoding))
            {
                changeLog.AddFieldValue(tableID, column.ColumnIndex, RecordID, encoding);
                field.Value = encoding;
            }
        }
    }
}