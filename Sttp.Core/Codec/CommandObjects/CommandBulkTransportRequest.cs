using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportRequest : CommandBase
    {
        public readonly Guid ID;
        public readonly long StartingPosition;
        public readonly long Length;

        public CommandBulkTransportRequest(Guid id, long startingPosition, long length)
            : base("BulkTransportRequest")
        {
            ID = id;
            StartingPosition = startingPosition;
            Length = length;
        }

        public CommandBulkTransportRequest(SttpMarkupReader reader)
            : base("BulkTransportRequest")
        {
            var element = reader.ReadEntireElement();

            ID = (Guid)element.GetValue("ID");
            StartingPosition = (long)element.GetValue("StartingPosition");
            Length = (long)element.GetValue("Length");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ID", ID);
            writer.WriteValue("StartingPosition", StartingPosition);
            writer.WriteValue("Length", Length);
        }

    }
}