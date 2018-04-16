using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandUnsubscribeResponse : CommandBase
    {
        public readonly int RawChannelID;

        public CommandUnsubscribeResponse(int rawChannelID)
            : base("UnsubscribeResponse")
        {
            RawChannelID = rawChannelID;
        }

        public CommandUnsubscribeResponse(CtpMarkupReader reader)
            : base("UnsubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawChannelID = (int)element.GetValue("RawChannelID");


            element.ErrorIfNotHandled();
        }

        public override void Save(CtpMarkupWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);

        }
    }
}
