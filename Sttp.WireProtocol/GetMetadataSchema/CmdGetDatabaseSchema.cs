namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class CmdGetDatabaseSchema : ICmd
    {
        public SubCommand SubCommand => SubCommand.GetDatabaseSchema;

        public void Load(PacketReader reader)
        {
        }

        CmdGetDatabaseSchema ICmd.GetDatabaseSchema => this;
        CmdGetDatabaseVersion ICmd.GetDatabaseVersion => null;
    }
}