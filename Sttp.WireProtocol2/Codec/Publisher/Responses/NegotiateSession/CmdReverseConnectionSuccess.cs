using System;

namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class CmdReverseConnectionSuccess : ICmd
    {
        public SubCommand SubCommand => SubCommand.ReverseConnectionSuccess;

        public void Load(PacketReader reader)
        {
        }
      
    }
}