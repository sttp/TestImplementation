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

        public MetadataSchemaTableUpdate(CtpDocumentElement documentElement)
        {
            TableName = (string)documentElement.GetValue("TableName");
            LastModifiedVersionNumber = (long)documentElement.GetValue("LastModifiedVersionNumber");
            documentElement.ErrorIfNotHandled();
        }

        public void Save(CtpDocumentWriter sml)
        {
            sml.WriteValue("TableName", TableName);
            sml.WriteValue("LastModifiedVersionNumber", LastModifiedVersionNumber);
        }
    }
}