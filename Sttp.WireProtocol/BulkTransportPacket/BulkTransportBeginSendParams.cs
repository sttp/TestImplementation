using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    public class BulkTransportBeginSendParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.BeginSend;
        public Guid Id;
        public BulkTransportMode Mode;
        public BulkTransportCompression Compression;
        public long OriginalSize;
        public byte[] Content;
    }
}