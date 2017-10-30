namespace Sttp.WireProtocol.Data
{
    public interface IMetadataParams
    {
        MetadataSubCommand SubCommand { get; }
        void Load(PacketReader reader);
    }
}