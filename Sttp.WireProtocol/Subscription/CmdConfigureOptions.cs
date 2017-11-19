namespace Sttp.WireProtocol.Subscription
{
    public class CmdConfigureOptions : ICmd
    {
        public SubCommand SubCommand => SubCommand.ConfigureOptions;
        public SttpNamedSet Options;

        public void Load(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
        }

    }
}