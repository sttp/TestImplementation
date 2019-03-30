using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CTP.Collection;
using CTP.IO;
using GSF;
using GSF.Diagnostics;
using GSF.Threading;
using CancellationToken = System.Threading.CancellationToken;

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
        private class StreamReading
        {
            private enum States
            {
                /// <summary>
                /// Occurs when socket IO is configured to not be reading.
                /// </summary>
                NotReading,
                /// <summary>
                /// Occurs when socket IO is pending.
                /// </summary>
                Reading,
                /// <summary>
                /// Occurs when disposed has been called and no more IO should occur.
                /// </summary>
                Disposed,
            }

            private States m_state;
            private Stream m_stream;
            private Action m_notify;
            private Action<Exception> m_onException;
            private Action<Task<int>> m_continueRead;

            /// <summary>
            /// A buffer used to read data from the underlying stream.
            /// </summary>
            private byte[] m_readBuffer;
            private CtpReadDecoder m_readDecoder;
            private object m_readSyncRoot = new object();
            private ManualResetEventSlim m_waiting;
            private CtpCommand m_pendingCommand;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="notify">A callback that occurs after an asynchronous read returns successfully. This is not signaled all the time, only when calls to <see cref="TryRead"/> fail</param>
            /// <param name="onException">Is called anytime that the socket raises an exception. The class is automatically disposed.</param>
            public StreamReading(Stream stream, Action notify, Action<Exception> onException)
            {
                m_state = States.NotReading;
                m_notify = notify ?? throw new ArgumentNullException(nameof(notify));
                m_stream = stream ?? throw new ArgumentNullException(nameof(stream));
                m_onException = onException ?? throw new ArgumentNullException(nameof(onException));

                m_readBuffer = new byte[3000];
                m_readDecoder = new CtpReadDecoder();
                m_continueRead = ContinueRead;
            }

            /// <summary>
            /// Attempts to read a command. If this fails, a wait object is returned to indicate when a receive is successful.
            /// </summary>
            /// <param name="command"></param>
            /// <param name="waiting"></param>
            /// <returns></returns>
            public bool TryRead(out CtpCommand command, out ManualResetEventSlim waiting)
            {
                waiting = null;
                command = null;

                try
                {
                    lock (m_readSyncRoot)
                    {
                        switch (m_state)
                        {
                            case States.Disposed:
                                throw new ObjectDisposedException(GetType().FullName);
                            case States.Reading:
                                waiting = m_waiting;
                                return false;
                            case States.NotReading:
                                if (TryReadInternal(out command))
                                {
                                    return true;
                                }
                                else
                                {
                                    m_state = States.Reading;
                                    m_waiting = new ManualResetEventSlim(false, 1);
                                    waiting = m_waiting;
                                    return false;
                                }
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                    return false;
                }
            }

            private bool TryReadInternal(out CtpCommand command)
            {
                if ((object)m_pendingCommand != null)
                {
                    command = m_pendingCommand;
                    m_pendingCommand = null;
                    return true;
                }

                while (true)
                {
                    if (m_readDecoder.ReadFromBuffer(out command))
                        return true;

                    var task = m_stream.ReadAsync(m_readBuffer, 0, m_readBuffer.Length);
                    if (!task.IsCompleted)
                    {
                        task.ContinueWith(m_continueRead, TaskContinuationOptions.RunContinuationsAsynchronously);
                        return false;
                    }
                    m_readDecoder.AppendToBuffer(m_readBuffer, task.Result);
                }
            }

            private void ContinueRead(Task<int> task)
            {
                try
                {
                    if (!task.IsCompleted)
                        return;

                    ManualResetEventSlim waiting;

                    lock (m_readSyncRoot)
                    {
                        if (m_state == States.NotReading)
                            throw new Exception("Invalid State");
                        if (m_state == States.Reading)
                        {
                            m_readDecoder.AppendToBuffer(m_readBuffer, task.Result);

                            if (!TryReadInternal(out m_pendingCommand))
                                return;

                            m_state = States.NotReading;
                        }
                        waiting = m_waiting;
                        m_waiting = null;
                    }

                    waiting.Set();
                    try
                    {
                        if (m_state != States.Disposed)
                        {
                            m_notify();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                }

            }

            private void OnException(Exception e)
            {
                lock (m_readSyncRoot)
                {
                    m_state = States.Disposed;
                }

                try
                {
                    m_onException(e);
                }
                catch (Exception)
                {
                }

            }


            public void Dispose()
            {
                lock (m_readSyncRoot)
                {
                    m_state = States.Disposed;
                }
            }


        }

        private class StreamWriting
        {
            private Stream m_stream;
            private Action m_notify;
            private Action<Exception> m_onException;
            private Action<Task> m_continueWrite;

            private CtpWriteEncoder m_writeEncoder;
            private object m_syncRoot = new object();
            private ScheduledTask m_processWriteTimeouts;
            private Queue<Tuple<ShortTime, PooledBuffer, ManualResetEventSlim>> m_writeQueue;
            private int m_sendTimeout;
            private bool m_disposed;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="notify">A callback that occurs after an asynchronous read returns successfully. This is not signaled all the time, only when calls to <see cref="TryRead"/> fail</param>
            /// <param name="onException">Is called anytime that the socket raises an exception. The class is automatically disposed.</param>
            public StreamWriting(Stream stream, Action notify, Action<Exception> onException)
            {
                m_notify = notify ?? throw new ArgumentNullException(nameof(notify));
                m_stream = stream ?? throw new ArgumentNullException(nameof(stream));
                m_onException = onException ?? throw new ArgumentNullException(nameof(onException));

                m_processWriteTimeouts = new ScheduledTask();
                m_processWriteTimeouts.Running += ProcessWriteTimeoutsRunning;
                m_processWriteTimeouts.Start(100);

                m_writeEncoder = new CtpWriteEncoder(CtpCompressionMode.None, WriteCallback);

                m_writeQueue = new Queue<Tuple<ShortTime, PooledBuffer, ManualResetEventSlim>>();

                m_continueWrite = ContinueWrite;
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
                    if (value < 1 || value > 60000)
                        throw new ArgumentOutOfRangeException(nameof(value), "SendTimeout must be between 1 and 60,000 milliseconds");
                    m_sendTimeout = value;
                    m_processWriteTimeouts.Start();
                }
            }

            /// <summary>
            /// Queues a command to send. Will not block or throw exceptions.
            /// </summary>
            /// <param name="command"></param>
            /// <param name="wait"></param>
            public bool TrySend(CommandObject command, out ManualResetEventSlim wait)
            {
                lock (m_syncRoot)
                {
                    if (m_disposed)
                    {
                        wait = new ManualResetEventSlim(false, 1);
                        wait.Set();
                        return false;
                    }

                    wait = m_writeEncoder.Send(command);
                    if (wait == null)
                        return true;
                    return false;
                }
            }

            /// <summary>
            /// Writes a packet to the underlying stream. 
            /// </summary>
            private ManualResetEventSlim WriteCallback(PooledBuffer data)
            {
                if ((object)data == null)
                    throw new ArgumentNullException(nameof(data));

                if (m_writeQueue.Count > 0)
                {
                    var resetEvent = new ManualResetEventSlim(false, 1);
                    m_writeQueue.Enqueue(Tuple.Create(ShortTime.Now, data, resetEvent));
                    return resetEvent;
                }

                var task = data.CopyToWriteAsync(m_stream);
                if (task.IsCompleted)
                {
                    data.Release();
                    return null;
                }
                else
                {
                    var resetEvent = new ManualResetEventSlim(false, 1);
                    m_writeQueue.Enqueue(Tuple.Create(ShortTime.Now, data, resetEvent));
                    task.ContinueWith(m_continueWrite, TaskContinuationOptions.RunContinuationsAsynchronously);
                    return resetEvent;
                }
            }

            private void ContinueWrite(Task task)
            {
                try
                {
                    TryAgain:

                    if (!task.IsCompleted)
                        return;

                    lock (m_syncRoot)
                    {
                        if (m_writeQueue.Count > 0)
                        {
                            var tuple = m_writeQueue.Dequeue();
                            tuple.Item2.Release();
                            tuple.Item3?.Set();
                        }

                        if (m_writeQueue.Count > 0)
                        {
                            var tuple = m_writeQueue.Peek();

                            task = tuple.Item2.CopyToWriteAsync(m_stream);
                            if (task.IsCompleted)
                            {
                                goto TryAgain;
                            }
                            else
                            {
                                task.ContinueWith(m_continueWrite, TaskContinuationOptions.RunContinuationsAsynchronously);
                            }

                        }
                    }

                    try
                    {
                        if (m_disposed)
                        {
                            m_notify();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                }

            }

            private void ProcessWriteTimeoutsRunning(object sender, EventArgs<ScheduledTaskRunningReason> e)
            {
                if (m_disposed)
                    return;

                m_processWriteTimeouts.Start(100);

                lock (m_syncRoot)
                {
                    if (m_writeQueue.Count == 0)
                        return;
                    if (m_writeQueue.Peek().Item1.ElapsedMilliseconds() < m_sendTimeout)
                        return;
                }
                OnException(new TimeoutException());
            }


            private void OnException(Exception e)
            {
                lock (m_syncRoot)
                {
                    m_disposed = true;
                }

                try
                {
                    m_onException(e);
                }
                catch (Exception)
                {
                }
            }

            public void Dispose()
            {
                lock (m_syncRoot)
                {
                    m_disposed = true;
                }
            }

        }

        public static LogPublisher Log = Logger.CreatePublisher(typeof(CtpNetStream), MessageClass.Component);

        /// <summary>
        /// Occurs when this class is disposed. This event is raised on a ThreadPool thread and will only be called once.
        /// </summary>
        public event Action OnDisposed;

        /// <summary>
        /// For Blocking Mode: This event will not be raised.
        /// For Queuing Mode: This event will be raised 
        /// </summary>
        public event PacketReceivedEventHandler PacketReceived;

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

        private readonly Stream m_stream;

        private int m_sendTimeout;
        private int m_receiveTimeout;
        private bool m_disposed;
        private bool m_readEvents;

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

        private ScheduledTask m_processReadEvents;

        private object m_readSyncRoot = new object();

        private StreamReading m_read;
        private StreamWriting m_write;

        public CtpNetStream(TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            m_socket = socket;
            m_netStream = netStream;
            m_ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;

            m_sendTimeout = 5000;
            m_receiveTimeout = 5000;

            m_read = new StreamReading(m_stream, NotifyDataReady, OnException);
            m_write = new StreamWriting(m_stream, NotifyWrite, OnException);

            m_processReadEvents = new ScheduledTask();
            m_processReadEvents.Running += ProcessReadEventsRunning;
        }

        /// <summary>
        /// Gets/Sets if this class will automatically process incoming data in the form of an event message.
        /// </summary>
        public bool ReadEvents
        {
            get => m_readEvents;
            set
            {
                if (m_readEvents != value)
                {
                    m_readEvents = value;
                    if (m_readEvents)
                    {
                        m_processReadEvents.Start();
                    }
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
            get => m_write.SendTimeout;
            set => m_write.SendTimeout = value;
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
                if (value < 1 || value > 60000)
                    throw new ArgumentOutOfRangeException(nameof(value), "ReceiveTimeout must be between 1 and 60,000 milliseconds");
                m_receiveTimeout = value;
            }
        }

        /// <summary>
        /// Event occurs when a send/receive operation has an exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        private void SendReceiveOnException(object sender, Exception ex)
        {
            Log.Publish(MessageLevel.Error, MessageFlags.None, "Send/Receive Error", "An error has occurred with sending or receiving from a socket. The socket will be disposed.", null, ex);
            Dispose();
        }

        private void OnNewPacket(CtpCommand obj)
        {
            if (m_disposed)
                return;
            PacketReceived?.Invoke(this, obj);
        }

        private void OnDisposedCallback(object state)
        {
            Log.Publish(MessageLevel.Info, "Disposed Event Raised");
            OnDisposed?.Invoke();
        }

        private void InternalDispose(object state)
        {
            try
            {
                Log.Publish(MessageLevel.Info, "Disposed Called");
                ThreadPool.QueueUserWorkItem(OnDisposedCallback);
                m_ssl?.Dispose();
                m_netStream?.Dispose();
                m_socket?.Dispose();
                m_stream?.Dispose();
                m_read.Dispose();
                m_write.Dispose();

            }
            catch (Exception e)
            {
                Logger.SwallowException(e, "Error occurred during dispose");
            }
        }

        public void Dispose()
        {
            if (m_disposed)
                return;
            ThreadPool.QueueUserWorkItem(InternalDispose);
        }

        /// <summary>
        /// Sends a command and blocks until the send operation has completed. Will throw a TimeoutException
        /// </summary>
        /// <param name="command"></param>
        public void Send(CommandObject command)
        {
            m_write.TrySend(command, out var wait);
            if (wait != null && !wait.Wait(m_sendTimeout))
            {
                SendReceiveOnException(this, new TimeoutException());
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// Queues a command to send. Will not block or throw exceptions.
        /// </summary>
        /// <param name="command"></param>
        public void SendAsync(CommandObject command)
        {
            m_write.TrySend(command, out var wait);
        }

        #region [ Read Methods ]

        private void ProcessReadEventsRunning(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            if (e.Argument == ScheduledTaskRunningReason.Disposing)
                return;
            while (ReadEvents && m_read.TryRead(out var command, out var waiting))
            {
                try
                {
                    OnNewPacket(command);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void OnException(Exception ex)
        {
            Log.Publish(MessageLevel.Error, MessageFlags.None, "Send/Receive Error", "An error has occurred with sending or receiving from a socket. The socket will be disposed.", null, ex);
            Dispose();
        }

        private void NotifyWrite()
        {
        }

        private void NotifyDataReady()
        {
            m_processReadEvents.Start();
        }

        /// <summary>
        /// Attempts a read operation.
        /// When ReceiveMode = Blocking,
        ///     this method will block until a read has occurred. If the stream has been disposed, an exception will be raised.
        /// When ReceiveMode = Event,
        ///     This method will throw an exception.
        /// </summary>
        /// <returns></returns>
        public CtpCommand Read()
        {
            TryAgain:

            ManualResetEventSlim wait;

            lock (m_readSyncRoot)
            {
                if (m_readEvents)
                    throw new NotSupportedException("Stream is currently configured to RaiseEvents. Set this property to false to change modes.");

                if (m_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (m_read.TryRead(out var packet, out wait))
                {
                    return packet;
                }
            }

            if (!wait.Wait(m_receiveTimeout))
            {
                SendReceiveOnException(this, new TimeoutException());
                throw new TimeoutException();
            }

            goto TryAgain;
        }



        #endregion


    }
}
