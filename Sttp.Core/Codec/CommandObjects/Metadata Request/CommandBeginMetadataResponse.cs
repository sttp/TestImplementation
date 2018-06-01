using System;
using System.Collections.Generic;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("BeginMetadataResponse")]
    public class CommandBeginMetadataResponse
        : DocumentObject<CommandBeginMetadataResponse>
    {
        [DocumentField()]
        public int BinaryChannelCode { get; private set; }
        [DocumentField()]
        public Guid EncodingMethod { get; private set; }
        [DocumentField()]
        public Guid RuntimeID { get; private set; }
        [DocumentField()]
        public long VersionNumber { get; private set; }
        [DocumentField()]
        public string TableName { get; private set; }
        [DocumentField()]
        public List<MetadataColumn> Columns { get; private set; }

        public CommandBeginMetadataResponse(int binaryChannelCode, Guid encodingMethod, Guid runtimeID, long versionNumber, string tableName, List<MetadataColumn> columns)
        {
            BinaryChannelCode = binaryChannelCode;
            EncodingMethod = encodingMethod;
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
            TableName = tableName;
            Columns = columns;
        }
        //Exists to support CtpSerializable
        private CommandBeginMetadataResponse()
        { }

        public static explicit operator CommandBeginMetadataResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
