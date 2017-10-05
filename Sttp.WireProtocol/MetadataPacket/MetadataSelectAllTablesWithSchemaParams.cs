namespace Sttp.WireProtocol.Data
{
    public class MetadataSelectAllTablesWithSchemaParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.SelectAllTablesWithSchema;

    }
}