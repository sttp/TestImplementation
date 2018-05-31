using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
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
