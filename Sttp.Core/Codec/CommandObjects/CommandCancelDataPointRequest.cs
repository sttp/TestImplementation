using System;

namespace Sttp.Codec
{
    public class CommandCancelDataPointRequest : CommandBase
    {
        public readonly Guid RequestID;

        public CommandCancelDataPointRequest(Guid requestID)
            : base("CancelDataPointRequest")
        {
            RequestID = requestID;
        }

        public CommandCancelDataPointRequest(SttpMarkupReader reader)
            : base("CancelDataPointRequest")
        {
            var element = reader.ReadEntireElement();

            RequestID = (Guid)element.GetValue("RequestID");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RequestID", RequestID);
        }
    }
}
