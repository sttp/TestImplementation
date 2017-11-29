using System.Text;
using Sttp.Codec;

namespace Sttp
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

        public MetadataSchemaTableUpdate(PayloadReader reader)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
        }

        public void Save(PayloadWriter writer)
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
    }
}