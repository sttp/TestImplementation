using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandDataPointReply : CommandBase
    {
        public readonly Guid? RequestID;
        public readonly bool IsEndOfResponse;
        public readonly byte EncodingMethod;
        public readonly byte[] Data;

        public CommandDataPointReply(SttpMarkupReader reader)
            : base("DataPointReply")
        {
            var element = reader.ReadEntireElement();

            RequestID = (Guid?)element.GetValue("RequestID");
            IsEndOfResponse = (bool)element.GetValue("IsEndOfResponse");
            EncodingMethod = (byte)element.GetValue("EncodingMethod");
            Data = (byte[])element.GetValue("Data");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RequestID", RequestID);
            writer.WriteValue("IsEndOfResponse", IsEndOfResponse);
            writer.WriteValue("EncodingMethod", EncodingMethod);
            writer.WriteValue("Data", Data);
        }
    }
}
