namespace Sttp.WireProtocol.Subscribe
{
    public class CmdByQuery : ICmd
    {
        public SubCommand SubCommand => SubCommand.ByQuery;
        public SubscriptionAppendMode Mode;
        public SttpQueryExpression Query;

        public void Load(PacketReader reader)
        {
            Mode = reader.Read<SubscriptionAppendMode>();
            Query = reader.Read<SttpQueryExpression>();
        }
    }
}