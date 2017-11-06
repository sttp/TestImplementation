using System;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdDatabaseVersion : ICmd
    {
        public SubCommand SubCommand => SubCommand.DatabaseVersion;
        public Guid SchemaVersion;
        public long Revision;

        public void Load(PacketReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
        }

    }
}