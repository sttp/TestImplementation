using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using GSF.Diagnostics;
using GSF.Security.Cryptography.X509;

namespace CTP.Net
{
    /// <summary>
    /// Identifies how the client will trust the server
    /// </summary>
    [Flags]
    public enum CertificateTrustMode
    {
        /// <summary>
        /// Trust is not required. This can still be ensured at a higher level.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates that trust will be native to how HTTPS works. Trusting the name of the remote entity, and a trusted root CA. 
        /// </summary>
        Native = 1,
        /// <summary>
        /// The specific certificate that the server uses will be trusted.
        /// </summary>
        TrustedCertificate = 2,
    }

    public class CtpClient
    {
        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpClient), MessageClass.Component);

        private IPEndPoint m_remoteEndpoint;
        private X509Certificate m_clientCertificate;
        private X509CertificateCollection m_trustedCertificates;
        private string m_hostName;

        public CtpClient()
        {
            RequireSSL = true;
            AllowNativeTrust = true;
            RequireTrustedServers = true;
        }

        /// <summary>
        /// Indicates if the client will require SSL authentication. Defaults to True.
        /// This can only be disabled if the authentication mode is not certificate based and if
        /// both sides negotiate that a connection will be without SSL.
        /// </summary>
        public bool RequireSSL { get; set; }

        /// <summary>
        /// Indicates if certificate trust is required. Defaults to True.
        /// The connection will only succeed if the server provided certificate matches a locally trusted certificate, 
        /// or if the OS trusts the certificate with the supplied host name.
        /// </summary>
        public bool RequireTrustedServers { get; set; }

        /// <summary>
        /// Indicates that trust can be established through the OS by having a trusted root CA 
        /// and a matching host name. Defaults to True.
        /// </summary>
        public bool AllowNativeTrust { get; set; }

        /// <summary>
        /// Specifies a client certificate that will be used to authenticate this connection. 
        /// If leaving this blank and a certificate is required, a self-signed certificate will
        /// be automatically generated.
        /// </summary>
        /// <param name="certificate"></param>
        public void SetClientCertificate(X509Certificate certificate)
        {
            m_clientCertificate = certificate;
        }

        /// <summary>
        /// Specifies a collection of endpoint certificates that will be used to establish trust. 
        /// </summary>
        /// <param name="trustedCertificates"></param>
        public void SetTrustedCertificates(X509CertificateCollection trustedCertificates)
        {
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

     

        public CtpSession Connect()
        {
            if (!RequireSSL && RequireTrustedServers)
            {
                throw new InvalidOperationException("If RequireTrustedServers is true, RequireSSL must also be true");
            }

            CertificateTrustMode certificateTrust = CertificateTrustMode.None;

            bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (m_trustedCertificates != null)
                {
                    string certHash = certificate.GetCertHashString();
                    string publicKey = certificate.GetPublicKeyString();
                    foreach (var cert in m_trustedCertificates)
                    {
                        if (cert.GetCertHashString() == certHash && cert.GetPublicKeyString() == publicKey)
                        {
                            certificateTrust = CertificateTrustMode.TrustedCertificate;
                            break;
                        }
                    }
                }

                if (AllowNativeTrust && sslPolicyErrors == SslPolicyErrors.None)
                {
                    certificateTrust |= CertificateTrustMode.Native;
                }

                return !RequireTrustedServers || certificateTrust != CertificateTrustMode.None;
            }

            var socket = new TcpClient();
            socket.SendTimeout = 3000;
            socket.ReceiveTimeout = 3000;
            socket.Connect(m_remoteEndpoint);
            var netStream = socket.GetStream();
            if (!RequireSSL)
            {
                netStream.WriteByte((byte)'N');
            }
            else
            {
                netStream.WriteByte((byte)'1');
            }
            netStream.Flush();

            char serverMode = (char)netStream.ReadByte();
            bool encrypt;
            switch (serverMode)
            {
                case 'N':
                    if (!RequireSSL)
                        throw new InvalidOperationException("Server requested No Encryption but the client requires encryption");
                    encrypt = false;
                    break;
                case '1':
                    encrypt = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            string hostname = m_hostName ?? m_remoteEndpoint.Address.ToString();
            SslStream sslStream = null;
            if (encrypt)
            {
                Log.Publish(MessageLevel.Debug, "Connect", $"Connecting to {m_remoteEndpoint.ToString()} using SSL, Client Certificate: {m_clientCertificate?.ToString() ?? "None"}");
                X509CertificateCollection collection = null;
                if (m_clientCertificate != null)
                {
                    collection = new X509CertificateCollection(new[] { m_clientCertificate });
                    sslStream = new SslStream(netStream, false, ValidateCertificate, UserCertificateSelectionCallback, EncryptionPolicy.RequireEncryption);
                }
                else
                {
                    sslStream = new SslStream(netStream, false, ValidateCertificate, null, EncryptionPolicy.RequireEncryption);
                }
                sslStream.AuthenticateAsClient(hostname, collection, SslProtocols.Tls12, false);
            }
            return new CtpSession(true, certificateTrust, socket, netStream, sslStream);
        }

        private X509Certificate UserCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return localCertificates[0];
        }


    }
}
