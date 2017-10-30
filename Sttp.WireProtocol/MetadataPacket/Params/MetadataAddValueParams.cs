namespace Sttp.WireProtocol.Data
{
    public class MetadataAddValueParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.AddValue;
        public short TableIndex;
        public short ColumnIndex;
        public int RowIndex;
        public byte[] Value;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
            Value = reader.ReadBytes();
        }
    }
}