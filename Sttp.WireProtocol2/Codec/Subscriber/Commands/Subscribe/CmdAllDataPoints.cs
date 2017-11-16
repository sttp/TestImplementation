namespace Sttp.WireProtocol.Subscribe
{
    public class CmdAllDataPoints : ICmd
    {
        public SubCommand SubCommand => SubCommand.AllDataPoints;
        public SubscriptionAppendMode Mode;

        public void Load(PacketReader reader)
        {
            Mode = reader.Read<SubscriptionAppendMode>();
        }
    }
}