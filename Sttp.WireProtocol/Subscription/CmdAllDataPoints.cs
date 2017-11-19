namespace Sttp.WireProtocol.Subscription
{
    public class CmdAllDataPoints : ICmd
    {
        public SubCommand SubCommand => SubCommand.AllDataPoints;
        public SttpNamedSet Options;
        public SubscriptionAppendMode Mode;

        public void Load(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            Mode = reader.Read<SubscriptionAppendMode>();
        }
    }
}