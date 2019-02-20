using System;
using System.IO;
using System.Threading;
using CTP.IO;
using GSF;

namespace CTP
{
    /// <summary>
    /// Serializes <see cref="CtpCommand"/> messages from a <see cref="Stream"/>.
    /// </summary>
    internal class CtpStreamReader : IDisposable
    {
        private object m_readLock = new object();
        private int m_readTimeout = 0;
        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;
        private readonly Action<object, Exception> m_onException;
        /// <summary>
        /// A buffer used to read data from the underlying stream.
        /// </summary>
        private byte[] m_readBuffer;
        private bool m_disposed;
        private CtpReadParser m_read;

        /// <summary>
        /// Creates a <see cref="CtpStreamReader"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="onException"></param>
        public CtpStreamReader(Stream stream, Action<object, Exception> onException)
        {
            m_stream = stream;
            m_onException = onException;
            m_readTimeout = stream.ReadTimeout;
            m_readBuffer = new byte[3000];
            m_read = new CtpReadParser();
        }

        /// <summary>
        /// Reads the next command from the stream. 
        /// </summary>
        /// <returns></returns>
        public CtpCommand Read(int timeout)
        {
            try
            {
                lock (m_readLock)
                {
                    if (m_disposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    CtpCommand packet;
                    while (!m_read.ReadFromBuffer(out packet))
                    {
                        if (m_disposed)
                            throw new ObjectDisposedException(GetType().FullName);
                        //ToDo: Consider not resetting the timeout if multiple while loops is required to read a single command.
                        if (m_readTimeout != timeout)
                        {
                            m_readTimeout = timeout;
                            m_stream.ReadTimeout = timeout;
                        }
                        int length = m_stream.Read(m_readBuffer, 0, m_readBuffer.Length);
                        m_read.AppendToBuffer(m_readBuffer, length);
                    }
                    return packet;
                }
            }
            catch (Exception e)
            {
                m_onException(this, e);
                throw;
            }

        }

        public void Dispose()
        {
            m_disposed = true;
            m_read.Dispose();
        }
    }
}
