using System;
using System.IO;
using System.Threading;

namespace CTP
{
    /// <summary>
    /// Serializes <see cref="CtpCommand"/> messages to a <see cref="Stream"/> in a synchronous manner.
    /// </summary>
    internal class CtpStreamWriter : IDisposable
    {
        private Action<object, Exception> m_onException;

        private object m_writeLock = new object();

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;

        /// <summary>
        /// The buffer used to write packets to the stream.
        /// </summary>
        private byte[] m_writeBuffer;

        private int m_writeTimeout;

        private bool m_disposed;

        /// <summary>
        /// Creates a <see cref="CtpStreamWriter"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="onException"></param>
        public CtpStreamWriter(Stream stream, Action<object, Exception> onException)
        {
            m_onException = onException ?? throw new ArgumentNullException(nameof(onException));
            m_stream = stream;
            m_writeTimeout = stream.WriteTimeout;
            m_writeBuffer = new byte[128];
        }

        /// <summary>
        /// The maximum packet size before protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// Writes a command to the underlying stream. Note: this method blocks until a packet has successfully been sent.
        /// </summary>
        public void Write(CtpCommand command, int timeout)
        {
            try
            {
                if ((object)command == null)
                    throw new ArgumentNullException(nameof(command));

                if (command.Length > MaximumPacketSize)
                    throw new Exception("This command is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

                lock (m_writeLock)
                {
                    EnsureCapacity(command.Length);
                    command.CopyTo(m_writeBuffer, 0);

                    if (m_disposed)
                        throw new ObjectDisposedException("Stream has been closed");
                    if (m_writeTimeout != timeout)
                    {
                        m_writeTimeout = timeout;
                        m_stream.WriteTimeout = timeout;
                    }
                    m_stream.Write(m_writeBuffer, 0, command.Length);
                }
            }
            catch (Exception ex)
            {
                m_disposed = true;
                m_onException(this, ex);
                throw;
            }
        }

        /// <summary>
        /// Ensures that <see cref="m_writeBuffer"/> has at least the supplied number of bytes
        /// before returning.
        /// </summary>
        /// <param name="bufferSize"></param>
        private void EnsureCapacity(int bufferSize)
        {
            if (m_writeBuffer.Length < bufferSize)
            {
                //12% larger than the requested buffer size.
                byte[] newBuffer = new byte[bufferSize + (bufferSize >> 3)];
                m_writeBuffer.CopyTo(newBuffer, 0);
                m_writeBuffer = newBuffer;
            }
        }

        public void Dispose()
        {
            m_disposed = true;
        }
    }
}
