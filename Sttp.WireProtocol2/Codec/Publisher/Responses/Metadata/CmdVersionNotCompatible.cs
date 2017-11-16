using System;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdVersionNotCompatible : ICmd
    {
        public SubCommand SubCommand => SubCommand.VersionNotCompatible;

        public void Load(PacketReader reader)
        {
        }
    }
}