using System;
using System.IO;
using System.Threading;
using GSF;

namespace CTP
{
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

    /// <summary>
    /// This class will wrap <see cref="CtpStream"/> and provide basic functionality to convert the synchronous
    /// <see cref="CtpStream"/> into one that can behave in multiple modes.
    /// </summary>
    public class CtpStream : IDisposable
    {
        public event Action OnDisposed;

        private CtpStreamReader m_reader;
        private CtpStreamReaderAsync m_readerAsync;
        private CtpStreamWriter m_writer;
        private CtpStreamWriterAsync m_writerAsync;

        /// <summary>
        /// For Blocking Mode: This event will not be raised.
        /// For Queuing Mode: This event will be raised 
        /// </summary>
        public event Action<CtpPacket> PacketReceived;

        public SendMode SendMode { get; private set; }

        public ReceiveMode ReceiveMode { get; private set; }

        private Stream m_stream;

        private bool m_started;

        private int m_sendTimeout;

        private int m_receiveTimeout;

        private volatile bool m_disposed;

        private object m_syncRoot = new object();

        public CtpStream(Stream stream)
        {
            m_stream = stream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveMode">The mode in which the receive function will operate</param>
        /// <param name="sendMode">The mode in which the send function will operate</param>
        /// <param name="receiveTimeout">
        /// May be 0.
        /// If ReceiveMode = Blocking: The maximum time to block for a read command.
        /// Otherwise Ignored.
        /// </param>
        /// <param name="sendTimeout"> Must be positive milliseconds
        /// If SendMode = Blocking: The timeout permitted during each Send operation
        /// If SendMode = Queuing: The depth of the queue permitted before the stream is deemed too far behind and is disposed.</param>
        public void Start(ReceiveMode receiveMode, int receiveTimeout, SendMode sendMode, int sendTimeout)
        {
            if (sendTimeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(sendTimeout), "SendTimeout cannot be infinite");
            if (receiveTimeout < 0)
                throw new ArgumentOutOfRangeException(nameof(receiveTimeout), "ReceiveTimeout cannot be infinite");

            lock (m_syncRoot)
            {
                if (m_started)
                    throw new Exception("Already Started");
                try
                {
                    m_sendTimeout = sendTimeout;
                    m_receiveTimeout = receiveTimeout;

                    SendMode = sendMode;
                    ReceiveMode = receiveMode;

                    switch (SendMode)
                    {
                        case SendMode.Blocking:
                            m_writer = new CtpStreamWriter(m_stream);
                            m_writer.OnException += OnException;
                            break;
                        case SendMode.Queueing:
                            m_writerAsync.OnException += OnException;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(receiveMode), receiveMode, null);
                    }
                    switch (ReceiveMode)
                    {
                        case ReceiveMode.Blocking:
                            m_reader = new CtpStreamReader(m_stream);
                            m_reader.OnException += OnException;
                            break;
                        case ReceiveMode.Event:
                            m_readerAsync = new CtpStreamReaderAsync(m_stream);
                            m_readerAsync.OnException += OnException;
                            m_readerAsync.NewPacket += OnNewPacket;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(receiveMode), receiveMode, null);
                    }

                    m_readerAsync?.Start();
                }
                finally
                {
                    m_started = true;
                }
            }
        }

        private void OnException(object arg1, Exception arg2)
        {
            Dispose();
        }

        private void OnNewPacket(CtpPacket obj)
        {
            if (m_disposed)
                return;
            PacketReceived?.Invoke(obj);
        }

        /// <summary>
        /// Attempts a read operation.
        /// When ReceiveMode = Blocking,
        ///     this method will block until a read has occurred. If the stream has been disposed, an exception will be raised.
        /// When ReceiveMode = Queuing,
        ///     this method will not block, nor raise an exception if the stream has been disposed.
        /// </summary>
        /// <returns></returns>
        public CtpPacket Read()
        {
            if (!m_started)
                throw new Exception("Call Start");

            switch (ReceiveMode)
            {
                case ReceiveMode.Blocking:
                    return m_reader.Read(m_receiveTimeout);
                case ReceiveMode.Event:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SendRaw(byte[] data, byte channel)
        {
            if (!m_started)
                throw new Exception("Call Start");
            Send(new CtpPacket(channel, true, data));
        }

        public void Send(DocumentObject document, byte channel = 0)
        {
            if (!m_started)
                throw new Exception("Call Start");
            Send(new CtpPacket(channel, document));
        }

        private void Send(CtpPacket packet)
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

        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;
                m_stream?.Dispose();
                m_reader?.Dispose();
                m_readerAsync?.Dispose();
                m_writer?.Dispose();
                m_writerAsync?.Dispose();
                OnDisposed?.Invoke();
            }
        }
    }
}
