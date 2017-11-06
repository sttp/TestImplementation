using System;

namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdDatabaseVersion : ICmd
    {
        public SubCommand SubCommand => SubCommand.DatabaseVersion;
        public Guid SchemaVersion;
        public long Revision;
        public bool IsUpdateQuery;

        public void Load(PacketReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            IsUpdateQuery = reader.ReadBoolean();
        }
    }
}