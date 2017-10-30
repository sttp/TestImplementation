using System;

namespace Sttp.WireProtocol.Data
{
    public class MetadataDatabaseVersionParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DatabaseVersion;
        public Guid MajorVersion;
        public long MinorVersion;

        public void Load(PacketReader reader)
        {
            MajorVersion = reader.ReadGuid();
            MinorVersion = reader.ReadInt64();
        }
    }
}