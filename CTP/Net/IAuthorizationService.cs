using System.Net.Security;

namespace CTP.Net
{
    public interface IAuthenticationService
    {
        void Authenticate(CtpStream stream, SslStream sslStream);
    }
}