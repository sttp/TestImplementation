using System;

namespace Sttp.WireProtocol.GetMetadata
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
        CmdJoin ICmd.Join => null;
        CmdSelect ICmd.Select => null;
        CmdWhereCompare ICmd.WhereCompare => null;
        CmdWhereInString ICmd.WhereInString => null;
        CmdWhereInValue ICmd.WhereInValue => null;
        CmdWhereOperator ICmd.WhereOperator => null;
    }
}