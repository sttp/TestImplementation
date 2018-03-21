using System;
using System.Text;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTableUpdate
    {
        public string TableName;
        public long LastModifiedSequenceNumber;

        public MetadataSchemaTableUpdate(string tableName, long lastModifiedSequenceNumber)
        {
            TableName = tableName;
            LastModifiedSequenceNumber = lastModifiedSequenceNumber;
        }

        public MetadataSchemaTableUpdate(SttpMarkupElement element)
        {
            TableName = (string)element.GetValue("TableName");
            LastModifiedSequenceNumber = (long)element.GetValue("LastModifiedSequenceNumber");
            element.ErrorIfNotHandled();
        }

        public void Save(SttpMarkupWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedSequenceNumber", LastModifiedSequenceNumber);
        }
    }
}