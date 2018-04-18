using System;
using System.Linq;
using CTP;

namespace Sttp.Codec
{
    public class CommandSubscribeResponse : DocumentCommandBase
    {
        public readonly int RawChannelID;
        public readonly Guid EncodingMethod;

        public CommandSubscribeResponse(int rawChannelID, Guid encodingMethod)
            : base("SubscribeResponse")
        {
            RawChannelID = rawChannelID;
            EncodingMethod = encodingMethod;
        }

        public CommandSubscribeResponse(CtpDocumentReader reader)
            : base("SubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawChannelID = (int)element.GetValue("RawChannelID");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

            element.ErrorIfNotHandled();
        }


        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);
            writer.WriteValue("EncodingMethod", EncodingMethod);
        }
    }
}
