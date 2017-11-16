namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class CmdDesiredOperation : ICmd
    {
        public SubCommand SubCommand => SubCommand.DesiredOperation;
        public SttpNamedSet Options;

        public void Load(PacketReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
        }


    }
}