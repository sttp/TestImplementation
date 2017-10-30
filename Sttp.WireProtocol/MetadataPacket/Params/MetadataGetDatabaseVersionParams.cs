namespace Sttp.WireProtocol.Data
{
    public class MetadataGetDatabaseVersionParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.GetDatabaseVersion;

        public void Load(PacketReader reader)
        {
       
        }

    }
}