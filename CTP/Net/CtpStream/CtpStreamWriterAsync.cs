using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GSF;
using GSF.Threading;

namespace CTP
{
    /// <summary>
    /// Serializes <see cref="CtpCommand"/> messages to a <see cref="Stream"/> in a synchronous manner.
    /// </summary>
    internal class CtpStreamWriterAsync : IDisposable
    {
        private Action<object, Exception> m_onException;

        private object m_syncLock = new object();

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;

        private AsyncCallback m_endWrite;

        private WaitCallback m_processNextWrite;

        private Queue<Tuple<ShortTime, ArraySegment<byte>>> m_queue;

        private ScheduledTask m_processTimeouts;

        private int m_timeout;

        private bool m_isWritePumping;

        private volatile bool m_disposed;

        /// <summary>
        /// The maximum packet size before a protocol parsing exceptions are raised. This defaults to 1MB.
        /// </summary>
        public int MaximumPacketSize { get; set; } = 1_000_000;

        /// <summary>
        /// Creates a <see cref="CtpStreamWriter"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="timeout"></param>
        /// <param name="onException"></param>
        public CtpStreamWriterAsync(Stream stream, int timeout, Action<object, Exception> onException)
        {
            m_onException = onException ?? throw new ArgumentNullException(nameof(onException));
            m_timeout = timeout;
            m_stream = stream;
            m_endWrite = EndWrite;
            m_queue = new Queue<Tuple<ShortTime, ArraySegment<byte>>>();

            m_processTimeouts = new ScheduledTask();
            m_processTimeouts.Running += M_processTimeouts_Running;
            m_processTimeouts.Start(100);
            m_processNextWrite = ProcessNextWrite;
        }

        public int Timeout
        {
            get => m_timeout;
            set
            {
                m_timeout = value;
                m_processTimeouts.Start();
            }
        }

        private void M_processTimeouts_Running(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            if (m_disposed)
                return;

            m_processTimeouts.Start(100);

            lock (m_syncLock)
            {
                if (m_disposed)
                {
                    m_queue = null;
                    m_processTimeouts.Dispose();
                    return;
                }

                if (m_queue.Count > 0)
                {
                    if (m_queue.Peek().Item1.ElapsedMilliseconds() > m_timeout)
                    {
                        m_queue = null;
                        m_processTimeouts.Dispose();
                        m_disposed = true;
                        m_onException(this, new TimeoutException());
                    }
                }
            }
        }

        private void ProcessNextWrite(object state)
        {
            TryAgain:
            if (m_disposed)
                return;

            ArraySegment<byte> data;
            lock (m_syncLock)
            {
                if (m_disposed)
                {
                    m_queue = null;
                    return;
                }
                if (m_queue.Count > 0)
                {
                    data = m_queue.Dequeue().Item2;
                }
                else
                {
                    m_isWritePumping = false;
                    return;
                }
            }
            try
            {
                if (m_stream.BeginWrite(data.Array, data.Offset, data.Count, m_endWrite, null).CompletedSynchronously)
                    goto TryAgain;
                //If this completed async, the EndWrite method will return control to this method.
            }
            catch (Exception e)
            {
                m_disposed = true;
                m_onException(this, e);
            }
        }

        private void EndWrite(IAsyncResult ar)
        {
            try
            {
                m_stream.EndWrite(ar);
                if (ar.CompletedSynchronously) //Allow the BeginWrite method to loop. This prevents stack overflow issues.
                    return;
                else
                    ProcessNextWrite(null);
            }
            catch (Exception e)
            {
                m_disposed = true;
                m_onException(this, e);
            }
        }

        /// <summary>
        /// Writes a packet to the underlying stream. 
        /// </summary>
        public void Write(ArraySegment<byte> data)
        {
            if ((object)data.Array == null)
                throw new ArgumentNullException(nameof(data));

            //In case of an overflow exception.
            if (data.Count > MaximumPacketSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            lock (m_syncLock)
            {
                if (m_disposed)
                {
                    m_queue = null;
                    return;
                }

                m_queue.Enqueue(Tuple.Create(ShortTime.Now, data));
                if (!m_isWritePumping)
                {
                    m_isWritePumping = true;
                    ThreadPool.QueueUserWorkItem(m_processNextWrite);
                }
            }
        }

        public void Dispose()
        {
            m_disposed = true;
        }
    }
}
