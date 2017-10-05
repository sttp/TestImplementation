using System;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for encoding each command into bytes
    /// </summary>
    public class SubscriptionDecoder : IPacketDecoder
    {
        public CommandCode CommandCode => CommandCode.Subscribe;

        public void Fill(StreamReader buffer)
        {
            throw new System.NotImplementedException();
        }

        
    }
}
