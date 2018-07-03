namespace CTP.Net
{
    public interface ICtpDataChannelHandler
    {
        void ProcessData(CtpSession session, byte[] data);
    }
}