using System;
using System.Collections.Generic;
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
        }

        /// <summary>
        /// The maximum packet size before protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// Writes a command to the underlying stream. Note: this method blocks until a packet has successfully been sent.
        /// </summary>
        public void Write(ArraySegment<byte> data, int timeout)
        {
            try
            {
                if ((object)data.Array == null)
                    throw new ArgumentNullException(nameof(data));

                if (data.Count > MaximumPacketSize)
                    throw new Exception("This command is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

                lock (m_writeLock)
                {
                    if (m_disposed)
                        throw new ObjectDisposedException("Stream has been closed");
                    if (m_writeTimeout != timeout)
                    {
                        m_writeTimeout = timeout;
                        m_stream.WriteTimeout = timeout;
                    }
                    m_stream.Write(data.Array, data.Offset, data.Count);
                }
            }
            catch (Exception ex)
            {
                m_disposed = true;
                m_onException(this, ex);
                throw;
            }
        }

        public void Dispose()
        {
            m_disposed = true;
        }
    }
}
