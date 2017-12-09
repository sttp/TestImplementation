using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportBeginSend
    {
        public CommandCode CommandCode => CommandCode.BulkTransportBeginSend;

        public readonly Guid ID;
        public readonly BulkTransportMode Mode;
        public readonly BulkTransportCompression Compression;
        public readonly long OrigionalSize;
        public readonly byte[] Data;

        public CommandBulkTransportBeginSend(PayloadReader reader)
        {
            ID = reader.ReadGuid();
            Mode = (BulkTransportMode)reader.ReadByte();
            Compression = (BulkTransportCompression)reader.ReadByte();
            OrigionalSize = reader.ReadInt64();
            Data = reader.ReadBytes();
        }

    }
}