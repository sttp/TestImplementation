using System;
using System.Collections.Generic;
using System.Linq;
using Sttp.WireProtocol;

namespace Sttp.Data
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

        internal void FillData(short tableIndex, MetadataChangeLog changeLog, MetadataColumn column, object value)
        {
            MetadataField field;

            while (Fields.Count <= column.Index)
            {
                Fields.Add(null);
            }
            field = Fields[column.Index];

            SttpValue encoding = column.Encode(value);
            if (field == null)
            {
                field = new MetadataField();
                Fields[column.Index] = field;
                field.Value = encoding;
                changeLog.AddValue(tableIndex, column.Index, RowIndex, encoding);
            }
            else if (field.Value != encoding)
            {
                changeLog.AddValue(tableIndex, column.Index, RowIndex, encoding);
                field.Value = encoding;
            }

        }

        public void ProcessCommand(Sttp.WireProtocol.GetMetadataResponse.CmdAddValue patch)
        {
            while (Fields.Count <= patch.ColumnIndex)
            {
                Fields.Add(null);
            }
            if (Fields[patch.ColumnIndex] == null)
            {
                Fields[patch.ColumnIndex] = new MetadataField();
            }
            Fields[patch.ColumnIndex].Value = patch.Value;
        }
    }
}
