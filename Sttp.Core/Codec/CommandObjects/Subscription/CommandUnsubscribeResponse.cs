using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("UnsubscribeResponse")]
    public class CommandUnsubscribeResponse
        : CtpDocumentObject<CommandUnsubscribeResponse>
    {
        [CtpSerializeField()]
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
            return ConvertFromDocument(obj);
        }
    }
}
