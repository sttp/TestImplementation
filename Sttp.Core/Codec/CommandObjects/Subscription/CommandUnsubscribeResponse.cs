using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

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

        public CommandUnsubscribeResponse(CtpDocumentReader reader)
            : base("UnsubscribeResponse")
        {
            var element = reader.ReadEntireElement();

            RawChannelID = (int)element.GetValue("RawChannelID");


            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);

        }
    }
}
