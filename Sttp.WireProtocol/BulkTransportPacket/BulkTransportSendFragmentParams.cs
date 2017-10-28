using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    public class BulkTransportSendFragmentParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.SendFragment;
        public Guid Id;
        public long BytesRemaining;
        public byte[] Content;
    }
}