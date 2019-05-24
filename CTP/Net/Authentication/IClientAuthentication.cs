namespace CTP.Net
{
    public interface IClientAuthentication
    {
        IClientHandshake StartHandshake();
        void AuthenticationFailed();
    }
}