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

        public MetadataForeignKey(ByteReader reader)
        {
            ColumnName = reader.ReadString();
            ForeignTableName = reader.ReadString();
        }

        public void Save(ByteWriter writer)
        {
            writer.Write(ColumnName);
            writer.Write(ForeignTableName);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix);
            builder.AppendLine($"({nameof(MetadataForeignKey)} {ColumnName} -> {ForeignTableName}");
        }
    }
}