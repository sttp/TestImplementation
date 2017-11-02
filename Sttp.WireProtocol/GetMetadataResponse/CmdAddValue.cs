namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdAddValue : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddValue;
        public short TableIndex;
        public short ColumnIndex;
        public int RowIndex;
        public SttpValue Value;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
            Value = reader.Read<SttpValue>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddRow ICmd.AddRow => null;
        CmdAddTable ICmd.AddTable => null;
        CmdAddValue ICmd.AddValue => this;
        CmdClear ICmd.Clear => null;
        CmdDeleteRow ICmd.DeleteRow => null;
    }
}