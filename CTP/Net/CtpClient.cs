using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CTP.Authentication.SRP;
using CTP.SRP;
using GSF.Diagnostics;

namespace CTP.Net
{
    public class SelfAuthProxy : IAuthProxy
    {
        private NetworkCredential m_credential;
        private string m_credentialName;
        public SelfAuthProxy(NetworkCredential credential)
        {
            m_credential = credential;
            m_credentialName = m_credential.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
        }

        public string GetCredentialName()
        {
            return m_credentialName;
        }

        public SrpClientProof Authenticate(byte[] serverProof, SrpAuthResponse authResponse, X509Certificate publicCertificate)
        {
            var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var strength = (SrpStrength)authResponse.SrpStrength;
            var publicB = authResponse.PublicB.ToUnsignedBigInteger();
            var param = SrpConstants.Lookup(strength);
            var publicA = BigInteger.ModPow(param.g, privateA, param.N);
            var x = authResponse.ComputeX(m_credentialName, m_credential.SecurePassword);
            var verifier = param.g.ModPow(x, param.N);
            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            var sessionKey = base1.ModPow(exp1, param.N);
            var privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, publicCertificate);
            var clientProof = new ClientProof(null, null, serverProof);
            return clientProof.Encrypt(publicA.ToUnsignedByteArray(), privateSessionKey);
        }
    }

    public interface IAuthProxy
    {
        string GetCredentialName();
        SrpClientProof Authenticate(byte[] serverProof, SrpAuthResponse authResponse, X509Certificate publicCertificate);
    }

    public enum AuthenticationMode
    {
        None,
        SRP,
        SessionPairing,
    }

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
        private AuthenticationMode m_authMode;
        private IAuthProxy m_authProxy;

        public CtpClient(bool useSSL)
        {
            m_authMode = AuthenticationMode.None;
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

        public void SetSrpProxy(IAuthProxy authorizationServer)
        {
            m_authMode = AuthenticationMode.SRP;
            m_authProxy = authorizationServer;
        }

        public void SetSrpCredentials(NetworkCredential credentials)
        {
            m_authMode = AuthenticationMode.SRP;
            m_authProxy = new SelfAuthProxy(credentials);
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
                case AuthenticationMode.None:
                    WriteDocument(new AuthNone());
                    break;
                case AuthenticationMode.SRP:
                    AuthSrp();
                    break;
                case AuthenticationMode.SessionPairing:
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

        private void AuthSrp()
        {
            var identity = m_authProxy.GetCredentialName();
            WriteDocument(new AuthSrp(identity));
            SrpAuthResponse authResponse = (SrpAuthResponse)ReadDocument();
            byte[] serverProof = RNG.CreateSalt(64);
            var proof = m_authProxy.Authenticate(serverProof, authResponse, m_sslStream?.RemoteCertificate);
            WriteDocument(proof);
            SrpServerProof cr = (SrpServerProof)ReadDocument();
            if (!serverProof.SequenceEqual(cr.ServerProof))
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
