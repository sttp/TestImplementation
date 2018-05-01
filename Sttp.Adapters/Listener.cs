using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GSF;
using GSF.Diagnostics;

namespace Sttp.Adapters
{
    public delegate void NewClientEstablished(TcpClient client, HashSet<Guid> sids);

    public class Listener
    {
        /// <summary>
        /// Event Occurs when a new client has connected to this listener.
        /// </summary>
        public event NewClientEstablished NewClient;

        private static readonly LogPublisher Log = Logger.CreatePublisher(typeof(Listener), MessageClass.Application);
        private readonly ManualResetEvent m_shutdownEvent = new ManualResetEvent(false);
        private ListenerConfig m_options;
        private TcpListener m_listener;
        private int m_pendingAccepts;
        private bool m_shutdown;
        private AsyncCallback m_onAccept;

        /// <summary>
        /// Listen for regular HTTP connections
        /// </summary>
        /// <exception cref="ArgumentNullException"><c>address</c> is null.</exception>
        /// <exception cref="ArgumentException">Port must be a positive number.</exception>
        public Listener(ListenerConfig options)
        {
            m_options = options ?? throw new ArgumentNullException(nameof(options));
            m_onAccept = OnAccept;
        }

        /// <summary>
        /// Start listen for new connections
        /// </summary>
        /// <exception cref="InvalidOperationException">Listener have already been started.</exception>
        public void Start()
        {
            if (m_listener != null)
            {
                Log.Publish(MessageLevel.Error, MessageFlags.BugReport, "Duplicate calls to Start without calls to Stop");
                throw new InvalidOperationException("Listener have already been started.");
            }

            Log.Publish(MessageLevel.Info, MessageFlags.None, "Listener started", "Using Port: " + m_options.ListenEndpoint.ToString());
            m_listener = new TcpListener(m_options.ListenEndpoint);
            if (m_options.ListenEndpoint.AddressFamily == AddressFamily.InterNetworkV6 && Environment.OSVersion.Version.Major > 5)
            {
                m_listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            }
            m_listener.Server.NoDelay = true;
            m_listener.Server.ReceiveBufferSize = 64 * 1024;
            m_listener.Server.SendBufferSize = 64 * 1024;
            m_listener.Server.SendTimeout = 3000;
            m_listener.Server.ReceiveTimeout = 3000;
            m_listener.Start(5);
            Interlocked.Increment(ref m_pendingAccepts);
            m_listener.BeginAcceptTcpClient(m_onAccept, null);
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public void Stop()
        {
            if (m_listener == null)
            {
                Log.Publish(MessageLevel.Error, MessageFlags.BugReport, "Duplicate calls to Stop without calls to Start");
                throw new InvalidOperationException("Listener have already been Stopped.");
            }
            m_shutdown = true;
            m_listener.Stop();
            m_shutdownEvent.WaitOne();
            m_listener = null;
            Log.Publish(MessageLevel.Info, MessageFlags.None, "Listener Stopped", "Using Port: " + m_options.ListenEndpoint.ToString());
        }


        /// <summary>
        /// Will try to accept connections one more time.
        /// </summary>
        /// <exception cref="Exception">If any exceptions is thrown.</exception>
        private void RetryBeginAccept()
        {
            try
            {
                m_listener.BeginAcceptTcpClient(m_onAccept, null);
            }
            catch (Exception ex)
            {
                Log.Publish(MessageLevel.Warning, MessageFlags.None, "Exception while attempting a restart", null, null, ex);
            }
        }

        private void OnAccept(IAsyncResult ar)
        {
            bool beginAcceptCalled = false;
            try
            {
                int count = Interlocked.Decrement(ref m_pendingAccepts);
                if (m_shutdown)
                {
                    if (count == 0)
                    {
                        m_shutdownEvent.Set();
                    }
                    return;
                }

                Interlocked.Increment(ref m_pendingAccepts);
                m_listener.BeginAcceptTcpClient(OnAccept, null);
                beginAcceptCalled = true;

                TcpClient socket = m_listener.EndAcceptTcpClient(ar);
                IPAddress ipAddress = ((IPEndPoint)socket.Client.RemoteEndPoint).Address;
                HashSet<Guid> sids = new HashSet<Guid>();
                if (m_options.HasAccess(ipAddress, sids))
                {
                    Log.Publish(MessageLevel.Info, MessageFlags.None, "The provided client connected to the server", ipAddress.ToString());
                    NewClient?.Invoke(socket, sids);
                    return;
                }
                else
                {
                    Log.Publish(MessageLevel.Info, MessageFlags.SecurityMessage, "The provided client could not be found", ipAddress.ToString());
                }
                socket.Close();
            }
            catch (Exception err)
            {
                Log.Publish(MessageLevel.Warning, MessageFlags.None, "Exception while calling OnAccept", null, null, err);

                if (!beginAcceptCalled)
                    RetryBeginAccept();
            }
        }

    }
}