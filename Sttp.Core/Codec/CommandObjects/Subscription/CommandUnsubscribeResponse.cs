using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("UnsubscribeResponse")]
    public class CommandUnsubscribeResponse
        : DocumentObject<CommandUnsubscribeResponse>
    {
        [DocumentField()]
        public int BinaryChannelCode;

        public CommandUnsubscribeResponse(int binaryChannelCode)
        {
            BinaryChannelCode = binaryChannelCode;
        }

        //Exists to support CtpSerializable
        private CommandUnsubscribeResponse()
        {

        }

        public static explicit operator CommandUnsubscribeResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
