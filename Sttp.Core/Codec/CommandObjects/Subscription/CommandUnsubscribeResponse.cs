using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable("UnsubscribeResponse")]
    public class CommandUnsubscribeResponse
    {
        [CtpSerializeField()]
        public int BinaryChannelCode;

        public CommandUnsubscribeResponse(int binaryChannelCode)
        {
            BinaryChannelCode = binaryChannelCode;
        }

        //Exists to support CtpSerializable
        private CommandUnsubscribeResponse() { }

    }
}
