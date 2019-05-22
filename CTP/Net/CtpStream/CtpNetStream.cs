using System;
using System.Collections.Generic;
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

namespace CTP.Net
{
    public delegate void PacketReceivedEventHandler(CtpNetStream sender, CtpCommand command);

    public class CtpNetStream : IDisposable
    {
        private class StreamReading
        {
            private bool m_disposed;
            private bool m_isReading;
            private Stream m_stream;
            private Action m_notify;
            private Action<Exception> m_onException;
            private WeakActionAsync<Task<int>> m_continueRead;

            /// <summary>
            /// A buffer used to read data from the underlying stream.
            /// </summary>
            private byte[] m_readBuffer;
            private CtpReadDecoder m_readDecoder;
            private object m_syncRoot = new object();
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
                m_notify = notify ?? throw new ArgumentNullException(nameof(notify));
                m_stream = stream ?? throw new ArgumentNullException(nameof(stream));
                m_onException = onException ?? throw new ArgumentNullException(nameof(onException));

                m_readBuffer = new byte[3000];
                m_readDecoder = new CtpReadDecoder();
                m_continueRead = new WeakActionAsync<Task<int>>(ContinueRead);
            }

            /// <summary>
            /// Attempts to read a command. If this fails, a wait object is returned to indicate when a receive is successful.
            /// A callback is also queued to indicate when more data has been received.
            /// </summary>
            /// <param name="command"></param>
            /// <param name="waiting"></param>
            /// <returns></returns>
            /// <exception cref="ObjectDisposedException"></exception>
            public bool TryRead(out CtpCommand command, out ManualResetEventSlim waiting)
            {
                waiting = null;
                command = null;

                try
                {
                    lock (m_syncRoot)
                    {
                        if (m_disposed)
                            throw new ObjectDisposedException(GetType().FullName);
                        if (m_isReading)
                        {
                            waiting = m_waiting;
                            return false;
                        }

                        command = TryReadInternal();
                        if ((object)command != null)
                            return true;
                        m_waiting = new ManualResetEventSlim(false, 1);
                        waiting = m_waiting;
                        return false;
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                    waiting = null;
                    return false;
                }
            }

            /// <summary>
            /// Must occur within a lock. This will attempt to read a Command and queue a read from the stream if it fails.
            /// </summary>
            /// <returns></returns>
            private CtpCommand TryReadInternal()
            {
                if ((object)m_pendingCommand != null)
                {
                    var command = m_pendingCommand;
                    m_pendingCommand = null;
                    return command;
                }

                while (true)
                {
                    if (m_readDecoder.ReadFromBuffer(out var command))
                        return command;

                    var task = m_stream.ReadAsync(m_readBuffer, 0, m_readBuffer.Length);
                    if (!task.IsCompleted)
                    {
                        m_isReading = true;
                        task.ContinueWith(m_continueRead.AsyncCallback);
                        return null;
                    }
                    if (task.IsFaulted)
                        throw task.Exception ?? new Exception("Task failed with no exception specified");
                    if (task.IsCanceled)
                        throw new Exception("Receive was canceled");
                    m_readDecoder.AppendToBuffer(m_readBuffer, task.Result);
                }
            }

            private void ContinueRead(Task<int> task)
            {
                try
                {
                    if (m_disposed)
                        return;
                    if (!task.IsCompleted)
                        throw new Exception("Only completed tasks should be here.");
                    if (task.IsFaulted)
                        throw task.Exception ?? new Exception("Task failed with no exception specified");
                    if (task.IsCanceled)
                        throw new Exception("Receive was canceled");

                    ManualResetEventSlim waiting;
                    lock (m_syncRoot)
                    {
                        if (m_disposed)
                            return;
                        if (!m_isReading)
                            throw new Exception("IsReading should have been set to true.");
                        m_isReading = false;
                        m_readDecoder.AppendToBuffer(m_readBuffer, task.Result);

                        m_pendingCommand = TryReadInternal();
                        if ((object)m_pendingCommand == null)
                            return; //If null, this means another read asyc operation started.

                        waiting = m_waiting;
                        m_waiting = null;
                    }

                    waiting?.Set();
                    try
                    {
                        if (!m_disposed)
                            m_notify();
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
                Dispose();
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
                    if (m_disposed)
                        return;
                    m_disposed = true;
                    m_continueRead.Dispose();
                    m_waiting?.Set();
                    m_waiting = null;
                }
                try
                {
                    m_notify();
                }
                catch (Exception)
                {
                }
            }
        }

        private class StreamWriting
        {
            private Stream m_stream;
            private Action m_notify;
            private Action<Exception> m_onException;
            private WeakActionAsync<Task> m_continueWrite;

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
            /// <param name="notify">A callback that occurs after an asynchronous read returns successfully. This is not signaled all the time, only when calls to <see cref="TryWrite"/> fail</param>
            /// <param name="onException">Is called anytime that the socket raises an exception. The class is automatically disposed.</param>
            public StreamWriting(Stream stream, Action notify, Action<Exception> onException)
            {
                m_sendTimeout = 5000;
                m_notify = notify ?? throw new ArgumentNullException(nameof(notify));
                m_stream = stream ?? throw new ArgumentNullException(nameof(stream));
                m_onException = onException ?? throw new ArgumentNullException(nameof(onException));

                m_processWriteTimeouts = new ScheduledTask();
                m_processWriteTimeouts.Running += ProcessWriteTimeoutsRunning;
                m_writeEncoder = new CtpWriteEncoder(CtpCompressionMode.None, WriteCallback);
                m_writeQueue = new Queue<Tuple<ShortTime, PooledBuffer, ManualResetEventSlim>>();
                m_continueWrite = new WeakActionAsync<Task>(ContinueWrite);
            }

            /// <summary>
            /// The timeout in milliseconds between 1 and 60,000
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
            /// Queues a command to write. Will not block or throw exceptions.
            /// When disposed, the wait handle is null
            /// </summary>
            /// <param name="command"></param>
            /// <param name="wait"></param>
            public bool TryWrite(CommandObject command, out ManualResetEventSlim wait)
            {
                try
                {
                    lock (m_syncRoot)
                    {
                        if (m_disposed)
                        {
                            wait = null;
                            return false;
                        }

                        wait = m_writeEncoder.Send(command);
                        return wait == null;
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                    wait = null;
                    return false;
                }
            }

            /// <summary>
            /// Writes a packet to the underlying stream. This is called in a sync context
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
                    if (task.IsFaulted)
                        throw task.Exception ?? new Exception("Task failed with no exception specified");
                    if (task.IsCanceled)
                        throw new Exception("Receive was canceled");
                    data.Release();
                    return null;
                }
                else
                {
                    m_processWriteTimeouts.Start(m_sendTimeout + 10);
                    var resetEvent = new ManualResetEventSlim(false, 1);
                    m_writeQueue.Enqueue(Tuple.Create(ShortTime.Now, data, resetEvent));
                    task.ContinueWith(m_continueWrite.AsyncCallback);
                    return resetEvent;
                }
            }

            private void ContinueWrite(Task task)
            {
                try
                {
                    TryAgain:

                    if (m_disposed)
                        return;
                    if (!task.IsCompleted)
                        throw new Exception("Only completed tasks should be here.");
                    if (task.IsFaulted)
                        throw task.Exception ?? new Exception("Task failed with no exception specified");
                    if (task.IsCanceled)
                        throw new Exception("Receive was canceled");

                    lock (m_syncRoot)
                    {
                        if (m_disposed)
                            return;
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
                                task.ContinueWith(m_continueWrite.AsyncCallback);
                            }
                        }
                    }

                    try
                    {
                        if (!m_disposed)
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
                lock (m_syncRoot)
                {
                    if (m_disposed)
                        return;
                    if (m_writeQueue.Count == 0)
                        return;
                    var msUntilTimeout = m_sendTimeout - m_writeQueue.Peek().Item1.ElapsedMilliseconds();
                    if (msUntilTimeout > 0)
                    {
                        m_processWriteTimeouts.Start((int)(msUntilTimeout + 10));
                        return;
                    }
                }
                OnException(new TimeoutException());
            }


            private void OnException(Exception e)
            {
                Dispose();

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
                    if (m_disposed)
                        return;
                    m_disposed = true;
                    m_continueWrite.Dispose();

                    while (m_writeQueue.Count > 0)
                    {
                        var tuple = m_writeQueue.Dequeue();
                        tuple.Item2.Release();
                        tuple.Item3?.Set();
                    }
                }
                try
                {
                    m_notify();
                }
                catch (Exception)
                {

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

        private int m_receiveTimeout;

        private bool m_disposed;

        private bool m_readEvents;

        /// <summary>
        /// The name of the mapped account.
        /// </summary>
        public string AccountName = string.Empty;
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

        private object m_syncRoot = new object();

        private StreamReading m_read;
        private StreamWriting m_write;

        public CtpNetStream(TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            m_socket = socket;
            m_netStream = netStream;
            m_ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;

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
                    if (value)
                    {
                        m_processReadEvents.Start();
                    }
                }
            }
        }

        /// <summary>
        /// The time that something can be pending a send operation before an exception is thrown.
        /// </summary>
        public int SendTimeout
        {
            get => m_write.SendTimeout;
            set => m_write.SendTimeout = value;
        }

        /// <summary>
        /// The default timeout during a blocking read before the socket is timed out. Must be between 1 and 60,000 milliseconds.
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

        private void OnNewPacket(CtpCommand obj)
        {
            if (m_disposed)
                return;
            PacketReceived?.Invoke(this, obj);
        }

        /// <summary>
        /// Sends a command and blocks until the send operation has completed. Will throw a TimeoutException
        /// </summary>
        /// <param name="command"></param>
        public void Send(CommandObject command)
        {
            m_write.TryWrite(command, out var wait);
            if (wait != null && !wait.Wait(SendTimeout))
            {
                OnException(new TimeoutException());
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// Queues a command to send. Will not block or throw exceptions.
        /// </summary>
        /// <param name="command"></param>
        public void SendAsync(CommandObject command)
        {
            m_write.TryWrite(command, out var wait);
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
            int totalWait = m_receiveTimeout;

            TryAgain:

            ManualResetEventSlim wait;

            if (m_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (m_read.TryRead(out var packet, out wait))
            {
                return packet;
            }

            if (wait == null)
                throw new ObjectDisposedException(GetType().FullName);

            ShortTime time = ShortTime.Now;
            if (!wait.Wait(totalWait))
            {
                OnException(new TimeoutException());
                throw new TimeoutException();
            }

            totalWait = Math.Max(0, (int)(totalWait - time.ElapsedMilliseconds()));

            goto TryAgain;
        }


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

        private void NotifyWrite()
        {

        }

        private void NotifyDataReady()
        {
            m_processReadEvents.Start();
        }

        private void OnException(Exception ex)
        {
            Log.Publish(MessageLevel.Error, MessageFlags.None, "Send/Receive Error", "An error has occurred with sending or receiving from a socket. The socket will be disposed.", null, ex);
            Dispose();
        }

        public void Dispose()
        {
            lock (m_syncRoot)
            {
                if (m_disposed)
                    return;
                m_disposed = true;
            }
            ThreadPool.QueueUserWorkItem(InternalDispose);
        }

        private void InternalDispose(object state)
        {
            try
            {
                Log.Publish(MessageLevel.Info, "Disposed Called");
                m_write.Dispose();
                m_read.Dispose();
                ThreadPool.QueueUserWorkItem(OnDisposedCallback);
                m_ssl?.Dispose();
                m_netStream?.Dispose();
                m_socket?.Dispose();
                m_stream?.Dispose();
            }
            catch (Exception e)
            {
                Logger.SwallowException(e, "Error occurred during dispose");
            }
        }

        private void OnDisposedCallback(object state)
        {
            Log.Publish(MessageLevel.Info, "Disposed Event Raised");
            try
            {
                OnDisposed?.Invoke();
            }
            catch (Exception)
            {
            }
        }

    }
}
