using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    public class BulkTransportBeginParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.BeginBulkTransport;
        public Guid Id;
        public BulkTransportMode Mode;
        public BulkTransportCompression Compression;
        public long OriginalSize;
        public byte[] Content;
    }
}