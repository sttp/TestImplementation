using System;

namespace Sttp.WireProtocol.NegotiateSession
{
    public class CmdGetAllInstances : ICmd
    {
        public SubCommand SubCommand => SubCommand.GetAllInstances;

        public void Load(PacketReader reader)
        {
        }

    }
}