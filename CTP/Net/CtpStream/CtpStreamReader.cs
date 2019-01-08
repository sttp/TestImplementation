using System;
using System.IO;
using System.Threading;
using GSF;

namespace CTP
{
    /// <summary>
    /// Serializes <see cref="CtpCommand"/> messages from a <see cref="Stream"/>.
    /// </summary>
    internal class CtpStreamReader : CtpStreamReaderBase
    {
        private object m_readLock = new object();
        private int m_readTimeout = 0;
        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;
        /// <summary>
        /// A buffer used to read data from the underlying stream.
        /// </summary>
        private byte[] m_readBuffer;

        /// <summary>
        /// Creates a <see cref="CtpStreamReader"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="onException"></param>
        public CtpStreamReader(Stream stream, Action<object, Exception> onException)
            : base(onException)
        {
            m_stream = stream;
            m_readTimeout = stream.ReadTimeout;
            m_readBuffer = new byte[3000];
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
                    if (Disposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    CtpCommand command;
                    while (!ReadFromBuffer(out command))
                    {
                        if (Disposed)
                            throw new ObjectDisposedException(GetType().FullName);
                        //ToDo: Consider not resetting the timeout if multiple while loops is required to read a single command.
                        if (m_readTimeout != timeout)
                        {
                            m_readTimeout = timeout;
                            m_stream.ReadTimeout = timeout;
                        }
                        int length = m_stream.Read(m_readBuffer, 0, m_readBuffer.Length);
                        AppendToBuffer(m_readBuffer, length);
                    }
                    return command;
                }
            }
            catch (Exception e)
            {
                OnException(e);
                throw;
            }

        }
    }
}
