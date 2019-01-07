using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("EndMetadataResponse")]
    public class CommandEndMetadataResponse
        : CommandObject<CommandEndMetadataResponse>
    {
        [CommandField()]
        public ulong BinaryChannelCode { get; private set; }
        [CommandField()]
        public int RowCount { get; private set; }

        public CommandEndMetadataResponse(ulong binaryChannelCode, int rowCount)
        {
            BinaryChannelCode = binaryChannelCode;
            RowCount = rowCount;
        }

        //Exists to support CtpSerializable
        private CommandEndMetadataResponse()
        { }

        public static explicit operator CommandEndMetadataResponse(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
