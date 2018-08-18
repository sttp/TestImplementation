using System.Net;
using System.Net.Security;
using System.Numerics;
using System.Text;

namespace CTP.Net
{
    public class LocalAuthentication : IAuthenticationService
    {
        private NetworkCredential m_credentials;
        //private ClientResumeTicket m_resumeCredentials;

        public LocalAuthentication(NetworkCredential credentials)
        {
            m_credentials = credentials;
        }

        public void Authenticate(CtpStream stream, SslStream sslStream)
        {
            //if (m_resumeCredentials == null)
            //{
            //    m_resumeCredentials = KeyExchange.AuthenticateAsClient(m_credentials, stream, sslStream);
            //}
            //else
            //{
            //    ResumeClientAuth.Authenticate(m_resumeCredentials, stream, sslStream);

            //}
        }

    }
}