namespace Sttp.WireProtocol.Data
{
    public class MetadataAddColumnParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.AddColumn;
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
    }
}