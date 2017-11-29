using System;
using System.Collections.Generic;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class MetadataSchemaTables
    {
        public string TableName;
        public long LastModifiedRevision;
        public List<MetadataColumn> Columns = new List<MetadataColumn>();
        public List<MetadataForeignKey> ForeignKeys = new List<MetadataForeignKey>();

        public MetadataSchemaTables()
        {

        }

        public MetadataSchemaTables(PayloadReader reader)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
            Columns = reader.ReadList<MetadataColumn>();
            ForeignKeys = reader.ReadList<MetadataForeignKey>();
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
            writer.Write(Columns);
            writer.Write(ForeignKeys);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(MetadataSchemaTables) + ")");
            builder.Append(linePrefix); builder.AppendLine($"TableName: {TableName} ");
            builder.Append(linePrefix); builder.AppendLine($"LastModifiedRevision: {LastModifiedRevision} ");
            builder.Append(linePrefix); builder.AppendLine($"Columns Count {Columns.Count} ");
            foreach (var value in Columns)
            {
                value.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"ForeignKeys Count {ForeignKeys.Count} ");
            foreach (var value in ForeignKeys)
            {
                value.GetFullOutputString(linePrefix + " ", builder);
            }
        }
    }
}