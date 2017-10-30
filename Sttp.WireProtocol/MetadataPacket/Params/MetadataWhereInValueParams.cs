namespace Sttp.WireProtocol.Data
{
    public class MetadataWhereInValueParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.WhereInValue;
        public short TableIndex;
        public short ColumnIndex;
        public byte[][] Items;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            Items = reader.ReadArray<byte[]>();
        }

    }
}