using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandBulkTransportReply : CommandBase
    {
        public readonly Guid ID;
        public readonly long Offset;
        public readonly byte[] Data;

        public CommandBulkTransportReply(Guid id, long offset, byte[] data)
            : base("BulkTransportReply")
        {
            ID = id;
            Offset = offset;
            Data = data;
        }

        public CommandBulkTransportReply(SttpMarkupReader reader)
            : base("BulkTransportReply")
        {
            var element = reader.ReadEntireElement();

            ID = (Guid)element.GetValue("ID");
            Offset = (long)element.GetValue("Offset");
            Data = (byte[])element.GetValue("Data");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ID", ID);
            writer.WriteValue("Offset", Offset);
            writer.WriteValue("Data", Data);
        }
    }
}