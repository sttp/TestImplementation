using System;
using System.Collections.Generic;
using System.Text;
using Sttp.Codec;

namespace Sttp.Codec
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

        public void Save(SttpMarkupWriter sml)
        {
            using (sml.StartElement("TableRecord"))
            {
                sml.WriteValue("TableName", TableName);
                sml.WriteValue("LastModifiedRevision", LastModifiedRevision);
                using (sml.StartElement("Columns"))
                {
                    foreach (var item in Columns)
                    {
                        item.Save(sml);
                    }
                }
                using (sml.StartElement("ForeignKeys"))
                {
                    foreach (var item in ForeignKeys)
                    {
                        item.Save(sml);
                    }
                }
            }
        }
    }
}