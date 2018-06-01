using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("SubscribeResponse")]
    public class CommandSubscribeResponse
        : DocumentObject<CommandSubscribeResponse>
    {
        [DocumentField()]
        public int BinaryChannelCode { get; private set; }
        [DocumentField()]
        public Guid EncodingMethod { get; private set; }

        public CommandSubscribeResponse(int binaryChannelCode, Guid encodingMethod)
        {
            BinaryChannelCode = binaryChannelCode;
            EncodingMethod = encodingMethod;
        }

        //Exists to support CtpSerializable
        private CommandSubscribeResponse()
        { }

        public static explicit operator CommandSubscribeResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }

    }
}
