using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public delegate void PacketReceivedEventHandler(CtpSession sender, CtpPacket packet);



    public class CtpSession : IDisposable
    {
        public event Action OnDisposed;

        public readonly bool IsServer;
        /// <summary>
        /// Gets the socket that this session is on.
        /// </summary>
        private readonly TcpClient m_socket;
        /// <summary>
        /// Gets the NetworkStream this session writes to.
        /// </summary>
        private readonly NetworkStream m_netStream;
        /// <summary>
        /// The SSL used to authenticate the connection if available.
        /// </summary>
        private readonly SslStream m_ssl;

        private Stream m_stream;

        private CtpStream m_ctpStream;

        /// <summary>
        /// The login name assigned to this session. Typically this will only be tracked by the server.
        /// </summary>
        public string LoginName = string.Empty;
        /// <summary>
        /// The roles granted to this session. Typically this will only be tracked by the server.
        /// </summary>
        public HashSet<string> GrantedRoles = new HashSet<string>();
        public IPEndPoint RemoteEndpoint => m_socket.Client.RemoteEndPoint as IPEndPoint;
        public X509Certificate RemoteCertificate => m_ssl?.RemoteCertificate;
        public X509Certificate LocalCertificate => m_ssl?.LocalCertificate;

        public event PacketReceivedEventHandler PacketReceived;

        public CtpSession(CtpStream stream, bool isServer, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            m_ctpStream = stream;
            m_ctpStream.OnDisposed += StreamOnOnDisposed;
            m_ctpStream.PacketReceived += M_ctpStream_PacketReceived;

            IsServer = isServer;
            m_socket = socket;
            m_netStream = netStream;
            m_ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;
        }

        private void M_ctpStream_PacketReceived(CtpPacket packet)
        {
            PacketReceived?.Invoke(this, packet);
        }

        public void Start(ReceiveMode receiveMode, int receiveTimeout, SendMode sendMode, int sendTimeout)
        {
            m_ctpStream.Start(receiveMode, receiveTimeout, sendMode, sendTimeout);
        }

        private void StreamOnOnDisposed()
        {
            Dispose();
            OnDisposed?.Invoke();
        }

        public CtpPacket Read()
        {
            return m_ctpStream.Read();
        }

        public void SendRaw(byte[] data, byte channel)
        {
            m_ctpStream.SendRaw(data, channel);
        }

        public void Send(DocumentObject document, byte channel = 0)
        {
            m_ctpStream.Send(document, channel);
        }

        public void Dispose()
        {
            m_ctpStream?.Dispose();
            m_ssl?.Dispose();
            m_netStream?.Dispose();
            m_socket?.Dispose();
            m_stream?.Dispose();
        }
    }
}
