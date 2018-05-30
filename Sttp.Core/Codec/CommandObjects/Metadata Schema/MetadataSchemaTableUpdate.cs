using System;
using System.Text;
using CTP;
using CTP.Serialization;
using Sttp.Codec;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class MetadataSchemaTableUpdate
    {
        [CtpSerializeField()]
        public string TableName;
        [CtpSerializeField()]
        public long LastModifiedVersionNumber;

        private MetadataSchemaTableUpdate()
        {

        }
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