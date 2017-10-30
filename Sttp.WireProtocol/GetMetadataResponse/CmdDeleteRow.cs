namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDeleteRow : ICmd
    {
        public SubCommand SubCommand => SubCommand.DeleteRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddRow ICmd.AddRow => null;
        CmdAddTable ICmd.AddTable => null;
        CmdAddValue ICmd.AddValue => null;
        CmdClear ICmd.Clear => null;
        CmdDeleteRow ICmd.DeleteRow => this;

    }
}