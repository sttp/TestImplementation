using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataClearParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.Clear;

        public void Load(PacketReader reader)
        {
        }

    }
}