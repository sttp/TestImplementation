namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdAddRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddRow ICmd.AddRow => this;
        CmdAddTable ICmd.AddTable => null;
        CmdAddValue ICmd.AddValue => null;
        CmdClear ICmd.Clear => null;
        CmdDeleteRow ICmd.DeleteRow => null;

    }
}