using System;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandDataPointResponse : CommandBase
    {
        public readonly byte StreamingCode;
        public readonly Guid EncodingMethod;

        public CommandDataPointResponse(byte streamingCode, Guid encodingMethod)
            : base("DataPointResponse")
        {
            StreamingCode = streamingCode;
            EncodingMethod = encodingMethod;
        }

        public CommandDataPointResponse(SttpMarkupReader reader)
            : base("DataPointResponse")
        {
            var element = reader.ReadEntireElement();

            StreamingCode = (byte)element.GetValue("StreamingCode");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("StreamingCode", StreamingCode);
            writer.WriteValue("EncodingMethod", EncodingMethod);
        }
    }
}
