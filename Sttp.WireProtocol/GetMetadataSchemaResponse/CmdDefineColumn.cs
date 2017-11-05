namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdDefineColumn : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineColumn;
        public short TableIndex;
        public short ColumnIndex;
        public string ColumnName;
        public byte ColumnTypeCode;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ColumnName = reader.ReadString();
            ColumnTypeCode = reader.ReadByte();
        }

    }
}