using System;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTableUpdate
    {
        public string TableName;
        public long LastModifiedVersionNumber;

        public MetadataSchemaTableUpdate(string tableName, long lastModifiedVersionNumber)
        {
            TableName = tableName;
            LastModifiedVersionNumber = lastModifiedVersionNumber;
        }

        public MetadataSchemaTableUpdate(CtpMarkupElement element)
        {
            TableName = (string)element.GetValue("TableName");
            LastModifiedVersionNumber = (long)element.GetValue("LastModifiedVersionNumber");
            element.ErrorIfNotHandled();
        }

        public void Save(CtpMarkupWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedVersionNumber", LastModifiedVersionNumber);
        }
    }
}