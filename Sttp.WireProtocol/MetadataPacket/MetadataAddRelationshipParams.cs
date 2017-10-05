namespace Sttp.WireProtocol.Data
{
    public class MetadataAddRelationshipParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.AddRelationship;
        public int TableIndex;
        public int ColumnIndex;
        public int ForeignTableIndex;
    }
}