using System;
using System.Linq;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandSubscribeResponse : CommandBase
    {
        public readonly int RawChannelID;
        public readonly Guid EncodingMethod;

        public CommandSubscribeResponse(int rawChannelID, Guid encodingMethod)
            : base("SubscribeResponse")
        {
            RawChannelID = rawChannelID;
            EncodingMethod = encodingMethod;
        }

        public CommandSubscribeResponse(CtpMarkupReader reader)
            : base("SubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawChannelID = (int)element.GetValue("RawChannelID");
            EncodingMethod = (Guid)element.GetValue("EncodingMethod");

            element.ErrorIfNotHandled();
        }


        public override void Save(CtpMarkupWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);
            writer.WriteValue("EncodingMethod", EncodingMethod);
        }
    }
}
