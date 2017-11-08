namespace Sttp.WireProtocol.NegotiateSession
{
    public class CmdSupportedFunctionality : ICmd
    {
        public SubCommand SubCommand => SubCommand.SupportedFunctionality;
        public SttpNamedSet Options;

        public void Load(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
        }


    }
}