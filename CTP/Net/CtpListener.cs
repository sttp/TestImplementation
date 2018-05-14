using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CTP.Net
{

    /// <summary>
    /// Listens on a specific endpoint to accept connections.
    /// </summary>
    public class CtpListener
    {
        private readonly ManualResetEvent m_shutdownEvent = new ManualResetEvent(false);
        private TcpListener m_listener;
        private bool m_shutdown;
        private AsyncCallback m_onAccept;
        private IPEndPoint m_listenEndpoint;

        public UserCredentialServices Permissions;
        public event SessionCompletedEventHandler SessionCompleted;

        /// <summary>
        /// Listen for a socket connection
        /// </summary>
        public CtpListener(IPEndPoint listenEndpoint)
        {
            m_listenEndpoint = listenEndpoint ?? throw new ArgumentNullException(nameof(listenEndpoint));
            m_onAccept = OnAccept;
            Permissions = new UserCredentialServices();
            Permissions.SessionCompleted += OnSessionCompleted;
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
                var session = new SessionToken(socket);
                Permissions.AuthenticateAsServer(session);
            }
            catch (Exception e)
            {
            }
        }

        protected virtual void OnSessionCompleted(SessionToken token)
        {
            SessionCompleted?.Invoke(token);
        }
    }
}