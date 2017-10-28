using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    public class BulkTransportCancelParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.CancelBulkTransport;
        public Guid Id;
    }
}