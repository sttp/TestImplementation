namespace Sttp.WireProtocol.Data
{
    public class MetadataAddValueParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.AddValue;
        public int ColumnIndex;
        public int RowIndex;
        public byte[] Value;
    }
}