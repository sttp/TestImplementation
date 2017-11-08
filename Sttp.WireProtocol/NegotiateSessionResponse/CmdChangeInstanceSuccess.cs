using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class CmdChangeInstanceSuccess : ICmd
    {
        public SubCommand SubCommand => SubCommand.ChangeInstanceSuccess;

        public void Load(PacketReader reader)
        {
        }
    }
}