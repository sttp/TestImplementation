using System;

namespace Sttp.WireProtocol.Data
{
    public class MetadataDatabaseVersionParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.DatabaseVersion;
        public Guid MajorVersion;
        public long MinorVersion;
    }
}