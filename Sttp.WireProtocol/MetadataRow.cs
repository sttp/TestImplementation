using System;
using System.Collections.Generic;

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
    }
}