using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GSF;
using GSF.Threading;

namespace CTP
{
    //ToDo: Final Review: Done

    /// <summary>
    /// Serializes <see cref="CtpPacket"/> messages to a <see cref="Stream"/> in a synchronous manner.
    /// </summary>
    internal class CtpStreamWriterAsync : IDisposable
    {
        public event Action<object, Exception> OnException;

        private object m_syncLock = new object();

        /// <summary>
        /// The underlying stream
        /// </summary>
        private Stream m_stream;

        /// <summary>
        /// The buffer used to write packets to the stream.
        /// </summary>
        private byte[] m_writeBuffer;

        private AsyncCallback m_endWrite;

        private WaitCallback m_processNextWrite;

        private Queue<Tuple<ShortTime, CtpCommand>> m_queue;

        private ScheduledTask m_processTimeouts;

        private int m_timeout;

        private bool m_isWriting;

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
        public CtpStreamWriterAsync(Stream stream, int timeout)
        {
            m_timeout = timeout;
            m_stream = stream;
            m_writeBuffer = new byte[128];
            m_endWrite = EndWrite;
            m_queue = new Queue<Tuple<ShortTime, CtpCommand>>();

            m_processTimeouts = new ScheduledTask();
            m_processTimeouts.Running += M_processTimeouts_Running;
            m_processTimeouts.Start(100);

            m_processNextWrite = ProcessNextWrite;
        }

        private void M_processTimeouts_Running(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            if (m_disposed)
                return;

            m_processTimeouts.Start(100);

            lock (m_syncLock)
            {
                if (m_queue.Count > 0)
                {
                    if (m_queue.Peek().Item1.ElapsedMilliseconds() > m_timeout)
                    {
                        m_disposed = true;
                        ThreadPool.QueueUserWorkItem(x => OnException?.Invoke(this, new TimeoutException()));
                    }
                }
            }
        }

        private void ProcessNextWrite(object state)
        {
            TryAgain:

            if (m_disposed)
                return;

            CtpCommand packet;
            lock (m_syncLock)
            {
                if (m_queue.Count > 0)
                {
                    packet = m_queue.Dequeue().Item2;
                }
                else
                {
                    m_isWriting = false;
                    return;
                }
            }
            try
            {
                EnsureCapacity(packet.Length);
                packet.CopyTo(m_writeBuffer,0);
                if (m_stream.BeginWrite(m_writeBuffer, 0, packet.Length, m_endWrite, null).CompletedSynchronously)
                    goto TryAgain;
                //If this completed async, the EndWrite method will return control to this method.
            }
            catch (Exception e)
            {
                m_disposed = true;
                OnException?.Invoke(this, e);
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
                OnException?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Writes a packet to the underlying stream. 
        /// </summary>
        public void Write(CtpCommand packet)
        {
            if ((object)packet == null)
                throw new ArgumentNullException(nameof(packet));

            //In case of an overflow exception.
            if (packet.Length > MaximumPacketSize)
                throw new Exception("This packet is too large to send, if this is a legitimate size, increase the MaxPacketSize.");

            lock (m_syncLock)
            {
                m_queue.Enqueue(Tuple.Create(ShortTime.Now, packet));
                if (!m_isWriting)
                {
                    m_isWriting = true;
                    ThreadPool.QueueUserWorkItem(m_processNextWrite);
                }
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
