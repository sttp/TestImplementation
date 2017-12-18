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