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
            TableName = (string)element.GetValue("TableName");
            LastModifiedRevision = (long)element.GetValue("LastModifiedRevision");
            element.ErrorIfNotHandled();
        }

        public void Save(SttpMarkupWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedRevision", LastModifiedRevision);
        }
    }
}