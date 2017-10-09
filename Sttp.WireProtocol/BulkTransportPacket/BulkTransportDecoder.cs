using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class BulkTransportDecoder : IPacketDecoder
    {
        public CommandCode CommandCode => CommandCode.BulkTransport;

        
    }
}
