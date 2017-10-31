using System;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdAddTable : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddTable;
        public short TableIndex;
        public string TableName;
        public TableFlags TableFlags;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            TableName = reader.ReadString();
            TableFlags = reader.Read<TableFlags>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddRow ICmd.AddRow => null;
        CmdAddTable ICmd.AddTable => this;
        CmdAddValue ICmd.AddValue => null;
        CmdClear ICmd.Clear => null;
        CmdDeleteRow ICmd.DeleteRow => null;
    }
}