using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.NegotiateSession
{
    public class CmdChangeInstance : ICmd
    {
        public SubCommand SubCommand => SubCommand.ChangeInstance;
        public string InstanceName;

        public void Load(PacketReader reader)
        {
            InstanceName = reader.ReadString();
        }
    }
}