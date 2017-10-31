namespace Sttp.WireProtocol.Codec.DataPointPacket
{
    public class DataPointDecoder
    {
        public CommandCode CommandCode => CommandCode.DataPointPacket;

        public void Fill(PacketReader buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
