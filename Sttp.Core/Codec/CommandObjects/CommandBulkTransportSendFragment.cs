using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandBulkTransportSendFragment : CommandBase
    {

        public readonly Guid ID;
        public readonly long BytesRemaining;
        public readonly byte[] Content;

        public CommandBulkTransportSendFragment(SttpMarkupReader reader)
            : base("BulkTransportSendFragment")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            ID = (Guid)element.GetValue("ID");
            BytesRemaining = (long)element.GetValue("BytesRemaining");
            Content = (byte[])element.GetValue("Content");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("ID", ID);
                writer.WriteValue("BytesRemaining", BytesRemaining);
                writer.WriteValue("Data", Content);
            }
        }
    }
}