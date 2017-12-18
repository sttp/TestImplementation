using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportBeginSend : CommandBase
    {
        public readonly Guid ID;
        public readonly long OrigionalSize;
        public readonly byte[] Data;

        public CommandBulkTransportBeginSend(SttpMarkupReader reader)
            : base("BulkTransportBeginSend")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            ID = (Guid)element.GetValue("ID");
            OrigionalSize = (long)element.GetValue("OrigionalSize");
            Data = (byte[])element.GetValue("Data");
            
            element.ErrorIfNotHandled();
        }

        public override CommandBase Load(SttpMarkupReader reader)
        {
            return new CommandGetMetadata(reader);
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("ID", ID);
                writer.WriteValue("OrigionalSize", OrigionalSize);
                writer.WriteValue("Data", Data);
            }
        }
    }
}