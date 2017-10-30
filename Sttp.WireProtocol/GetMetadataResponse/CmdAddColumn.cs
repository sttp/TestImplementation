namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdAddColumn : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddColumn;
        public short TableIndex;
        public short ColumnIndex;
        public string ColumnName;
        public ValueType ColumnType;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ColumnName = reader.ReadString();
            ColumnType = reader.Read<ValueType>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => this;
        CmdAddRow ICmd.AddRow => null;
        CmdAddTable ICmd.AddTable => null;
        CmdAddValue ICmd.AddValue => null;
        CmdClear ICmd.Clear => null;
        CmdDeleteRow ICmd.DeleteRow => null;
    }
}