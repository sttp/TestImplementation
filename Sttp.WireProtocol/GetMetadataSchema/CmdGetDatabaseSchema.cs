namespace Sttp.WireProtocol.GetMetadataSchema
{
    public class CmdGetDatabaseSchema : ICmd
    {
        public SubCommand SubCommand => SubCommand.GetDatabaseSchema;

        public void Load(PacketReader reader)
        {
        }

    }
}