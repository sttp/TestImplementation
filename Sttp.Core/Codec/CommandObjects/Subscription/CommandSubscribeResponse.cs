using System;
using System.Linq;

namespace Sttp.Codec
{
    public class CommandSubscribeResponse : CommandBase
    {
        public readonly byte RawChannelID;
        public readonly Guid EncodingMethod;

        public CommandSubscribeResponse(byte rawChannelID, Guid encodingMethod)
            : base("SubscribeResponse")
        {
            RawChannelID = rawChannelID;
            EncodingMethod = encodingMethod;
        }

        public CommandSubscribeResponse(SttpMarkupReader reader)
            : base("SubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawChannelID = (byte)element.GetValue("RawChannelID");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

            element.ErrorIfNotHandled();
        }


        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);
            writer.WriteValue("EncodingMethod", EncodingMethod);
        }
    }
}
