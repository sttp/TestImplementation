namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class NegotiateSessionDecoder : IPacketDecoder
    {
        public CommandCode CommandCode => CommandCode.NegotiateSession;

        public void Fill(PacketReader buffer)
        {
            throw new System.NotImplementedException();
        }

    }
}
