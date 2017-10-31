using System;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdDatabaseVersion : ICmd
    {
        public SubCommand SubCommand => SubCommand.DatabaseVersion;
        public Guid MajorVersion;
        public long MinorVersion;

        public void Load(PacketReader reader)
        {
            MajorVersion = reader.ReadGuid();
            MinorVersion = reader.ReadInt64();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => this;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddTable ICmd.AddTable => null;
    }
}