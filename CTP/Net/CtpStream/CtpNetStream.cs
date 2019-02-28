using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using CTP.IO;
using GSF.Diagnostics;

namespace CTP.Net
{
    public delegate void PacketReceivedEventHandler(CtpNetStream sender, CtpCommand command);

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

    public class CtpNetStream : IDisposable
    {
        public static LogPublisher Log = Logger.CreatePublisher(typeof(CtpNetStream), MessageClass.Component);

        /// <summary>
        /// Occurs when this class is disposed. This event is raised on a ThreadPool thread and will only be called once.
        /// </summary>
        public event Action OnDisposed;
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
        private int m_disposed = 0;
        private CtpWriteParser m_write;

        public CtpNetStream(TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            m_socket = socket;
            m_netStream = netStream;
            m_ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;

            m_sendTimeout = 5000;
            m_receiveTimeout = 5000;
            m_sendMode = SendMode.Blocking;
            m_receiveMode = ReceiveMode.Blocking;
            m_writer = new CtpStreamWriter(m_stream, SendReceiveOnException);
            m_reader = new CtpStreamReader(m_stream, SendReceiveOnException);
            m_write = new CtpWriteParser(CtpCompressionMode.None, InternalSend);
        }

        /// <summary>
        /// Changes the SendMode on the session. The session starts out in blocking mode, but can be changed to a queuing mode.
        /// However, it cannot be changed back into a blocking mode at the present time.
        /// </summary>
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
                            m_writerAsync = new CtpStreamWriterAsync(m_stream, m_sendTimeout, SendReceiveOnException);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    m_sendMode = value;
                }
            }
        }

        /// <summary>
        /// Changes the ReceiveMode of the session. It starts out in a blocking mode, and can be changed into an event mode.
        /// However, it cannot be changed back into a blocking mode at the present time.
        /// </summary>
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
                            m_readerAsync = new CtpStreamReaderAsync(m_stream, SendReceiveOnException);
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
                if (m_writerAsync != null)
                    m_writerAsync.Timeout = value;
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

        /// <summary>
        /// A helper method to specifies all of the modes and timeouts in one swoop. This method is not required before reading/writing from a socket.
        /// </summary>
        /// <param name="receiveMode"></param>
        /// <param name="receiveTimeout"></param>
        /// <param name="sendMode"></param>
        /// <param name="sendTimeout"></param>
        public void Start(ReceiveMode receiveMode, int receiveTimeout, SendMode sendMode, int sendTimeout)
        {
            ReceiveTimeout = receiveTimeout;
            SendTimeout = sendTimeout;
            ReceiveMode = receiveMode;
            SendMode = sendMode;
        }

        /// <summary>
        /// Event occurs when a send/receive operation has an exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        private void SendReceiveOnException(object sender, Exception ex)
        {
            Log.Publish(MessageLevel.Error, MessageFlags.None, "Send/Receive Error", "An error has occurred with sending or receiving from a socket. The socket will be disposed.", null, ex);
            if (Interlocked.CompareExchange(ref m_disposed, 1, 0) == 0)
            {
                ThreadPool.QueueUserWorkItem(InternalDispose);
            }
        }

        private void OnNewPacket(CtpCommand obj)
        {
            if (m_disposed == 1)
                return;
            PacketReceived?.Invoke(this, obj);
        }

        private void OnDisposedCallback(object state)
        {
            OnDisposed?.Invoke();
        }

        private void InternalDispose(object state)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(OnDisposedCallback);
                m_ssl?.Dispose();
                m_netStream?.Dispose();
                m_socket?.Dispose();
                m_stream?.Dispose();

                m_reader?.Dispose();
                m_readerAsync?.Dispose();
                m_writer?.Dispose();
                m_writerAsync?.Dispose();
            }
            catch (Exception e)
            {

            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref m_disposed, 1, 0) == 0)
            {
                InternalDispose(null);
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

        //public void Send(CtpCommand command)
        //{
        //    m_write.Send(command);
        //}

        public void Send(CommandObject command)
        {
            m_write.Send(command);
        }

        private void InternalSend(ArraySegment<byte> packet)
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
