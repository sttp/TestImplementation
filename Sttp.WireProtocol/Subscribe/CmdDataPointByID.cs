namespace Sttp.WireProtocol.Subscribe
{
    public class CmdDataPointByID : ICmd
    {
        public SubCommand SubCommand => SubCommand.DataPointByID;
        public SubscriptionAppendMode Mode;
        public SttpPointID[] Points;

        public void Load(PacketReader reader)
        {
            Mode = reader.Read<SubscriptionAppendMode>();
            Points = reader.ReadArray<SttpPointID>();
        }
        
    }
}