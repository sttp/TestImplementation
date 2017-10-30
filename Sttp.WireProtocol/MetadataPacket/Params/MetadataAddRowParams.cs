namespace Sttp.WireProtocol.Data
{
    public class MetadataAddRowParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.AddRow;
        public short TableIndex;
        public int RowIndex;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            RowIndex = reader.ReadInt32();
        }

    }
}