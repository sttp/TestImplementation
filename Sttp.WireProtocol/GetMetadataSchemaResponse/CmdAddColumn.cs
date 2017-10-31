namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdAddColumn : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddColumn;
        public short TableIndex;
        public short ColumnIndex;
        public string ColumnName;
        public SttpValueTypeCode ColumnTypeCode;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ColumnName = reader.ReadString();
            ColumnTypeCode = reader.Read<SttpValueTypeCode>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => this;
        CmdAddTable ICmd.AddTable => null;
    }
}