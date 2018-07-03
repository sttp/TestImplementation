using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using CTP.SRP;
using GSF.IO;
using GSF.Security.Cryptography.X509;

namespace CTP.Net
{
    public delegate void SessionCompletedEventHandler(CtpSession token);

    /// <summary>
    /// Listens on a specific endpoint to accept connections.
    /// </summary>
    public class CtpServer
    {
        private class ProcessClient
        {
            private readonly CtpServer m_server;
            private readonly TcpClient m_client;
            private Stream m_finalStream;
            private SslStream m_ssl;
            private CtpSession m_session;
            public ProcessClient(CtpServer server, TcpClient client)
            {
                m_server = server;
                m_client = client;

                var thread = new Thread(Process);
                thread.IsBackground = true;
                thread.Start(null);
            }

            private void Process(object tcpClient)
            {
                try
                {
                    TcpClient socket = tcpClient as TcpClient;
                    NetworkStream netStream = socket.GetStream();
                    bool requireSSL = m_server.DefaultRequireSSL;
                    bool hasAccess = m_server.DefaultAllowConnections;
                    X509Certificate serverCertificate = m_server.DefaultCertificate;

                    var ipBytes = (socket.Client.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes();
                    foreach (var item in m_server.m_encryptionOptions.Values)
                    {
                        if (item.IP.IsMatch(ipBytes))
                        {
                            requireSSL = item.RequireSSL;
                            hasAccess = item.HasAccess;
                            serverCertificate = item.ServerCertificate;
                            break;
                        }
                    }

                    if (!hasAccess)
                    {
                        throw new Exception("Client does not have access");
                    }

                    char mode = (char)netStream.ReadNextByte();
                    switch (mode)
                    {
                        case 'N':
                            if (requireSSL)
                            {
                                mode = '1';
                            }
                            else
                            {
                                mode = 'N';
                            }
                            break;
                        case '1':
                            requireSSL = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    netStream.WriteByte((byte)mode);
                    netStream.Flush();

                    CertificateTrustMode certificateTrust = CertificateTrustMode.None;

                    bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
                    {
                        if (sslpolicyerrors == SslPolicyErrors.None || sslpolicyerrors == SslPolicyErrors.RemoteCertificateNotAvailable)
                        {
                            certificateTrust = CertificateTrustMode.Native;
                        }
                        return true;
                    }

                    if (requireSSL)
                    {
                        serverCertificate = serverCertificate ?? EmphericalCertificate.Value;
                        m_ssl = new SslStream(netStream, false, UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
                        m_ssl.AuthenticateAsServer(serverCertificate, true, SslProtocols.Tls12, false);
                        m_finalStream = m_ssl;
                    }
                    else
                    {
                        m_finalStream = netStream;
                    }

                    m_session = new CtpSession(false, certificateTrust, socket, netStream, m_ssl);

                    var doc = ReadDocument();
                    switch (doc.RootElement)
                    {
                        case "AuthSrp":
                            AuthSrp((AuthSrp)doc);
                            break;
                        case "AuthNone":
                            break;
                        default:
                            throw new Exception();
                    }


                    m_server.OnSessionCompleted(m_session);
                }
                catch (Exception e)
                {

                }
            }

            private void AuthSrp(AuthSrp command)
            {
                var items = m_server.Authentication.FindSrpUser(command);

                int m_state = 0;
                SrpUserCredential<SrpUserMapping> m_user = items;
                byte[] privateSessionKey;
                SrpConstants param;
                BigInteger verifier;
                BigInteger privateB;
                BigInteger publicB;

                param = SrpConstants.Lookup(m_user.Verifier.SrpStrength);
                verifier = m_user.Verifier.Verification.ToUnsignedBigInteger();
                privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
                publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);

                WriteDocument(new SrpIdentityLookup(m_user.Verifier.SrpStrength, m_user.Verifier.Salt, publicB.ToUnsignedByteArray(), m_user.Verifier.IterationCount));

                var clientResponse = (SrpClientResponse)ReadDocument();

                var publicA = clientResponse.PublicA.ToUnsignedBigInteger();
                byte[] clientChallenge = clientResponse.ClientChallenge;

                var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);

                var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_ssl?.RemoteCertificate, m_ssl?.LocalCertificate);
                var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_ssl?.RemoteCertificate, m_ssl?.LocalCertificate);
                privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_ssl?.RemoteCertificate, m_ssl?.LocalCertificate);

                if (!challengeClient.SequenceEqual(clientChallenge))
                    throw new Exception("Failed client challenge");
                byte[] serverChallenge = challengeServer;


                m_session.LoginName = m_user.Token.LoginName;
                m_session.GrantedRoles.UnionWith(m_user.Token.Roles);
                WriteDocument(new SrpServerResponse(serverChallenge));
            }

            private void WriteDocument(DocumentObject command)
            {
                var document = command.ToDocument();
                byte[] data = new byte[document.Length + 2];
                if (document.Length > 60000)
                    throw new Exception();
                data[0] = (byte)(document.Length >> 8);
                data[1] = (byte)document.Length;
                document.CopyTo(data, 2);
                m_finalStream.Write(data, 0, data.Length);
                m_finalStream.Flush();
            }

            private CtpDocument ReadDocument()
            {
                byte[] buffer = new byte[2];
                m_finalStream.ReadAll(buffer, 0, 2);
                int length = (buffer[0] << 8) + buffer[1];
                buffer = new byte[length];
                m_finalStream.ReadAll(buffer, 0, buffer.Length);
                return new CtpDocument(buffer);
            }


        }


        private static readonly Lazy<X509Certificate2> EmphericalCertificate = new Lazy<X509Certificate2>(() => CertificateMaker.GenerateSelfSignedCertificate(CertificateSigningMode.RSA_2048_SHA2_256, Guid.NewGuid().ToString("N")), LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly ManualResetEvent m_shutdownEvent = new ManualResetEvent(false);
        private TcpListener m_listener;
        private bool m_shutdown;
        private AsyncCallback m_onAccept;
        private IPEndPoint m_listenEndpoint;
        public event SessionCompletedEventHandler SessionCompleted;

        public ServerAuthentication Authentication = new ServerAuthentication();

        private SortedList<IpMatchDefinition, EncryptionOptions> m_encryptionOptions = new SortedList<IpMatchDefinition, EncryptionOptions>();

        /// <summary>
        /// Listen for a socket connection
        /// </summary>
        public CtpServer(IPEndPoint listenEndpoint)
        {
            m_listenEndpoint = listenEndpoint ?? throw new ArgumentNullException(nameof(listenEndpoint));
            m_onAccept = OnAccept;
        }

        public X509Certificate2 DefaultCertificate { get; private set; } = null;

        public bool DefaultRequireSSL { get; private set; } = true;

        public bool DefaultAllowConnections { get; private set; } = true;

        public void SetDefaultOptions(bool allowConnections, bool requireSSL = true, X509Certificate2 defaultCertificate = null)
        {
            DefaultCertificate = defaultCertificate;
            DefaultRequireSSL = requireSSL;
            DefaultAllowConnections = allowConnections;
        }

        public void SetIPSpecificOptions(IPAddress remoteIP, int bitmask, bool allowConnections = true, bool requireSSL = true, X509Certificate localCertificate = null)
        {
            var mask = new IpMatchDefinition(remoteIP, bitmask);
            m_encryptionOptions[mask] = new EncryptionOptions(mask, allowConnections, requireSSL, localCertificate);
        }

        /// <summary>
        /// Start listen for new connections
        /// </summary>
        /// <exception cref="InvalidOperationException">Listener have already been started.</exception>
        public void Start()
        {
            if (m_listener != null)
                throw new InvalidOperationException("Listener have already been started.");

            m_listener = new TcpListener(m_listenEndpoint);
            if (m_listenEndpoint.AddressFamily == AddressFamily.InterNetworkV6 && Environment.OSVersion.Version.Major > 5)
            {
                m_listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            }
            m_listener.Server.NoDelay = true;
            m_listener.Server.ReceiveBufferSize = 64 * 1024;
            m_listener.Server.SendBufferSize = 64 * 1024;
            m_listener.Server.SendTimeout = 3000;
            m_listener.Server.ReceiveTimeout = 3000;
            m_listener.Start(5);
            m_shutdownEvent.Reset();
            try
            {
                m_listener.BeginAcceptTcpClient(m_onAccept, null);
            }
            catch (Exception)
            {
                m_listener = null;
                throw;
            }
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public void Stop()
        {
            if (m_listener == null)
                throw new InvalidOperationException("Listener have already been Stopped.");

            m_shutdown = true;
            m_listener.Stop();
            m_shutdownEvent.WaitOne();
            m_listener = null;
        }

        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                TcpClient socket = m_listener.EndAcceptTcpClient(ar);
                socket.SendTimeout = 3000;
                socket.ReceiveTimeout = 3000;
                if (m_shutdown)
                {
                    m_shutdownEvent.Set();
                    return;
                }

                new ProcessClient(this, socket);
                m_listener.BeginAcceptTcpClient(OnAccept, null);
            }
            catch (ObjectDisposedException)
            {
                if (m_shutdown)
                {
                    m_shutdownEvent.Set();
                    return;
                }
            }
            catch (Exception er)
            {
                if (m_shutdown)
                {
                    m_shutdownEvent.Set();
                    return;
                }
                try
                {
                    m_listener.BeginAcceptTcpClient(OnAccept, null);
                }
                catch (Exception e)
                {
                }
            }
        }




        protected virtual void OnSessionCompleted(CtpSession token)
        {
            SessionCompleted?.Invoke(token);
        }
    }
}