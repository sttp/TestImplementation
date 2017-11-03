using System;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdClear : ICmd
    {
        public SubCommand SubCommand => SubCommand.Clear;

        public void Load(PacketReader reader)
        {
        }
      

    }
}