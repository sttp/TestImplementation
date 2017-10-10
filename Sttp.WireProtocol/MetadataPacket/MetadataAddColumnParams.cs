namespace Sttp.WireProtocol.Data
{
    public class MetadataAddColumnParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.AddColumn;
        public int TableIndex;
        public int ColumnIndex;
        public string ColumnName;
        public ValueType ColumnType;
    }
}