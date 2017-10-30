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

        CmdDatabaseVersion ICmd.DatabaseVersion => this;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddRow ICmd.AddRow => null;
        CmdAddTable ICmd.AddTable => null;
        CmdAddValue ICmd.AddValue => null;
        CmdClear ICmd.Clear => null;
        CmdDeleteRow ICmd.DeleteRow => null;
    }
}