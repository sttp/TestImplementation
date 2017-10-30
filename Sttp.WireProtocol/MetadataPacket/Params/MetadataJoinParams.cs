namespace Sttp.WireProtocol.Data
{
    public class MetadataJoinParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.Join;
        public short TableIndex;
        public short ColumnIndex;
        public short ForeignTableIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
            ForeignTableIndex = reader.ReadInt16();
        }
    }
}