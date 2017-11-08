namespace Sttp.WireProtocol.NegotiateSessionResponse
{
    public class CmdChangeUdpCiperResponse : ICmd
    {
        public SubCommand SubCommand => SubCommand.ChangeUdpCiperResponse;
        public byte[] Nonce;

        public void Load(PacketReader reader)
        {
            Nonce = reader.ReadBytes();
        }

    }
}