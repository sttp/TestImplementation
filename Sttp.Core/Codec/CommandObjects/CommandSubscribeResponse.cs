using System;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandSubscribeResponse : CommandBase
    {
        public readonly byte RawCommandCode;
        public readonly Guid EncodingMethod;

        public CommandSubscribeResponse(byte rawCommandCode, Guid encodingMethod)
            : base("SubscribeResponse")
        {
            RawCommandCode = rawCommandCode;
            EncodingMethod = encodingMethod;
        }

        public CommandSubscribeResponse(SttpMarkupReader reader)
            : base("SubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawCommandCode = (byte)element.GetValue("RawCommandCode");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RawCommandCode", RawCommandCode);
            writer.WriteValue("EncodingMethod", EncodingMethod);
        }
    }
}
