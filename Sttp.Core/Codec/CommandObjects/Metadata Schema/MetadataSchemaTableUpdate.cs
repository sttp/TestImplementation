using System;
using System.Text;
using CTP;
using CTP.Serialization;
using Sttp.Codec;

namespace Sttp.Codec
{
    [CtpSerializable("MetadataSchemaTableUpdate")]
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
    }
}