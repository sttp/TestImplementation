using System;
using System.Text;
using CTP;
using CTP.Serialization;
using Sttp.Codec;

namespace Sttp.Codec
{
    public class MetadataSchemaTableUpdate
        : CommandObject<MetadataSchemaTableUpdate>
    {
        [CommandField()]
        public string TableName;
        [CommandField()]
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