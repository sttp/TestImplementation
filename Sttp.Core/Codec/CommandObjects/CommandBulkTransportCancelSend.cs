using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportCancelSend : CommandBase
    {
        public readonly Guid ID;

        public CommandBulkTransportCancelSend(SttpMarkupReader reader)
            : base("BulkTransportCancelSend")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            ID = (Guid)element.GetValue("ID");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("ID", ID);
            }
        }


    }
}