using System;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandPollResponse : CommandBase
    {
        public readonly byte RawCommandCode;
        public readonly Guid EncodingMethod;

        public CommandPollResponse(byte rawCommandCode, Guid encodingMethod)
            : base("PollResponse")
        {
            RawCommandCode = rawCommandCode;
            EncodingMethod = encodingMethod;
        }

        public CommandPollResponse(SttpMarkupReader reader)
            : base("PollResponse")
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
