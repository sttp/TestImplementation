using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using CTP.Codec;

namespace Sttp.Codec
{
    public class CommandEndMetadataResponse : CommandBase
    {
        public readonly int RawChannelID;
        public readonly int RowCount;

        public CommandEndMetadataResponse(int rawChannelID, int rowCount)
            : base("EndMetadataResponse")
        {
            RawChannelID = rawChannelID;
            RowCount = rowCount;
        }

        public CommandEndMetadataResponse(CtpMarkupReader reader)
            : base("EndMetadataResponse")
        {
            var element = reader.ReadEntireElement();
            RawChannelID = (int)element.GetValue("RawChannelID");
            RowCount = (int)element.GetValue("RowCount");
            element.ErrorIfNotHandled();
        }

        public override void Save(CtpMarkupWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);
            writer.WriteValue("RowCount", RowCount);
        }
    }
}
