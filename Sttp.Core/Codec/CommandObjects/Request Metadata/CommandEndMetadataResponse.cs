using System;
using System.Collections.Generic;
using System.Linq;
using CTP;

namespace Sttp.Codec
{
    public class CommandEndMetadataResponse : DocumentCommandBase
    {
        public readonly int RawChannelID;
        public readonly int RowCount;

        public CommandEndMetadataResponse(int rawChannelID, int rowCount)
            : base("EndMetadataResponse")
        {
            RawChannelID = rawChannelID;
            RowCount = rowCount;
        }

        public CommandEndMetadataResponse(CtpDocumentReader reader)
            : base("EndMetadataResponse")
        {
            var element = reader.ReadEntireElement();
            RawChannelID = (int)element.GetValue("RawChannelID");
            RowCount = (int)element.GetValue("RowCount");
            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("RawChannelID", RawChannelID);
            writer.WriteValue("RowCount", RowCount);
        }
    }
}
