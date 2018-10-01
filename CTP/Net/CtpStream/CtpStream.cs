using System;
using System.IO;

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

        private Stream m_stream;

        private int m_sendTimeout;

        private int m_receiveTimeout;

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

        private volatile bool m_disposed;

        private object m_syncRoot = new object();
        private SendMode m_sendMode;
        private ReceiveMode m_receiveMode;

        public CtpStream(Stream stream)
        {
            m_stream = stream;
            m_sendTimeout = 5000;
            m_receiveTimeout = 5000;
            m_sendMode = SendMode.Blocking;
            m_receiveMode = ReceiveMode.Blocking;
            m_writer = new CtpStreamWriter(m_stream);
            m_writer.OnException += OnException;
            m_reader = new CtpStreamReader(m_stream);
            m_reader.OnException += OnException;
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
            Send(new CtpPacket(channel, true, data));
        }

        public void Send(DocumentObject document, byte channel = 0)
        {
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
