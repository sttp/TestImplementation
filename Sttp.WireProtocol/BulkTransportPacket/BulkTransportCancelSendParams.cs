using System;
using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    public class BulkTransportCancelSendParams : IBulkTransportParams
    {
        public BulkTransportCommand Command => BulkTransportCommand.CancelSend;
        public Guid Id;
    }
}