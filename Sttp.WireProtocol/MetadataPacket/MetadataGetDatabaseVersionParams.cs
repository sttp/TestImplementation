namespace Sttp.WireProtocol.Data
{
    public class MetadataGetDatabaseVersionParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.GetDatabaseVersion;

    }
}