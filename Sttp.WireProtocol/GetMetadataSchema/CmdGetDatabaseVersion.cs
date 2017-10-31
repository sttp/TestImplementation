namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class CmdGetDatabaseVersion : ICmd
    {
        public SubCommand SubCommand => SubCommand.GetDatabaseVersion;

        public void Load(PacketReader reader)
        {
        }

        CmdGetDatabaseSchema ICmd.GetDatabaseSchema => null;
        CmdGetDatabaseVersion ICmd.GetDatabaseVersion => this;
    }
}