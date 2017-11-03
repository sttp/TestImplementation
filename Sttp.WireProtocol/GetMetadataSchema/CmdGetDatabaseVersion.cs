namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class CmdGetDatabaseVersion : ICmd
    {
        public SubCommand SubCommand => SubCommand.GetDatabaseVersion;

        public void Load(PacketReader reader)
        {
        }

    }
}