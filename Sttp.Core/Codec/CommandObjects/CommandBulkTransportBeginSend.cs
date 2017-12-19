using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportBeginSend : CommandBase
    {
        public readonly Guid ID;
        public readonly long OrigionalSize;
        public readonly byte[] Data;

        public CommandBulkTransportBeginSend(Guid id, long origionalSize, byte[] data)
            : base("BulkTransportBeginSend")
        {
            ID = id;
            OrigionalSize = origionalSize;
            Data = data;
        }

        public CommandBulkTransportBeginSend(SttpMarkupReader reader)
            : base("BulkTransportBeginSend")
        {
            var element = reader.ReadEntireElement();
            ID = (Guid)element.GetValue("ID");
            OrigionalSize = (long)element.GetValue("OrigionalSize");
            Data = (byte[])element.GetValue("Data");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("ID", ID);
            writer.WriteValue("OrigionalSize", OrigionalSize);
            writer.WriteValue("Data", Data);
        }
    }
}