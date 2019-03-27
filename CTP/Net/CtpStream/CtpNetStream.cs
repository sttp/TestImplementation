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
using CTP.Collection;
using CTP.IO;
using GSF;
using GSF.Diagnostics;
using GSF.Threading;

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

        private CtpStreamReader m_reader;
        private CtpStreamReaderAsync m_readerAsync;
        private int m_sendTimeout;
        private int m_receiveTimeout;
        private ReceiveMode m_receiveMode;
        private bool m_disposed;
        private CtpWriteEncoder m_writeEncoder;


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

        /// <summary>
        /// Used to synchronize the <see cref="m_writeQueue"/> during writing, disposing, and timeout checks.
        /// </summary>
        private object m_writeSyncRoot = new object();

        private AsyncCallback m_endWrite;
        private WaitCallback m_processNextWrite;
        private ScheduledTask m_processWriteTimeouts;
        private Queue<Tuple<ShortTime, PooledBuffer, ManualResetEventSlim>> m_writeQueue;

        public CtpNetStream(TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            m_socket = socket;
            m_netStream = netStream;
            m_ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;

            m_sendTimeout = 5000;
            m_receiveTimeout = 5000;
            m_receiveMode = ReceiveMode.Blocking;
            m_reader = new CtpStreamReader(m_stream, SendReceiveOnException);
            m_writeEncoder = new CtpWriteEncoder(CtpCompressionMode.None, WriteCallback);

            m_endWrite = EndWrite;
            m_writeQueue = new Queue<Tuple<ShortTime, PooledBuffer, ManualResetEventSlim>>();

            m_processWriteTimeouts = new ScheduledTask();
            m_processWriteTimeouts.Running += ProcessWriteTimeoutsRunning;
            m_processWriteTimeouts.Start(100);
            m_processNextWrite = ProcessNextWrite;
        }

        /// <summary>
        /// Changes the ReceiveMode of the session. It starts out in a blocking mode, and can be changed into an event mode.
        /// However, it cannot be changed back into a blocking mode at the present time.
        /// </summary>
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

                            if (PacketReceived == null)
                                throw new Exception("User must handle PacketReceived event before changing the receive mode.");

                            Log.Publish(MessageLevel.Info, "Receive Mode Changed To Event");
                            m_readerAsync = new CtpStreamReaderAsync(m_stream, SendReceiveOnException);
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
                m_processWriteTimeouts.Start();
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

        /// <summary>
        /// A helper method to specifies all of the modes and timeouts in one swoop. This method is not required before reading/writing from a socket.
        /// </summary>
        /// <param name="receiveMode"></param>
        /// <param name="receiveTimeout"></param>
        /// <param name="sendMode"></param>
        /// <param name="sendTimeout"></param>
        public void Start(ReceiveMode receiveMode, int receiveTimeout, SendMode sendMode, int sendTimeout)
        {
            ReceiveTimeout = receiveTimeout;
            SendTimeout = sendTimeout;
            ReceiveMode = receiveMode;
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

                m_reader?.Dispose();
                m_readerAsync?.Dispose();
                m_processWriteTimeouts?.Dispose();
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
            lock (m_writeSyncRoot)
            {
                if (m_disposed)
                    return;
                m_disposed = true;
                while (m_writeQueue.Count > 0)
                {
                    var item = m_writeQueue.Dequeue();
                    item.Item2.Release();
                    item.Item3?.Set();
                }
            }
            ThreadPool.QueueUserWorkItem(InternalDispose);
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
            switch (ReceiveMode)
            {
                case ReceiveMode.Blocking:
                    return m_reader.Read(m_receiveTimeout);
                case ReceiveMode.Event:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region [ Write Methods ]

        /// <summary>
        /// Sends a command and blocks until the send operation has completed. Will throw a TimeoutException
        /// </summary>
        /// <param name="command"></param>
        public void Send(CommandObject command)
        {
            var wait = new ManualResetEventSlim(false, 1);
            m_writeEncoder.Send(command, wait);
            if (!wait.Wait(m_sendTimeout))
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
            m_writeEncoder.Send(command);
        }

        private void ProcessWriteTimeoutsRunning(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            if (m_disposed)
                return;

            m_processWriteTimeouts.Start(100);

            lock (m_writeSyncRoot)
            {
                if (m_writeQueue.Count == 0)
                    return;
                if (m_writeQueue.Peek().Item1.ElapsedMilliseconds() < m_sendTimeout)
                    return;
            }
            SendReceiveOnException(this, new TimeoutException());
        }

        /// <summary>
        /// Writes a packet to the underlying stream. 
        /// </summary>
        private void WriteCallback(PooledBuffer data, ManualResetEventSlim resetEvent)
        {
            if ((object)data == null)
                throw new ArgumentNullException(nameof(data));

            lock (m_writeSyncRoot)
            {
                if (m_disposed)
                {
                    //Silently ignore the send operation and do not error.
                    data.Release();
                    resetEvent?.Set();
                    return;
                }

                m_writeQueue.Enqueue(Tuple.Create(ShortTime.Now, data, resetEvent));

                if (m_writeQueue.Count > 1)
                {
                    //if there are more than 1 items in the queue, this means some other thread is actively working the queue.
                    return;
                }
            }

            ProcessNextWrite(null);
        }

        private void ProcessNextWrite(object state)
        {
            Tuple<ShortTime, PooledBuffer, ManualResetEventSlim> tuple;
            lock (m_writeSyncRoot)
            {
                if (m_disposed)
                    return;
                if (m_writeQueue.Count == 0)
                    return;
                tuple = m_writeQueue.Peek();
            }

            try
            {
                tuple.Item2.CopyToBeginWrite(m_stream, m_endWrite, null);
            }
            catch (Exception e)
            {
                tuple.Item3?.Set();
                tuple.Item2.Release();
                SendReceiveOnException(this, e);
            }
        }

        private void EndWrite(IAsyncResult ar)
        {
            try
            {
                m_stream.EndWrite(ar);
                Tuple<ShortTime, PooledBuffer, ManualResetEventSlim> tuple;
                lock (m_writeSyncRoot)
                {
                    if (m_writeQueue.Count == 0)
                        return; //There's a possible race condition on a dispose operation.
                    tuple = m_writeQueue.Dequeue();
                    if (m_writeQueue.Count > 0)
                        ThreadPool.QueueUserWorkItem(m_processNextWrite);
                }

                tuple.Item2.Release();
                tuple.Item3?.Set();
            }
            catch (Exception e)
            {
                SendReceiveOnException(this, e);
            }
        }

        #endregion


    }
}
