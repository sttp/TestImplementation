using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CommandName("SubscribeResponse")]
    public class CommandSubscribeResponse
        : CommandObject<CommandSubscribeResponse>
    {
        [CommandField()]
        public int BinaryChannelCode { get; private set; }
        [CommandField()]
        public Guid EncodingMethod { get; private set; }

        public CommandSubscribeResponse(int binaryChannelCode, Guid encodingMethod)
        {
            BinaryChannelCode = binaryChannelCode;
            EncodingMethod = encodingMethod;
        }

        //Exists to support CtpSerializable
        private CommandSubscribeResponse()
        { }

        public static explicit operator CommandSubscribeResponse(CtpCommand obj)
        {
            return FromDocument(obj);
        }

    }
}
