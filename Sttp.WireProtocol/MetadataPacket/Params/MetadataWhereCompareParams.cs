namespace Sttp.WireProtocol.Data
{
    public class MetadataWhereCompareParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.WhereCompare;
        public short TableIndex;
        public short ColumnIndex;
        public MetadataCompareMethod CompareMethod;
        public byte[] Item;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            CompareMethod = reader.Read<MetadataCompareMethod>();
            Item = reader.ReadBytes();
        }

    }
}