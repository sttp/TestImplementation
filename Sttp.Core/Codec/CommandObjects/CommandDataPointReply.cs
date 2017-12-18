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
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            RequestID = (Guid?)element.GetValue("RequestID");
            IsEndOfResponse = (bool)element.GetValue("IsEndOfResponse");
            EncodingMethod = (byte)element.GetValue("EncodingMethod");
            Data = (byte[])element.GetValue("Data");

            element.ErrorIfNotHandled();
        }

        public override CommandBase Load(SttpMarkupReader reader)
        {
            return new CommandGetMetadata(reader);
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("ID", RequestID);
                writer.WriteValue("IsEndOfResponse", IsEndOfResponse);
                writer.WriteValue("EncodingMethod", EncodingMethod);
                writer.WriteValue("Data", Data);
            }
        }
    }
}
