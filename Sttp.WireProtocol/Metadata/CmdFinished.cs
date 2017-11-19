namespace Sttp.WireProtocol.Metadata
{
    public class CmdFinished : ICmd
    {
        public SubCommand SubCommand => SubCommand.Finished;

        public void Load(PacketReader reader)
        {
        }

    }
}