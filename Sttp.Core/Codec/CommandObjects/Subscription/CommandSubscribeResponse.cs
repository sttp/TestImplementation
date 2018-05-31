using System;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable]
    public class CommandSubscribeResponse 
    {
        [CtpSerializeField()]
        public int BinaryChannelCode { get; private set; }
        [CtpSerializeField()]
        public Guid EncodingMethod { get; private set; }

        public CommandSubscribeResponse(int binaryChannelCode, Guid encodingMethod)
        {
            BinaryChannelCode = binaryChannelCode;
            EncodingMethod = encodingMethod;
        }

        //Exists to support CtpSerializable
        private CommandSubscribeResponse() { }
      
    }
}
