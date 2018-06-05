using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using CTP.SRP;
using GSF.Diagnostics;
using GSF.Security.Cryptography.X509;

namespace CTP.Net
{
    /// <summary>
    /// Specifies how the connection will ensure data protection.
    /// 
    /// During session negotiate, the client and server will default to the highest negotiated encryption mode supported.
    ///
    /// Escalation Order:
    /// None,
    /// ServerOnlyCertificate
    /// MutualCertificate
    /// 
    /// </summary>
    [Flags]
    public enum EncryptionMode
    {
        /// <summary>
        /// Specifies that this connection will not be encrypted. 
        /// This should only be used when connecting to a loop-back interface or in a highly trusted environment. 
        /// The session type is not negotiable, so both endpoints must have this explicitly turned on in order to use.
        /// </summary>
        None = 0,
        /// <summary>
        /// A TLS connection with only a server supplied certificate. Man-in-the-middle protection
        /// can be established by trusting the certificate, or authenticating with a method that provides
        /// this protection.
        /// </summary>
        ServerCertificate = 1,
        /// <summary>
        /// A TLS connection with both a server and client supplied certificate. Man-in-the-middle protection
        /// can be established if either the server or the client trusts each ether's certificate, 
        /// or authenticating with a method that provides this protection.
        /// </summary>
        MutualCertificate = 2,
    }

    /// <summary>
    /// Identifies how the client will trust the server
    /// </summary>
    [Flags]
    public enum ServerTrustMode
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

    public class CtpSocket
    {
        private static readonly Lazy<X509Certificate2> EmphericalCertificate = new Lazy<X509Certificate2>(() => CertificateMaker.GenerateSelfSignedCertificate(CertificateSigningMode.RSA_2048_SHA2_256, Guid.NewGuid().ToString("N")), LazyThreadSafetyMode.ExecutionAndPublication);

        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpSocket), MessageClass.Component);

        private IPEndPoint m_remoteEndpoint;
        private X509Certificate m_clientCertificate;
        private X509CertificateCollection m_trustedCertificates;
        private string m_hostName;

        public CtpSocket()
        {
            RequireSSL = true;
            AllowNativeTrust = true;
        }

        public EncryptionMode EncryptionMode
        {
            get
            {
                var mode = EncryptionMode.None;
                if (m_clientCertificate != null)
                {
                    mode = EncryptionMode.MutualCertificate;
                }
                else if (RemoteTrustMode != ServerTrustMode.None)
                {
                    mode = EncryptionMode.ServerCertificate;
                }
                else if (RequireSSL)
                {
                    mode = EncryptionMode.ServerCertificate;
                }
                return mode;
            }
        }

        public ServerTrustMode RemoteTrustMode
        {
            get
            {
                ServerTrustMode mode = ServerTrustMode.None;
                if (AllowNativeTrust)
                {
                    mode |= ServerTrustMode.Native;
                }
                if (m_trustedCertificates != null && m_trustedCertificates.Count > 0)
                {
                    mode |= ServerTrustMode.TrustedCertificate;
                }
                return mode;
            }
        }

        /// <summary>
        /// Indicates if the client will require SSL authentication. True is the default mode. 
        /// This can only be disabled if the authentication mode is not certificate based and if
        /// both sides negotiate that a connection will be without SSL.
        /// </summary>
        public bool RequireSSL { get; set; }

        /// <summary>
        /// Indicates that trust can be established through the OS by having a trusted root CA 
        /// and a matching host name.
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
        public void AddTrustedCertificates(X509CertificateCollection trustedCertificates)
        {
            m_trustedCertificates = trustedCertificates;
        }

        public void SetHost(IPAddress address, int port)
        {
            m_remoteEndpoint = new IPEndPoint(address, port);
        }

        public void SetHost(IPEndPoint host)
        {
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
            ServerTrustMode server = ServerTrustMode.None;

            RemoteCertificateValidationCallback validateCertificate = (sender, certificate, chain, sslPolicyErrors) =>
                          {
                              if (AllowNativeTrust && sslPolicyErrors == SslPolicyErrors.None)
                              {
                                  server = ServerTrustMode.Native;
                                  return true;
                              }

                              if (m_trustedCertificates != null)
                              {
                                  string certHash = certificate.GetCertHashString();
                                  string publicKey = certificate.GetPublicKeyString();
                                  foreach (var cert in m_trustedCertificates)
                                  {
                                      if (cert.GetCertHashString() == certHash && cert.GetPublicKeyString() == publicKey)
                                      {
                                          server = ServerTrustMode.TrustedCertificate;
                                          return true;
                                      }
                                  }
                              }
                              return false;
                          };



            var m_client = new TcpClient();
            m_client.SendTimeout = 3000;
            m_client.ReceiveTimeout = 3000;
            m_client.Connect(m_remoteEndpoint);
            var m_networkStream = m_client.GetStream();
            var encMode = EncryptionMode;
            switch (encMode)
            {
                case EncryptionMode.None:
                    m_networkStream.WriteByte((byte)'N');
                    break;
                case EncryptionMode.ServerCertificate:
                    m_networkStream.WriteByte((byte)'S');
                    break;
                case EncryptionMode.MutualCertificate:
                    m_networkStream.WriteByte((byte)'M');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            m_networkStream.Flush();

            char serverMode = (char)m_networkStream.ReadByte();
            switch (serverMode)
            {
                case 'N':
                    if (encMode != EncryptionMode.None)
                        throw new InvalidOperationException("Server requested No Encryption but the client requires encryption");
                    encMode = EncryptionMode.None;
                    break;
                case 'S':
                    if (encMode == EncryptionMode.MutualCertificate)
                        throw new InvalidOperationException("Server disallowed the client to specify a certificate.");
                    encMode = EncryptionMode.ServerCertificate;
                    break;
                case 'M':
                    if (m_clientCertificate != null)
                    {
                        m_clientCertificate = EmphericalCertificate.Value;
                        Log.Publish(MessageLevel.Info, "Authentication", "The server requested mutual certificates, but none was provided. Generating a certificate.");
                    }
                    encMode = EncryptionMode.MutualCertificate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SslStream m_sslStream = null;
            if (encMode != EncryptionMode.None)
            {
                Log.Publish(MessageLevel.Debug, "Connect", $"Connecting to {m_remoteEndpoint.ToString()} using SSL, Client Certificate: {m_clientCertificate?.ToString() ?? "None"}");
                X509CertificateCollection collection = null;
                if (m_clientCertificate != null)
                {
                    collection = new X509CertificateCollection(new[] { m_clientCertificate });
                    m_sslStream = new SslStream(m_networkStream, false, validateCertificate, UserCertificateSelectionCallback, EncryptionPolicy.RequireEncryption);
                }
                else
                {
                    m_sslStream = new SslStream(m_networkStream, false, validateCertificate, null, EncryptionPolicy.RequireEncryption);
                }
                m_sslStream.AuthenticateAsClient(m_hostName ?? m_remoteEndpoint.Address.ToString(), collection, SslProtocols.Tls12, false);
            }
            return new CtpSession(encMode, server, m_client, m_networkStream, m_sslStream);
        }

        private X509Certificate UserCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return localCertificates[0];
        }


    }
}
