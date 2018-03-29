using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandUnsubscribeResponse : CommandBase
    {
        public readonly byte RawChannelID;

        public CommandUnsubscribeResponse(byte rawChannelID)
            : base("UnsubscribeResponse")
        {
            RawChannelID = rawChannelID;
        }

        public CommandUnsubscribeResponse(SttpMarkupReader reader)
            : base("UnsubscribeResponse")
        {
            var element = reader.ReadEntireElement();
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {

        }
    }
}
