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
    /// ServerCertificate, ClientOnlyCertificate (If both are specified, it promotes to Mutual)
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
        /// A TLS connection where the client executes the TLS handshake as the server. During initial negotiation, 
        /// it's most probably that this kind of connection will be promoted to a Mutual Certificate Connection.
        /// </summary>
        ClientCertificate = 2,
        /// <summary>
        /// A TLS connection with both a server and client supplied certificate. Man-in-the-middle protection
        /// can be established if either the server or the client trusts each ether's certificate, 
        /// or authenticating with a method that provides this protection.
        /// </summary>
        MutualCertificate = 3,
    }

    /// <summary>
    /// Identifies how the server will trust the client. This is in addition to any certificate trust that may already 
    /// exist.
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// No additional authentication information is provided.
        /// </summary>
        None,
        /// <summary>
        /// An existing trusted connection is used to establish another connection. 
        /// </summary>
        ResumeSessionTicket,
        /// <summary>
        /// SRP will be used to authenticate a client to a server and protect against MITM attacks.
        /// </summary>
        SRP,
        /// <summary>
        /// LDAP will be used to authenticate a client to a server. This should not be used unless communicating
        /// over a trusted connection since credentials are provided in the clear to the server.
        /// </summary>
        LDAP,
        /// <summary>
        /// An OAUTH ticket will be provided to the server. Like LDAP, this should only be done over a trusted 
        /// connection.
        /// </summary>
        OAUTH,
        /// <summary>
        /// KERBEROS/NTLM will be used to authenticate a client to a server. If KERBEROS is used, this 
        /// will also protect against MITM attacks. If NTLM, this may protect against attacks, but an attacker
        /// would be able to brute-force that password offline and successfully mount an attack later.
        /// </summary>
        NegotiateSession
    }

    /// <summary>
    /// Identifies how the client will trust the server
    /// </summary>
    [Flags]
    public enum ServerTrustMode
    {
        /// <summary>
        /// Trust will either not be established, or be dependent on SRP or KERBEROS.
        /// </summary>
        None = 0,
        /// <summary>
        /// The specific certificate that the server uses will be trusted.
        /// </summary>
        TrustedCertificate = 1,
        /// <summary>
        /// The certificate will be trusted if it's part of a trusted certificate authority
        /// and the Name of the certificate matches.
        /// </summary>
        TrustedNameAndCA = 2,
    }

    public class CtpClient
    {
        private static readonly Lazy<X509Certificate2> EmphericalCertificate = new Lazy<X509Certificate2>(() => CertificateMaker.GenerateSelfSignedCertificate(CertificateSigningMode.RSA_2048_SHA2_256, Guid.NewGuid().ToString("N")), LazyThreadSafetyMode.ExecutionAndPublication);
        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(CtpClient), MessageClass.Component);

        private IPEndPoint m_remoteEndpoint;

        public EncryptionMode EncryptionMode
        {
            get
            {
                var mode = EncryptionMode.None;
                if (ServerTrustMode != ServerTrustMode.None)
                {
                    mode = EncryptionMode.ServerCertificate;
                }

                if (m_userCertificate != null)
                {
                    mode |= EncryptionMode.ClientCertificate;
                }

                if (RequireSSL && mode != EncryptionMode.None)
                {
                    mode = EncryptionMode.ServerCertificate;
                }

                if (AuthenticationMode == AuthenticationMode.LDAP && mode != EncryptionMode.None)
                {
                    mode = EncryptionMode.ServerCertificate;
                }

                return mode;
            }
        }

        public AuthenticationMode AuthenticationMode { get; private set; }

        public ServerTrustMode ServerTrustMode { get; private set; }

        private X509Certificate m_userCertificate;
        private NetworkCredential m_credential;

        private byte[] m_resumeSessionTicket;

        private X509CertificateCollection m_trustedCertificates;
        private string[] m_names;
        private X509CertificateCollection m_trustedRootCertificates;

        private TcpClient m_client;
        private NetworkStream m_networkStream;
        private SslStream m_sslStream;
        private NegotiateStream m_negotiateStream;

        private string m_hostName;

        public CtpClient()
        {
            AuthenticationMode = AuthenticationMode.None;
            ServerTrustMode = ServerTrustMode.None;
        }

        /// <summary>
        /// Indicates if the client will require SSL authentication. True is the default mode. 
        /// This can only be disabled if the authentication mode is not certificate based and if
        /// both sides negotiate that a connection will be without SSL.
        /// </summary>
        public bool RequireSSL { get; set; } = true;

        public void SetResumeSessionTicket(byte[] resumeSessionTicket)
        {
            AuthenticationMode = AuthenticationMode.ResumeSessionTicket;
            m_resumeSessionTicket = resumeSessionTicket;
        }

        public void SetUserCredentials(X509Certificate certificate)
        {
            m_userCertificate = certificate;
        }

        public void SetLDAPCredentials(string domain, string username, string password)
        {
            m_credential = new NetworkCredential(username, password, domain);
            AuthenticationMode = AuthenticationMode.LDAP;
        }

        public void SetNegotiateCredentials()
        {
            m_credential = CredentialCache.DefaultNetworkCredentials;
            AuthenticationMode = AuthenticationMode.NegotiateSession;
        }

        public void SetNegotiateCredentials(NetworkCredential credentials)
        {
            m_credential = credentials;
            AuthenticationMode = AuthenticationMode.NegotiateSession;
        }

        public void SetNegotiateCredentials(string domain, string username, string password)
        {
            m_credential = new NetworkCredential(username, password, domain);
            AuthenticationMode = AuthenticationMode.NegotiateSession;
        }

        public void SetUserCredentials(string username, string password)
        {
            m_credential = new NetworkCredential(username, password);
            AuthenticationMode = AuthenticationMode.SRP;
        }

        public void SetUserCredentials(NetworkCredential credentials)
        {
            m_credential = credentials;
        }

        public void AddTrustedCertificates(X509CertificateCollection trustedCertificates)
        {
            m_trustedCertificates = trustedCertificates;
        }

        public void AddTrustedNames(string[] names, X509CertificateCollection trustedRootCertificates)
        {
            m_names = names;
            m_trustedRootCertificates = trustedRootCertificates;
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

        public Stream GetFinalStream => (Stream)m_sslStream ?? m_networkStream;

        public void Connect()
        {
            m_client = new TcpClient();
            m_client.SendTimeout = 3000;
            m_client.ReceiveTimeout = 3000;
            m_client.Connect(m_remoteEndpoint);
            m_networkStream = m_client.GetStream();
            var encMode = EncryptionMode;
            switch (encMode)
            {
                case EncryptionMode.None:
                    m_networkStream.WriteByte((byte)'N');
                    break;
                case EncryptionMode.ServerCertificate:
                    m_networkStream.WriteByte((byte)'S');
                    break;
                case EncryptionMode.ClientCertificate:
                    m_networkStream.WriteByte((byte)'C');
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
                    if ((encMode & EncryptionMode.ClientCertificate) == EncryptionMode.ClientCertificate)
                        throw new InvalidOperationException("Server disallowed the client to specify a certificate.");
                    encMode = EncryptionMode.ServerCertificate;
                    break;
                case 'C':
                    if ((encMode & EncryptionMode.ServerCertificate) == EncryptionMode.ServerCertificate)
                        throw new InvalidOperationException("Server did not specify a certificate.");
                    encMode = EncryptionMode.ClientCertificate;
                    break;
                case 'M':
                    if (m_userCertificate != null)
                    {
                        m_userCertificate = EmphericalCertificate.Value;
                        Log.Publish(MessageLevel.Info, "Authentication", "The server requested mutual certificates, but none was provided. Generating a certificate.");
                    }
                    encMode = EncryptionMode.MutualCertificate;
                    break;
            }

            if (encMode != EncryptionMode.None)
            {
                Log.Publish(MessageLevel.Debug, "Connect", $"Connecting to {m_remoteEndpoint.ToString()} using SSL, Client Certificate: {m_userCertificate?.ToString() ?? "None"}");
                X509CertificateCollection collection = null;
                if (m_userCertificate != null)
                {
                    collection = new X509CertificateCollection(new[] { m_userCertificate });
                    m_sslStream = new SslStream(m_networkStream, false, UserCertificateValidationCallback, UserCertificateSelectionCallback, EncryptionPolicy.RequireEncryption);
                }
                else
                {
                    m_sslStream = new SslStream(m_networkStream, false, UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
                }
                m_sslStream.AuthenticateAsClient(m_hostName ?? string.Empty, collection, SslProtocols.Tls12, false);
            }

            Stream stream = (Stream)m_sslStream ?? m_networkStream;

            switch (AuthenticationMode)
            {
                case AuthenticationMode.None:
                    stream.WriteByte((byte)AuthenticationProtocols.None);
                    stream.Flush();
                    break;
                case AuthenticationMode.ResumeSessionTicket:
                    throw new NotSupportedException();
                    break;
                case AuthenticationMode.SRP:
                    stream.WriteByte((byte)AuthenticationProtocols.SRP);
                    stream.Flush();
                    SrpAsClient();
                    break;
                case AuthenticationMode.LDAP:
                    stream.WriteByte((byte)AuthenticationProtocols.LDAP);
                    stream.Flush();
                    break;
                case AuthenticationMode.OAUTH:
                    throw new NotSupportedException();
                    break;
                case AuthenticationMode.NegotiateSession:
                    stream.WriteByte((byte)AuthenticationProtocols.NegotiateStream);
                    stream.Flush();
                    m_negotiateStream = new NegotiateStream(m_networkStream, true);
                    m_negotiateStream.AuthenticateAsClient(CredentialCache.DefaultNetworkCredentials, m_hostName ?? string.Empty, ProtectionLevel.EncryptAndSign, TokenImpersonationLevel.Identification);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private X509Certificate UserCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return localCertificates[0];
        }

        private void SrpAsClient()
        {
            Stream stream = (Stream)m_sslStream ?? m_networkStream;
            Srp6aClient.Authenticate(m_credential.UserName, m_credential.Password, stream, m_sslStream?.LocalCertificate, m_sslStream?.RemoteCertificate);
        }

        private bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
