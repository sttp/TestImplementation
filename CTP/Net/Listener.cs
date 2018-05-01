using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CTP.Net
{
    public delegate void NewClientEstablished(TcpClient client, ConnectionOptions options);

    public class Listener
    {
        /// <summary>
        /// Event Occurs when a new client has connected to this listener.
        /// </summary>
        public event NewClientEstablished NewClient;
        private readonly ManualResetEvent m_shutdownEvent = new ManualResetEvent(false);
        private TcpListener m_listener;
        private int m_pendingAccepts;
        private bool m_shutdown;
        private AsyncCallback m_onAccept;
        private IPEndPoint m_listenEndpoint;
        private object m_syncRoot;
        private List<IpMatchDefinition> m_list;

        /// <summary>
        /// Listen for a socket connection
        /// </summary>
        public Listener(IPEndPoint listenEndpoint)
        {
            m_listenEndpoint = listenEndpoint;
            m_syncRoot = new object();
            m_list = new List<IpMatchDefinition>();
            m_onAccept = OnAccept;
        }

        /// <summary>
        /// Assigns behavior based on an access list.
        /// </summary>
        /// <param name="ip">the IP address to match</param>
        /// <param name="maskBits">The bits of the subnet mask</param>
        /// <param name="options">Options associated with the connection</param>
        public void AssignOptions(IPAddress ip, int maskBits, ConnectionOptions options = null)
        {
            var client = new IpMatchDefinition(ip, maskBits, options);
            lock (m_syncRoot)
            {
                if (!m_list.Contains(client))
                {
                    m_list.Add(client);
                    m_list.Sort();
                }
            }
        }

        private bool HasAccess(IPAddress ipAddress, out ConnectionOptions options)
        {
            options = null;
            var src = ipAddress.GetAddressBytes();
            lock (m_syncRoot)
            {
                if (m_list.Count == 0)
                    return true;

                foreach (var remote in m_list)
                {
                    if (remote.IsMatch(src))
                    {
                        options = remote.Options;
                        return true;
                    }
                }
                return false;
            }
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
            Interlocked.Increment(ref m_pendingAccepts);
            try
            {
                m_listener.BeginAcceptTcpClient(m_onAccept, null);
            }
            catch (Exception)
            {
                Interlocked.Decrement(ref m_pendingAccepts);
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
            catch (Exception)
            {
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
                ConnectionOptions options;
                if (HasAccess(ipAddress, out options))
                {
                    NewClient?.Invoke(socket, options);
                    return;
                }
                socket.Close();
            }
            catch (Exception er)
            {
                if (!beginAcceptCalled)
                    RetryBeginAccept();
            }
        }

    }
}