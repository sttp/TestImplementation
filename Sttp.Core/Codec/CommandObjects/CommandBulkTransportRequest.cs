using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportRequest : CommandBase
    {
        public readonly Guid ID;
        public readonly long StartingPosition;
        public readonly long Length;

        public CommandBulkTransportRequest(SttpMarkupReader reader)
            : base("BulkTransportRequest")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != CommandName)
                throw new Exception("Invalid command");

            ID = (Guid)element.GetValue("ID");
            StartingPosition = (long)element.GetValue("StartingPosition");
            Length = (long)element.GetValue("Length");

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
                writer.WriteValue("StartingPosition", StartingPosition);
                writer.WriteValue("Length", Length);
            }
        }

    }
}