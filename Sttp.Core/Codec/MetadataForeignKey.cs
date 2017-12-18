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
            if (element.ElementName != "Column")
                throw new Exception("Invalid command");

            ColumnName = (string)element.GetValue("ColumnName");
            ForeignTableName = (string)element.GetValue("ForeignTableName");
            element.ErrorIfNotHandled();
        }


        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix);
            builder.AppendLine($"({nameof(MetadataForeignKey)} {ColumnName} -> {ForeignTableName}");
        }

        public void Save(SttpMarkupWriter sml)
        {
            using (sml.StartElement("Column"))
            {
                sml.WriteValue("ColumnName", ColumnName);
                sml.WriteValue("ForeignTableName", ForeignTableName);
            }
        }
    }
}