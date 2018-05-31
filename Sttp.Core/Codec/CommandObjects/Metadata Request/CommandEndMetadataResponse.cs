using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandEndMetadataResponse
    {
        [CtpSerializeField()]
        public int BinaryChannelCode { get; private set; }
        [CtpSerializeField()]
        public int RowCount { get; private set; }

        public CommandEndMetadataResponse(int binaryChannelCode, int rowCount)
        {
            BinaryChannelCode = binaryChannelCode;
            RowCount = rowCount;
        }

        //Exists to support CtpSerializable
        private CommandEndMetadataResponse() { }
    }
}
