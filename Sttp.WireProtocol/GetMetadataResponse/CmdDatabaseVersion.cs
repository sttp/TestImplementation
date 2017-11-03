using System;

namespace Sttp.WireProtocol.GetMetadataResponse
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
      
    }
}