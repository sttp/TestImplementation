using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public delegate void PacketReceivedEventHandler(CtpSession sender, CtpCommand packet);

    public enum ReceiveMode
    {
        Blocking,
        Event,
    }

    public enum SendMode
    {
        Blocking,
        Queueing
    }

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

        /// <summary>
        /// For Blocking Mode: This event will not be raised.
        /// For Queuing Mode: This event will be raised 
        /// </summary>
        public event PacketReceivedEventHandler PacketReceived;

        private CtpStreamReader m_reader;
        private CtpStreamReaderAsync m_readerAsync;
        private CtpStreamWriter m_writer;
        private CtpStreamWriterAsync m_writerAsync;
        private int m_sendTimeout;
        private int m_receiveTimeout;
        private SendMode m_sendMode;
        private ReceiveMode m_receiveMode;
        private volatile bool m_disposed;

        public CtpSession(Stream stream, bool isServer, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {

            IsServer = isServer;
            m_socket = socket;
            m_netStream = netStream;
            m_ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;

            m_sendTimeout = 5000;
            m_receiveTimeout = 5000;
            m_sendMode = SendMode.Blocking;
            m_receiveMode = ReceiveMode.Blocking;
            m_writer = new CtpStreamWriter(m_stream);
            m_writer.OnException += OnException;
            m_reader = new CtpStreamReader(m_stream);
            m_reader.OnException += OnException;
        }

        public SendMode SendMode
        {
            get => m_sendMode;
            set
            {
                if (m_sendMode != value)
                {
                    switch (value)
                    {
                        case SendMode.Blocking:
                            throw new InvalidOperationException("Cannot change from a Queuing based reading scheme back into a blocking one.");
                        case SendMode.Queueing:
                            m_writerAsync = new CtpStreamWriterAsync(m_stream, m_sendTimeout);
                            m_writerAsync.OnException += OnException;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    m_sendMode = value;
                }
            }
        }



        public ReceiveMode ReceiveMode
        {
            get => m_receiveMode;
            set
            {
                if (m_receiveMode != value)
                {
                    switch (value)
                    {
                        case ReceiveMode.Blocking:
                            throw new InvalidOperationException("Cannot change from an event based reading scheme back into a blocking one.");
                        case ReceiveMode.Event:
                            m_readerAsync = new CtpStreamReaderAsync(m_stream);
                            m_readerAsync.OnException += OnException;
                            m_readerAsync.NewPacket += OnNewPacket;
                            m_readerAsync.Start();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    m_receiveMode = value;
                }
            }
        }

        /// <summary>
        /// Must be positive milliseconds
        /// If SendMode = Blocking: The timeout permitted during each Send operation
        /// If SendMode = Queuing: The depth of the queue permitted before the stream is deemed too far behind and is disposed.
        /// </summary>
        public int SendTimeout
        {
            get => m_sendTimeout;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "SendTimeout cannot be infinite");
                m_sendTimeout = value;
            }
        }

        /// <summary>
        /// May be 0.
        /// If ReceiveMode = Blocking: The maximum time to block for a read command.
        /// Otherwise Ignored.
        /// </summary>
        public int ReceiveTimeout
        {
            get => m_receiveTimeout;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "ReceiveTimeout cannot be negative");
                m_receiveTimeout = value;
            }
        }


        public void Start(ReceiveMode receiveMode, int receiveTimeout, SendMode sendMode, int sendTimeout)
        {
            ReceiveTimeout = receiveTimeout;
            SendTimeout = sendTimeout;
            ReceiveMode = receiveMode;
            SendMode = sendMode;
        }

        private void OnException(object arg1, Exception arg2)
        {
            Dispose();
        }

        private void OnNewPacket(CtpCommand obj)
        {
            if (m_disposed)
                return;
            PacketReceived?.Invoke(this, obj);
        }

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;
                m_ssl?.Dispose();
                m_netStream?.Dispose();
                m_socket?.Dispose();
                m_stream?.Dispose();
            }
        }

        /// <summary>
        /// Attempts a read operation.
        /// When ReceiveMode = Blocking,
        ///     this method will block until a read has occurred. If the stream has been disposed, an exception will be raised.
        /// When ReceiveMode = Queuing,
        ///     this method will not block, nor raise an exception if the stream has been disposed.
        /// </summary>
        /// <returns></returns>
        public CtpCommand Read()
        {
            switch (ReceiveMode)
            {
                case ReceiveMode.Blocking:
                    return m_reader.Read(m_receiveTimeout);
                case ReceiveMode.Event:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Send(CtpCommand packet)
        {
            switch (SendMode)
            {
                case SendMode.Blocking:
                    m_writer.Write(packet, m_sendTimeout);
                    break;
                case SendMode.Queueing:
                    m_writerAsync.Write(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
