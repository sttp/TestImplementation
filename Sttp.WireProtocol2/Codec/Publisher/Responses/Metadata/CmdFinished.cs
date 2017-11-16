namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdFinished : ICmd
    {
        public SubCommand SubCommand => SubCommand.Finished;

        public void Load(PacketReader reader)
        {
        }

    }
}