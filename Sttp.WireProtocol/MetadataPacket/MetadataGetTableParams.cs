namespace Sttp.WireProtocol.Data
{
    public class MetadataGetTableParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.GetTable;
        public int TableIndex;
        public int[] ColumnList;
        public string[] FilterExpression;
    }
}