using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using CTP.Authentication;
using CTP.SRP;
using GSF.Diagnostics;

namespace CTP.Net
{
    public class CtpClient
    {
        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpClient), MessageClass.Component);

        private IPEndPoint m_remoteEndpoint;
        private CtpSession m_clientSession;
        private TcpClient m_socket;
        private NetworkStream m_netStream;
        private SslStream m_sslStream = null;
        private Stream m_finalStream;
        private CtpStream m_ctpStream;
        private ITicketSource m_ticket;
        private Auth m_auth;

        public CtpClient(IPEndPoint host, ITicketSource ticket = null)
        {
            m_remoteEndpoint = host;
            m_ticket = ticket;
        }

        public bool UseSSL => m_ticket != null;

        public CtpSession Connect()
        {
            m_auth = m_ticket?.GetTicket();

            m_socket = new TcpClient();
            m_socket.SendTimeout = 3000;
            m_socket.ReceiveTimeout = 3000;
            m_socket.Connect(m_remoteEndpoint);
            m_netStream = m_socket.GetStream();

            m_sslStream = null;
            if (UseSSL)
            {
                m_sslStream = new SslStream(m_netStream, false, ValidateCertificate, null, EncryptionPolicy.RequireEncryption);
                m_sslStream.AuthenticateAsClient(string.Empty, null, SslProtocols.Tls12, false);
                m_finalStream = m_sslStream;
            }
            else
            {
                m_finalStream = m_netStream;
            }

            m_ctpStream = new CtpStream(m_finalStream);
            m_clientSession = new CtpSession(m_ctpStream, true, m_socket, m_netStream, m_sslStream);
            if (m_auth == null)
            {
                m_clientSession.Send(new AuthNone());
            }
            else
            {
                m_clientSession.Send(m_auth);
            }
            return m_clientSession;
        }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            foreach (var remoteCerts in ((Ticket)m_auth.Ticket).ApprovedClientCertificates)
            {
                if (certificate.GetCertHashString() == remoteCerts)
                    return true;
            }
            return true;
        }

    }
}
