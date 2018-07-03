using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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

                var thread = new Thread(ProcessClient);
                thread.IsBackground = true;
                thread.Start(socket);
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

        private void ProcessClient(object tcpClient)
        {
            try
            {
                TcpClient socket = tcpClient as TcpClient;
                NetworkStream netStream = socket.GetStream();
                SslStream ssl = null;
                bool requireSSL = DefaultRequireSSL;
                bool hasAccess = DefaultAllowConnections;
                X509Certificate serverCertificate = DefaultCertificate;

                var ipBytes = (socket.Client.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes();
                foreach (var item in m_encryptionOptions.Values)
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
                    ssl = new SslStream(netStream, false, UserCertificateValidationCallback, null, EncryptionPolicy.RequireEncryption);
                    ssl.AuthenticateAsServer(serverCertificate, true, SslProtocols.Tls12, false);
                }

                var session = new CtpSession(false, certificateTrust, socket, netStream, ssl);

                OnSessionCompleted(session);
            }
            catch (Exception e)
            {

            }
        }

        protected virtual void OnSessionCompleted(CtpSession token)
        {
            SessionCompleted?.Invoke(token);
        }
    }
}