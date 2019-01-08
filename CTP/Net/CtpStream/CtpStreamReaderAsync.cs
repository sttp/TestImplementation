using System;
using System.IO;
using System.Threading;
using GSF;

namespace CTP
{
    /// <summary>
    /// Serializes <see cref="CtpCommand"/> messages from a <see cref="Stream"/>.
    /// </summary>
    internal class CtpStreamReaderAsync : CtpStreamReaderBase
    {
        public event Action<CtpCommand> NewPacket;
        private AsyncCallback m_endRead;
        private WaitCallback m_beginRead;
        private bool m_started;
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
        public CtpStreamReaderAsync(Stream stream, Action<object, Exception> onException)
            : base(onException)
        {
            m_stream = stream;
            m_beginRead = BeginRead;
            m_endRead = EndRead;
            m_readBuffer = new byte[3000];
        }

        public void Start()
        {
            if (m_started)
                throw new Exception("Already started");
            m_started = true;
            ThreadPool.QueueUserWorkItem(m_beginRead, null);
        }

        private void BeginRead(object state)
        {
            if (Disposed)
                return;
            try
            {
                IAsyncResult async = m_stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, m_endRead, null);
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        private void EndRead(IAsyncResult ar)
        {
            try
            {
                int length = m_stream.EndRead(ar);
                AppendToBuffer(m_readBuffer, length);
                while (ReadFromBuffer(out var command))
                {
                    if (Disposed)
                        return;

                    try
                    {
                        NewPacket?.Invoke(command);
                    }
                    catch (Exception e)
                    {

                    }
                }

                if (ar.CompletedSynchronously)
                    ThreadPool.QueueUserWorkItem(m_beginRead, null);
                else
                    BeginRead(null);
            }
            catch (Exception e)
            {
                OnException(e);
            }

        }
    }
}
