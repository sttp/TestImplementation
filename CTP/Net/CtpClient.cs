using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Numerics;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CTP.SRP;
using GSF.Diagnostics;

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
        private X509Certificate m_certificateCredentials;
        private X509CertificateCollection m_trustedCertificates;
        private string m_hostName;
        private CtpSession m_clientSession;
        private CertificateTrustMode m_certificateTrust = CertificateTrustMode.None;
        private TcpClient m_socket;
        private NetworkStream m_netStream;
        private SslStream m_sslStream = null;
        private AuthenticationProtocols m_authMode = AuthenticationProtocols.None;
        private Stream m_finalStream;
        private NetworkCredential m_credentials;
        private CtpStream m_ctpStream;
        private bool m_requireTrustedServers;
        private bool m_allowNativeTrust;

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
        /// Specifies a client certificate that will be used to authenticate this connection. 
        /// If leaving this blank and a certificate is required, a self-signed certificate will
        /// be automatically generated.
        /// </summary>
        /// <param name="certificate"></param>
        public void SetCertificateCredentials(X509Certificate certificate)
        {
            m_authMode = AuthenticationProtocols.UserCertificate;
            m_certificateCredentials = certificate;
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

        public void SetSrpCredentials(NetworkCredential credentials)
        {
            m_authMode = AuthenticationProtocols.SRP;
            m_credentials = credentials;
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

            switch (m_authMode)
            {
                case AuthenticationProtocols.None:
                    WriteDocument(new AuthNone());
                    break;
                case AuthenticationProtocols.SRP:
                    AuthSrp();
                    break;
                case AuthenticationProtocols.UserCertificate:
                    AuthCertificate();
                    break;
                case AuthenticationProtocols.Negotiate:
                    AuthNegotiate();
                    break;
                case AuthenticationProtocols.LDAP:
                    AuthLDAP();
                    break;
                default:
                    throw new Exception();
            }

            m_clientSession = new CtpSession(m_ctpStream, true, m_certificateTrust, m_socket, m_netStream, m_sslStream);
            return m_clientSession;
        }

        private void WriteDocument(DocumentObject command)
        {
            m_ctpStream.Send(0, command.ToDocument().ToArray());
        }

        private CtpDocument ReadDocument()
        {
            m_ctpStream.Read(-1);
            return new CtpDocument(m_ctpStream.Results.Payload);
        }

        private void AuthLDAP()
        {
            throw new NotImplementedException();
        }

        private void AuthNegotiate()
        {
            throw new NotImplementedException();
        }

        private void AuthCertificate()
        {
            throw new NotImplementedException();
        }

        private void AuthSrp()
        {
            var identity = m_credentials.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            SecureString password = m_credentials.SecurePassword;

            WriteDocument(new AuthSrp(identity));
            SrpIdentityLookup lookup = (SrpIdentityLookup)ReadDocument();
            var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)lookup.SrpStrength;
            var publicB = lookup.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);

            var x = lookup.ComputePassword(identity, password);
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_sslStream?.LocalCertificate, m_sslStream?.RemoteCertificate);
            var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_sslStream?.LocalCertificate, m_sslStream?.RemoteCertificate);
            var privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_sslStream?.LocalCertificate, m_sslStream?.RemoteCertificate);
            byte[] clientChallenge = challengeClient;

            WriteDocument(new SrpClientResponse(publicA.ToUnsignedByteArray(), clientChallenge));
            SrpServerResponse cr = (SrpServerResponse)ReadDocument();
            if (!challengeServer.SequenceEqual(cr.ServerChallenge))
                throw new Exception("Failed server challenge");

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
