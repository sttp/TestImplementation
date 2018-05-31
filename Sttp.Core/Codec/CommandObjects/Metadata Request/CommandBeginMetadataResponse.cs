using System;
using System.Collections.Generic;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable("BeginMetadataResponse")]
    public class CommandBeginMetadataResponse
    {
        [CtpSerializeField()]
        public int BinaryChannelCode { get; private set; }
        [CtpSerializeField()]
        public Guid EncodingMethod { get; private set; }
        [CtpSerializeField()]
        public Guid RuntimeID { get; private set; }
        [CtpSerializeField()]
        public long VersionNumber { get; private set; }
        [CtpSerializeField()]
        public string TableName { get; private set; }
        [CtpSerializeField()]
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
        private CommandBeginMetadataResponse() { }

    }
}
