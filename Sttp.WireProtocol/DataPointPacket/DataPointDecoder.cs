namespace Sttp.WireProtocol.Codec.DataPointPacket
{
    public class DataPointDecoder : IPacketDecoder
    {
        public CommandCode CommandCode => CommandCode.DataPointPacket;

        public void Fill(StreamReader buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
