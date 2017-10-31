namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }

        void Load(PacketReader reader);

        CmdDatabaseVersion DatabaseVersion { get; }
        CmdAddColumn AddColumn { get; }
        CmdAddTable AddTable { get; }
    }
}