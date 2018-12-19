using System;
using System.Collections.Generic;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("BeginMetadataResponse")]
    public class CommandBeginMetadataResponse
        : CommandObject<CommandBeginMetadataResponse>
    {
        [CommandField()]
        public ulong BinaryChannelCode { get; private set; }
        [CommandField()]
        public Guid EncodingMethod { get; private set; }
        [CommandField()]
        public Guid RuntimeID { get; private set; }
        [CommandField()]
        public long VersionNumber { get; private set; }
        [CommandField()]
        public string TableName { get; private set; }
        [CommandField()]
        public List<MetadataColumn> Columns { get; private set; }

        public CommandBeginMetadataResponse(ulong binaryChannelCode, Guid encodingMethod, Guid runtimeID, long versionNumber, string tableName, List<MetadataColumn> columns)
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

        public static explicit operator CommandBeginMetadataResponse(CtpCommand obj)
        {
            return FromDocument(obj);
        }
    }
}
