namespace Sttp.WireProtocol.Data
{
    public class MetadataDeleteRowParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DeleteRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

    }
}