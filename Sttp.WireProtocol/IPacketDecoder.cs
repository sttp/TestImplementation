namespace Sttp.WireProtocol
{
    public interface IPacketDecoder
    {
        CommandCode CommandCode { get; }
        MetadataGetSchemaCommand CreateCommandMetadataGetSchemaCommand();
        MetadataGetSchemaResponseCommand CreateMetadataGetSchemaResponseCommand();
    }
}