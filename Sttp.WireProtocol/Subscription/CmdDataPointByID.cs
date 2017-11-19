namespace Sttp.WireProtocol.Subscription
{
    public class CmdDataPointByID : ICmd
    {
        public SubCommand SubCommand => SubCommand.DataPointByID;
        public SubscriptionAppendMode Mode;
        public SttpDataPointID[] DataPoints;

        public void Load(PacketReader reader)
        {
            Mode = reader.Read<SubscriptionAppendMode>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
        
    }
}