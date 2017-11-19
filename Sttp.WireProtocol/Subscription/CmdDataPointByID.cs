namespace Sttp.WireProtocol.Subscription
{
    public class CmdDataPointByID : ICmd
    {
        public SubCommand SubCommand => SubCommand.DataPointByID;
        public SttpNamedSet Options;
        public SubscriptionAppendMode Mode;
        public SttpDataPointID[] DataPoints;

        public void Load(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            Mode = reader.Read<SubscriptionAppendMode>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
        
    }
}