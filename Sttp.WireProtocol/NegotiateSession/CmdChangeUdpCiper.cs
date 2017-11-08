namespace Sttp.WireProtocol.NegotiateSession
{
    public class CmdChangeUdpCiper : ICmd
    {
        public SubCommand SubCommand => SubCommand.ChangeUdpCiper;
        public byte[] Nonce;

        public void Load(PacketReader reader)
        {
            Nonce = reader.ReadBytes();
        }

    }
}