namespace Sttp.WireProtocol.Data
{
    public class MetadataUseTableParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.UseTable;
        public int TableIndex;
    }
}