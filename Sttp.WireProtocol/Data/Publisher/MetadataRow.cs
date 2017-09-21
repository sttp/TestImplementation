using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Data.Publisher
{
    public class MetadataRow
    {
        public int RowIndex;
        public List<MetadataField> Fields;

        public MetadataRow(int rowIndex)
        {
            RowIndex = rowIndex;
            Fields = new List<MetadataField>();
        }

        internal void FillData(MetadataChangeLog changeLog, MetadataColumn column, object value)
        {
            MetadataField field;

            while (Fields.Count < column.Index)
            {
                Fields.Add(null);
            }
            field = Fields[column.Index];
            byte[] encoding = column.Encode(value);
            if (field == null)
            {
                field = new MetadataField();
                Fields[column.Index] = field;
                field.Value = encoding;
                changeLog.AddValue(column.Index, RowIndex, encoding);
            }
            else if (!field.Value.SequenceEqual(encoding))
            {
                changeLog.AddValue(column.Index, RowIndex, encoding);
                field.Value = encoding;
            }

        }
    }
}