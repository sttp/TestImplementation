namespace Sttp.WireProtocol.SendDataPoints
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.SendDataPoints;

        public void Fill(PacketReader buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
