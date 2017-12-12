using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportRequest
    {
        public CommandCode CommandCode => CommandCode.BulkTransportRequest;

        public readonly Guid ID;
        public readonly long StartingPosition;
        public readonly long Length;

        public CommandBulkTransportRequest(PayloadReader reader)
        {
            ID = reader.ReadGuid();
            StartingPosition = reader.ReadInt64();
            Length = reader.ReadInt64();
        }

    }
}