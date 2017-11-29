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
    }
}