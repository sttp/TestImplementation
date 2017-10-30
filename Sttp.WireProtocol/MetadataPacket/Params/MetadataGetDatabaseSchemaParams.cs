namespace Sttp.WireProtocol.Data
{
    public class MetadataGetDatabaseSchemaParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.GetDatabaseSchema;

        public void Load(PacketReader reader)
        {
            
        }

    }
}