namespace Sttp.WireProtocol.Data
{
    public class MetadataSelectParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.Select;
        public short TableIndex;
        public short ColumnIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            ColumnIndex = reader.ReadInt16();
        }

    }
}