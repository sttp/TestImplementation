using System;

namespace Sttp.WireProtocol.NegotiateSession
{
    public class CmdInitiateReverseConnection : ICmd
    {
        public SubCommand SubCommand => SubCommand.InitiateReverseConnection;

        public void Load(PacketReader reader)
        {
        }
      
    }
}