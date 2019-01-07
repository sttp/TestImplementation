using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("UnsubscribeResponse")]
    public class CommandUnsubscribeResponse
        : CommandObject<CommandUnsubscribeResponse>
    {
        [CommandField()]
        public int BinaryChannelCode;

        public CommandUnsubscribeResponse(int binaryChannelCode)
        {
            BinaryChannelCode = binaryChannelCode;
        }

        //Exists to support CtpSerializable
        private CommandUnsubscribeResponse()
        {

        }

        public static explicit operator CommandUnsubscribeResponse(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}
