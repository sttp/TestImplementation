using System;
using System.Text;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTableUpdate
    {
        public string TableName;
        public long LastModifiedRevision;

        public MetadataSchemaTableUpdate(string tableName, long lastModifiedRevision)
        {
            TableName = tableName;
            LastModifiedRevision = lastModifiedRevision;
        }

        public MetadataSchemaTableUpdate(SttpMarkupElement element)
        {
            if (element.ElementName != "TableRecord")
                throw new Exception("Invalid command");

            TableName = (string)element.GetValue("TableName");
            LastModifiedRevision = (long)element.GetValue("LastModifiedRevision");
            element.ErrorIfNotHandled();
        }

        public MetadataSchemaTableUpdate(ByteReader reader)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
        }

        public void Save(ByteWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(MetadataSchemaTables) + ")");
            builder.Append(linePrefix); builder.AppendLine($"TableName: {TableName} ");
            builder.Append(linePrefix); builder.AppendLine($"LastModifiedRevision: {LastModifiedRevision} ");
        }

        public void Save(SttpMarkupWriter sml)
        {
            using (sml.StartElement("TableRecord"))
            {
                sml.WriteValue("TableName", TableName);
                sml.WriteValue("LastModifiedRevision", LastModifiedRevision);
            }
        }
    }
}