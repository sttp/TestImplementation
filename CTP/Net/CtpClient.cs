using System;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CTP.SRP;
using GSF.Diagnostics;

namespace CTP.Net
{
    public class ClientResumeTicket
    {
        public byte[] Ticket;
        public byte[] TicketHMAC;
        public byte[] ChallengeResponseKey;

        public ClientResumeTicket(byte[] ticket, byte[] ticketHmac, byte[] challengeResponseKey)
        {
            Ticket = ticket;
            TicketHMAC = ticketHmac;
            ChallengeResponseKey = challengeResponseKey;
        }
    }

    public class LocalAuthorization : IAuthorizationService
    {
        private NetworkCredential m_credentials;
        private ClientResumeTicket m_resumeCredentials;

        public LocalAuthorization(NetworkCredential credentials)
        {
            m_credentials = credentials;
        }

        public void Authenticate(CtpStream stream, SslStream sslStream)
        {
            if (m_resumeCredentials == null)
            {
                WriteDocument(stream, new Auth(m_credentials.UserName, false));
                AuthResponse authResponse = (AuthResponse)ReadDocument(stream);
                var credentialName = m_credentials.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
                var privateA = RNG.CreateSalt(32).ToUnsignedBigInteger();
                var strength = (SrpStrength)authResponse.BitStrength;
                var publicB = authResponse.PublicB.ToUnsignedBigInteger();
                var param = SrpConstants.Lookup(strength);
                var publicA = BigInteger.ModPow(param.g, privateA, param.N);
                var x = authResponse.ComputeX(credentialName, m_credentials.SecurePassword);
                var verifier = param.g.ModPow(x, param.N);
                var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
                var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
                var sessionKey = base1.ModPow(exp1, param.N);
                var privateSessionKey = SrpMethods.ComputeChallenge(sessionKey, sslStream?.RemoteCertificate);
                var proof = new AuthClientProof(publicA.ToByteArray(), CreateKey(privateSessionKey, "Client Proof"));
                WriteDocument(stream, proof);
                AuthServerProof cr = (AuthServerProof)ReadDocument(stream);

                byte[] serverProof = CreateKey(privateSessionKey, "Server Proof");
                if (!serverProof.SequenceEqual(cr.ServerProof))
                    throw new Exception("Failed server challenge");

                if ((cr.SessionTicket?.Length ?? 0) > 0)
                {
                    m_resumeCredentials = cr.CreateResumeTicket(m_credentials.UserName, CreateKey(privateSessionKey, "Ticket Signing"), CreateKey(privateSessionKey, "Challenge Response"));
                }
            }
            else
            {
                var auth = new AuthResume(m_resumeCredentials.Ticket, m_resumeCredentials.TicketHMAC);
                WriteDocument(stream, auth);
                var authResponse = (AuthResumeResponse)ReadDocument(stream);

                byte[] clientChallenge = RNG.CreateSalt(32);
                byte[] cproof;
                byte[] sproof;
                using (var hmac = new HMACSHA256(m_resumeCredentials.ChallengeResponseKey))
                {
                    cproof = hmac.ComputeHash(authResponse.ServerChallenge.Concat(clientChallenge));
                    sproof = hmac.ComputeHash(clientChallenge.Concat(authResponse.ServerChallenge));
                }
                var rcp = new AuthResumeClientProof(cproof, clientChallenge);
                WriteDocument(stream, rcp);
                var rsp = (AuthResumeServerProof)ReadDocument(stream);

                if (!rsp.ServerProof.SequenceEqual(sproof))
                {
                    throw new Exception("Authorization Failed");
                }
            }
        }

        private byte[] CreateKey(byte[] privateSessionKey, string keyName)
        {
            using (var hmac = new HMACSHA256(privateSessionKey))
            {
                byte[] name = Encoding.ASCII.GetBytes(keyName);
                return hmac.ComputeHash(name, 0, name.Length);
            }
        }

        private void WriteDocument(CtpStream stream, DocumentObject command)
        {
            stream.Send(0, command.ToDocument().ToArray());
        }

        private CtpDocument ReadDocument(CtpStream stream)
        {
            stream.Read(-1);
            return new CtpDocument(stream.Results.Payload);
        }
    }

    public interface IAuthorizationService
    {
        void Authenticate(CtpStream stream, SslStream sslStream);
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

        private IAuthorizationService m_authorizationService;

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

        public void SetSrpCredentials(NetworkCredential credentials)
        {
            m_authorizationService = new LocalAuthorization(credentials);
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

            m_authorizationService.Authenticate(m_ctpStream, m_sslStream);

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
