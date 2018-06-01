using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("EndMetadataResponse")]
    public class CommandEndMetadataResponse
        : DocumentObject<CommandEndMetadataResponse>
    {
        [DocumentField()]
        public int BinaryChannelCode { get; private set; }
        [DocumentField()]
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
            return FromDocument(obj);
        }
    }
}
