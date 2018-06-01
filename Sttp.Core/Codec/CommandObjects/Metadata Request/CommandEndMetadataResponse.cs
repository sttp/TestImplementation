using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("EndMetadataResponse")]
    public class CommandEndMetadataResponse
        : CtpDocumentObject<CommandEndMetadataResponse>
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
        private CommandEndMetadataResponse()
        { }

        public static explicit operator CommandEndMetadataResponse(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }
}
