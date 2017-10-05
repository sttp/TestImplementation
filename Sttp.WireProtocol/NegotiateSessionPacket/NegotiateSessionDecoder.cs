namespace Sttp.WireProtocol
{
    /// <summary>
    /// Responsible for decoding each packet into commands.
    /// </summary>
    public class NegotiateSessionDecoder : IPacketDecoder
    {
        public CommandCode CommandCode => CommandCode.NegotiateSession;

        public void Fill(StreamReader buffer)
        {
            throw new System.NotImplementedException();
        }

    }
}
