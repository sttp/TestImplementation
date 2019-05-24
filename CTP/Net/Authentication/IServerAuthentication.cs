namespace CTP.Net
{
    public interface IServerAuthentication
    {
        IServerHandshake StartHandshake();
    }
}