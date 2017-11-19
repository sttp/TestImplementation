using System;

namespace Sttp.WireProtocol.Metadata
{
    public class CmdVersionNotCompatible : ICmd
    {
        public SubCommand SubCommand => SubCommand.VersionNotCompatible;

        public void Load(PacketReader reader)
        {
        }
    }
}