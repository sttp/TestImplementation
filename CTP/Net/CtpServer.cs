using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GSF;
using GSF.Diagnostics;

namespace CTP.Net
{
    public delegate void SessionCompletedEventHandler(CtpNetStream netStream);

    /// <summary>
    /// Listens on a specific endpoint to accept connections and create <see cref="CtpNetStream"/>s.
    /// </summary>
    public partial class CtpServer : IDisposable
    {
        private readonly LogPublisher Log;
        private readonly ManualResetEvent m_shutdownCompleted = new ManualResetEvent(false);
        private TcpListener m_listener;
        private volatile bool m_shuttingDown;
        private AsyncCallback m_onAccept;
        private IPEndPoint m_listenEndpoint;
        private CtpRuntimeConfig m_config;

        /// <summary>
        /// Raised when a client successfully connects
        /// </summary>
        public event SessionCompletedEventHandler SessionCompleted;

        /// <summary>
        /// Listen for a socket connection
        /// </summary>
        public CtpServer(IPEndPoint listenEndpoint, CtpServerConfig config)
        {
            m_config = new CtpRuntimeConfig(config);
            m_listenEndpoint = listenEndpoint ?? throw new ArgumentNullException(nameof(listenEndpoint));
            m_onAccept = OnAccept;

            var logMessages = new LogStackMessages("Listen Port", listenEndpoint.ToString());
            logMessages = logMessages.Union("Use SSL", m_config.EncryptionOptions.EnableSSL.ToString());
            if (m_config.EncryptionOptions.EnableSSL)
                logMessages = logMessages.Union("Certificate", m_config.EncryptionOptions.ServerCertificate.ToString());
            using (Logger.AppendStackMessages(logMessages))
                Log = Logger.CreatePublisher(typeof(CtpServer), MessageClass.Framework);
        }

        /// <summary>
        /// Start listen for new connections
        /// </summary>
        /// <exception cref="InvalidOperationException">Listener have already been started.</exception>
        public void Start()
        {
            if (m_listener != null)
                throw new InvalidOperationException("Listener have already been started.");

            if (SessionCompleted == null)
                throw new Exception("SessionCompleted event must be handled before starting a CtpServer");

            Log.Publish(MessageLevel.Info, "Starting Listener");

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
            m_shutdownCompleted.Reset();
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
        /// Stop the listener. Will block until all pending Accept operations are completed.
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public void Stop()
        {
            if (m_listener == null)
                throw new InvalidOperationException("Listener have already been Stopped.");

            Log.Publish(MessageLevel.Info, "Listener Stopped");

            m_shuttingDown = true;
            m_listener.Stop();
            m_shutdownCompleted.WaitOne();
            m_listener = null;
        }

        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                TcpClient socket = m_listener.EndAcceptTcpClient(ar);
                socket.SendTimeout = 3000;
                socket.ReceiveTimeout = 3000;
                if (m_shuttingDown)
                {
                    m_shutdownCompleted.Set();
                    return;
                }

                ProcessClient.AcceptAsync(this, socket);
                m_listener.BeginAcceptTcpClient(m_onAccept, null);
            }
            catch (ObjectDisposedException)
            {
                if (m_shuttingDown)
                {
                    m_shutdownCompleted.Set();
                    return;
                }
            }
            catch (Exception er)
            {
                if (m_shuttingDown)
                {
                    m_shutdownCompleted.Set();
                    return;
                }
                Logger.SwallowException(er, "Exception while processing a client's accept, restarting the OnAccept");
                try
                {
                    m_listener.BeginAcceptTcpClient(m_onAccept, null);
                }
                catch (Exception e)
                {
                    m_shutdownCompleted.Set();
                    Logger.SwallowException(e, "Exception while attempting to restart the OnAccept");
                }
            }
        }

        protected virtual void OnSessionCompleted(CtpNetStream token)
        {
            SessionCompleted?.Invoke(token);
        }

        public void Dispose()
        {
            try
            {
                m_shutdownCompleted.Set();
                m_listener?.Stop();
            }
            catch (Exception e)
            {
                Logger.SwallowException(e, "Exception throw while disposing");
            }
            m_listener = null;
        }
    }
}