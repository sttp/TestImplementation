namespace Sttp.WireProtocol.Subscription
{
    public class CmdUnsubscribeFromAll : ICmd
    {
        public SubCommand SubCommand => SubCommand.UnsubscribeFromAll;

        public void Load(PacketReader reader)
        {
        }

    }
}