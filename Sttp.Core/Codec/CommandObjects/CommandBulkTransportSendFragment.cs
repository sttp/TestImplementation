using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandBulkTransportSendFragment : CommandBase
    {

        public readonly Guid ID;
        public readonly long BytesRemaining;
        public readonly byte[] Content;

        public CommandBulkTransportSendFragment(Guid id, long bytesRemaining, byte[] content)
            : base("BulkTransportSendFragment")
        {
            ID = id;
            BytesRemaining = bytesRemaining;
            Content = content;
        }

        public CommandBulkTransportSendFragment(SttpMarkupReader reader)
            : base("BulkTransportSendFragment")
        {
            var element = reader.ReadEntireElement();

            ID = (Guid)element.GetValue("ID");
            BytesRemaining = (long)element.GetValue("BytesRemaining");
            Content = (byte[])element.GetValue("Content");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ID", ID);
            writer.WriteValue("BytesRemaining", BytesRemaining);
            writer.WriteValue("Data", Content);
        }
    }
}