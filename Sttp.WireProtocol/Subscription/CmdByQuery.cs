namespace Sttp.WireProtocol.Subscription
{
    public class CmdByQuery : ICmd
    {
        public SubCommand SubCommand => SubCommand.ByQuery;
        public SttpNamedSet Options;
        public SubscriptionAppendMode Mode;
        public SttpQueryExpression Query;

        public void Load(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            Mode = reader.Read<SubscriptionAppendMode>();
            Query = reader.Read<SttpQueryExpression>();
        }
    }
}