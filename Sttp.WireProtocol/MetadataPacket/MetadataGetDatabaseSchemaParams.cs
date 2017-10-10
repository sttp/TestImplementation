namespace Sttp.WireProtocol.Data
{
    public class MetadataGetDatabaseSchemaParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.GetDatabaseSchema;

    }
}