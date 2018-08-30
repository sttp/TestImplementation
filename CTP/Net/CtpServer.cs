using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace CTP.Net
{
    public class ClientCerts
    {
        public X509Certificate2 Certificate;
        public CtpClientCert ClientCert;

        public ClientCerts(CtpClientCert clientCert, X509Certificate2 certificate)
        {
            ClientCert = clientCert;
            Certificate = certificate;
        }
    }

    public class CtpRuntimeConfig
    {
        public SortedList<IpMatchDefinition, EncryptionOptions> EncryptionOptions = new SortedList<IpMatchDefinition, EncryptionOptions>();

        public SortedList<IpMatchDefinition, string> AnonymousMappings = new SortedList<IpMatchDefinition, string>();

        public Dictionary<string, List<string>> Accounts = new Dictionary<string, List<string>>();

        public Dictionary<string, ClientCerts> CertificateClients = new Dictionary<string, ClientCerts>();

        public CtpRuntimeConfig(CtpServerConfig config)
        {
            config.Validate();
            foreach (var item in config.InstalledCertificates)
            {
                if (item.IsEnabled)
                {
                    X509Certificate2 cert = null;
                    if (item.EnableSSL)
                    {
                        if (!File.Exists(item.CertificatePath))
                            throw new Exception($"Missing certificate for {item.Name} at {item.CertificatePath}");
                        cert = new X509Certificate2(item.CertificatePath);
                    }

                    foreach (var ip in item.RemoteIPs)
                    {
                        var def = new IpMatchDefinition(IPAddress.Parse(ip.IpAddress), ip.MaskBits);
                        EncryptionOptions.Add(def, new EncryptionOptions(def, cert));
                    }
                }
            }

            foreach (var item in config.AnonymousMappings)
            {
                var def = new IpMatchDefinition(IPAddress.Parse(item.AccessList.IpAddress), item.AccessList.MaskBits);
                AnonymousMappings.Add(def, item.AccountName);
            }

            foreach (var item in config.Accounts)
            {
                Accounts.Add(item.Name, item.Roles);
            }

            foreach (var item in config.ClientCerts)
            {
                foreach (var path in item.CertificatePaths)
                {
                    if (File.Exists(path))
                    {
                        X509Certificate2 certificate = new X509Certificate2(path);
                        CertificateClients.Add(certificate.Thumbprint, new ClientCerts(item, certificate));
                    }
                }
            }
        }
    }

    public delegate void SessionCompletedEventHandler(CtpSession token);

    /// <summary>
    /// Listens on a specific endpoint to accept connections.
    /// </summary>
    public partial class CtpServer
    {
        private readonly ManualResetEvent m_shutdownEvent = new ManualResetEvent(false);
        private TcpListener m_listener;
        private bool m_shutdown;
        private AsyncCallback m_onAccept;
        private IPEndPoint m_listenEndpoint;
        private CtpRuntimeConfig m_config;
        public event SessionCompletedEventHandler SessionCompleted;

        /// <summary>
        /// Listen for a socket connection
        /// </summary>
        public CtpServer(IPEndPoint listenEndpoint, CtpServerConfig config)
        {
            m_config = new CtpRuntimeConfig(config);
            m_listenEndpoint = listenEndpoint ?? throw new ArgumentNullException(nameof(listenEndpoint));
            m_onAccept = OnAccept;
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