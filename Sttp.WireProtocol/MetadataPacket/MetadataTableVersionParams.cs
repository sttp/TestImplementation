using System;

namespace Sttp.WireProtocol.Data
{
    public class MetadataTableVersionParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.TableVersion;
        public int TableIndex;
        public Guid MajorVersion;
        public long MinorVersion;
    }
}