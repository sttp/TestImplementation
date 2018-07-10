using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using GSF.Diagnostics;

namespace CTP.Net
{
    public class CtpClient
    {
        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpClient), MessageClass.Component);

        private IPEndPoint m_remoteEndpoint;
        private X509CertificateCollection m_trustedCertificates;
        private string m_hostName;
        private CtpSession m_clientSession;
        private CertificateTrustMode m_certificateTrust = CertificateTrustMode.None;
        private TcpClient m_socket;
        private NetworkStream m_netStream;
        private SslStream m_sslStream = null;
        private Stream m_finalStream;
        private CtpStream m_ctpStream;
        private bool m_requireTrustedServers;
        private bool m_allowNativeTrust;
        private IAuthenticationService m_authenticationService;

        public CtpClient(bool useSSL)
        {
            UseSSL = useSSL;
            AllowNativeTrust = useSSL;
            RequireTrustedServers = useSSL;
        }

        public bool UseSSL { get; }

        /// <summary>
        /// Indicates if certificate trust is required. Defaults to True.
        /// The connection will only succeed if the server provided certificate matches a locally trusted certificate, 
        /// or if the OS trusts the certificate with the supplied host name.
        /// </summary>
        public bool RequireTrustedServers
        {
            get => m_requireTrustedServers;
            set
            {
                if (value && !UseSSL)
                    throw new Exception("Trusted servers can only exist with an SSL connection");
                m_requireTrustedServers = value;
            }
        }

        /// <summary>
        /// Indicates that trust can be established through the OS by having a trusted root CA 
        /// and a matching host name. Defaults to True.
        /// </summary>
        public bool AllowNativeTrust
        {
            get => m_allowNativeTrust;
            set
            {
                if (value && !UseSSL)
                    throw new Exception("Native trust can only exist with an SSL connection");
                m_allowNativeTrust = value;
            }
        }

        /// <summary>
        /// Specifies a collection of endpoint certificates that will be used to establish trust. 
        /// </summary>
        /// <param name="trustedCertificates"></param>
        public void SetTrustedCertificates(X509CertificateCollection trustedCertificates)
        {
            if (!UseSSL && trustedCertificates != null)
                throw new Exception("trusted certificates can only exist with an SSL connection");
            m_trustedCertificates = trustedCertificates;
        }

        public void SetHost(IPAddress address, int port, string hostName = null)
        {
            m_hostName = hostName;
            m_remoteEndpoint = new IPEndPoint(address, port);
        }

        public void SetHost(IPEndPoint host, string hostName = null)
        {
            m_hostName = hostName;
            m_remoteEndpoint = host;
        }

        public void SetHost(string hostName, int port)
        {
            m_hostName = hostName;
            IPAddress address = Dns.GetHostAddresses(hostName).First();
            m_remoteEndpoint = new IPEndPoint(address, port);
        }

        public void SetCredentials(NetworkCredential credentials)
        {
            m_authenticationService = new LocalAuthentication(credentials);
        }

        public CtpSession Connect()
        {
            m_certificateTrust = CertificateTrustMode.None;
            m_socket = new TcpClient();
            m_socket.SendTimeout = 3000;
            m_socket.ReceiveTimeout = 3000;
            m_socket.Connect(m_remoteEndpoint);
            m_netStream = m_socket.GetStream();

            string hostname = m_hostName ?? m_remoteEndpoint.Address.ToString();
            m_sslStream = null;
            if (UseSSL)
            {
                m_sslStream = new SslStream(m_netStream, false, ValidateCertificate, null, EncryptionPolicy.RequireEncryption);
                m_sslStream.AuthenticateAsClient(hostname, null, SslProtocols.Tls12, false);
                m_finalStream = m_sslStream;
            }
            else
            {
                m_finalStream = m_netStream;
            }

            m_ctpStream = new CtpStream();
            m_ctpStream.SetActiveStream(m_finalStream);

            m_authenticationService.Authenticate(m_ctpStream, m_sslStream);

            m_clientSession = new CtpSession(m_ctpStream, true, m_certificateTrust, m_socket, m_netStream, m_sslStream);
            return m_clientSession;
        }

        private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (m_trustedCertificates != null)
            {
                string certHash = certificate.GetCertHashString();
                string publicKey = certificate.GetPublicKeyString();
                foreach (var cert in m_trustedCertificates)
                {
                    if (cert.GetCertHashString() == certHash && cert.GetPublicKeyString() == publicKey)
                    {
                        m_certificateTrust = CertificateTrustMode.TrustedCertificate;
                        break;
                    }
                }
            }

            if (AllowNativeTrust && sslPolicyErrors == SslPolicyErrors.None)
            {
                m_certificateTrust |= CertificateTrustMode.Native;
            }

            return !RequireTrustedServers || m_certificateTrust != CertificateTrustMode.None;
        }

    }
}
