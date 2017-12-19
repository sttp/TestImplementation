using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportCancelSend : CommandBase
    {
        public readonly Guid ID;

        public CommandBulkTransportCancelSend(Guid id)
            : base("BulkTransportCancelSend")
        {
            ID = id;
        }

        public CommandBulkTransportCancelSend(SttpMarkupReader reader)
            : base("BulkTransportCancelSend")
        {
            var element = reader.ReadEntireElement();

            ID = (Guid)element.GetValue("ID");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ID", ID);
        }


    }
}