using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class MetadataForeignKey
    {
        public readonly string ColumnName;
        public readonly string ForeignTableName;

        public MetadataForeignKey(string columnName, string foreignTableName)
        {
            ColumnName = columnName;
            ForeignTableName = foreignTableName;
        }

        public MetadataForeignKey(SttpMarkupElement element)
        {
            ColumnName = (string)element.GetValue("ColumnName");
            ForeignTableName = (string)element.GetValue("ForeignTableName");
            element.ErrorIfNotHandled();
        }

        public void Save(SttpMarkupWriter sml)
        {
            sml.WriteValue("ColumnName", ColumnName);
            sml.WriteValue("ForeignTableName", ForeignTableName);
        }
    }
}