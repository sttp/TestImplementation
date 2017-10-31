namespace Sttp.WireProtocol.GetMetadataSchema
{
    public interface ICmd
    {
        SubCommand SubCommand { get; }

        void Load(PacketReader reader);

        CmdGetDatabaseSchema GetDatabaseSchema { get; }
        CmdGetDatabaseVersion GetDatabaseVersion { get; }
    }
}