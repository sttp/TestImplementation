namespace Sttp.WireProtocol.SendDataPoints
{
    public class DataPointDecoder
    {
        public CommandCode CommandCode => CommandCode.SendDataPoints;

        public void Fill(PacketReader buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
