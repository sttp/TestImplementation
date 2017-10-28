using Sttp.WireProtocol.BulkTransportPacket;

namespace Sttp.WireProtocol
{
    public interface IBulkTransportParams
    {
        BulkTransportCommand Command { get; }
    }
}