using System;
using System.Text;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTableUpdate
    {
        public string TableName;
        public long LastModifiedVersion;

        public MetadataSchemaTableUpdate(string tableName, long lastModifiedVersion)
        {
            TableName = tableName;
            LastModifiedVersion = lastModifiedVersion;
        }

        public MetadataSchemaTableUpdate(SttpMarkupElement element)
        {
            TableName = (string)element.GetValue("TableName");
            LastModifiedVersion = (long)element.GetValue("LastModifiedVersion");
            element.ErrorIfNotHandled();
        }

        public void Save(SttpMarkupWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedVersion", LastModifiedVersion);
        }
    }
}