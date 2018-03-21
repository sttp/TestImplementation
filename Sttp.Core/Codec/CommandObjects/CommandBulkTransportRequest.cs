using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportRequest : CommandBase
    {
        public readonly Guid ID;
        public readonly long Offset;
        public readonly int Length;

        public CommandBulkTransportRequest(Guid id, long offset, int length)
            : base("BulkTransportRequest")
        {
            ID = id;
            Offset = offset;
            Length = length;
        }

        public CommandBulkTransportRequest(SttpMarkupReader reader)
            : base("BulkTransportRequest")
        {
            var element = reader.ReadEntireElement();

            ID = (Guid)element.GetValue("ID");
            Offset = (long)element.GetValue("Offset");
            Length = (int)element.GetValue("Length");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ID", ID);
            writer.WriteValue("Offset", Offset);
            writer.WriteValue("Length", Length);
        }

    }
}