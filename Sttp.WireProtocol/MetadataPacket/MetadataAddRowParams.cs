namespace Sttp.WireProtocol.Data
{
    public class MetadataAddRowParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.AddRow;
        public int TableIndex;
        public int RowIndex;

    }
}