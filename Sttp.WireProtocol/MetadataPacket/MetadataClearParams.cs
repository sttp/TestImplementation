using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataClearParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.Clear;
    }
}