using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using GSF.Diagnostics;

namespace CTP.Net
{
    /// <summary>
    /// A client that can connect to a <see cref="CtpServer"/>. Call <see cref="Connect"/> to grab a <see cref="CtpNetStream"/>.
    /// <see cref="Connect"/> can be called multiple times to create new connections and either blocks, errors, or times out.
    /// </summary>
    public class CtpClient
    {
        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpClient), MessageClass.Component);

        private IPEndPoint m_remoteEndpoint;
        private ITicketSource m_ticket;

        /// <summary>
        /// Creates a means of connecting to a <see cref="CtpServer"/>.
        /// </summary>
        /// <param name="host">The host IP</param>
        /// <param name="ticket">The authentication ticket to use. Specify null to not use SSL.</param>
        public CtpClient(IPEndPoint host, ITicketSource ticket = null)
        {
            m_remoteEndpoint = host;
            m_ticket = ticket;
        }

        public bool UseSSL => m_ticket != null;
        private List<string> m_approvedCerts;

        public CtpNetStream Connect()
        {
            var auth = m_ticket?.GetTicket();

            var socket = new TcpClient();
            socket.SendTimeout = 3000;
            socket.ReceiveTimeout = 3000;
            socket.Connect(m_remoteEndpoint);
            var netStream = socket.GetStream();

            SslStream m_sslStream = null;
            if (UseSSL)
            {
                m_approvedCerts = auth?.ValidServerSidePublicKeys;
                m_sslStream = new SslStream(netStream, false, ValidateCertificate, null, EncryptionPolicy.RequireEncryption);
                m_sslStream.AuthenticateAsClient(string.Empty, new X509Certificate2Collection(auth.ClientCertificate), SslProtocols.None, false);
            }

            var session = new CtpNetStream(socket, netStream, m_sslStream);
            if (auth == null)
            {
                session.Send(new AuthNone());
            }
            else
            {
                session.Send(auth.Auth);
            }
            return session;
        }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            foreach (var remoteCerts in m_approvedCerts)
            {
                if (certificate.GetCertHashString() == remoteCerts)
                    return true;
            }
            return true;
        }

    }
}
