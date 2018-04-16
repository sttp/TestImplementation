using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public CommandUnsubscribeResponse(SttpMarkupReader reader)
            : base("UnsubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawChannelID = (int)element.GetValue("RawChannelID");


            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);

        }
    }
}
