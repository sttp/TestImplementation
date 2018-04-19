using System;
using CTP;

namespace Sttp.Codec
{
    public class CommandSubscribeResponse : DocumentCommandBase
    {
        public readonly int BinaryChannelCode;
        public readonly Guid EncodingMethod;

        public CommandSubscribeResponse(int binaryChannelCode, Guid encodingMethod)
            : base("SubscribeResponse")
        {
            BinaryChannelCode = binaryChannelCode;
            EncodingMethod = encodingMethod;
        }

        public CommandSubscribeResponse(CtpDocumentReader reader)
            : base("SubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            BinaryChannelCode = (int)element.GetValue("BinaryChannelCode");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

            element.ErrorIfNotHandled();
        }


        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("BinaryChannelCode", BinaryChannelCode);
            writer.WriteValue("EncodingMethod", EncodingMethod);
        }
    }
}
