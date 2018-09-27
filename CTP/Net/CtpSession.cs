using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using GSF;
using GSF.Threading;

namespace CTP.Net
{
    public delegate void PacketReceivedEventHandler(CtpSession sender, CtpPacket packet);

    public enum ReceiveMode
    {
        Blocking,
        Queueing,
        Events
    }

    public enum SendMode
    {
        Blocking,
        Queueing
    }

    public class CtpSession : IDisposable
    {
        public event Action OnDisposed;

        private ConcurrentQueue<CtpPacket> m_writeQueue = new ConcurrentQueue<CtpPacket>();
        private ConcurrentQueue<CtpPacket> m_readQueue = new ConcurrentQueue<CtpPacket>();

        public readonly bool IsServer;
        /// <summary>
        /// Gets the socket that this session is on.
        /// </summary>
        private readonly TcpClient Socket;
        /// <summary>
        /// Gets the NetworkStream this session writes to.
        /// </summary>
        private readonly NetworkStream NetStream;
        /// <summary>
        /// The SSL used to authenticate the connection if available.
        /// </summary>
        private readonly SslStream Ssl;
        private Stream m_stream;
        private CtpStream m_ctpStream;

        /// <summary>
        /// The login name assigned to this session. Typically this will only be tracked by the server.
        /// </summary>
        public string LoginName = string.Empty;
        /// <summary>
        /// The roles granted to this session. Typically this will only be tracked by the server.
        /// </summary>
        public HashSet<string> GrantedRoles = new HashSet<string>();
        public IPEndPoint RemoteEndpoint => Socket.Client.RemoteEndPoint as IPEndPoint;
        public X509Certificate RemoteCertificate => Ssl?.RemoteCertificate;
        public X509Certificate LocalCertificate => Ssl?.LocalCertificate;
        public ShortTime LastSentTime { get; private set; }
        public ShortTime LastReceiveTime { get; private set; }

        public ScheduledTask m_processReads;
        public ScheduledTask m_processWrites;
        private bool m_processReceivedPackets;
        private bool m_started;

        public ReceiveMode ReceiveMode { get; private set; }
        public SendMode SendMode { get; private set; }

        public event PacketReceivedEventHandler PacketReceived;

        public CtpSession(CtpStream stream, bool isServer, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            stream.OnDisposed += StreamOnOnDisposed;
            m_ctpStream = stream;
            IsServer = isServer;
            Socket = socket;
            NetStream = netStream;
            Ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;

            m_processReads = new ScheduledTask(ThreadingMode.DedicatedBackground);
            m_processReads.Running += M_processReads_Running;
            m_processReads.Start();

            m_processWrites = new ScheduledTask(ThreadingMode.DedicatedBackground);
            m_processWrites.Running += M_processWrites_Running;
        }

        public void Start(ReceiveMode receiveMode, SendMode sendMode)
        {
            if (m_started)
                throw new Exception("Already Started");
            m_started = true;
            SendMode = sendMode;
            ReceiveMode = receiveMode;
            switch (receiveMode)
            {
                case ReceiveMode.Blocking:
                    break;
                case ReceiveMode.Queueing:
                case ReceiveMode.Events:
                    m_processReads.Start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(receiveMode), receiveMode, null);
            }
        }

        private void StreamOnOnDisposed()
        {
            Dispose();
            OnDisposed?.Invoke();
        }

        private void M_processWrites_Running(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            while (m_writeQueue.TryDequeue(out var packet))
            {
                m_ctpStream.Write(packet);
            }
        }

        private void M_processReads_Running(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            if (e.Argument == ScheduledTaskRunningReason.Disposing)
                return;

            LastReceiveTime = ShortTime.Now;
            while (true)
            {
                var packet = m_ctpStream.Read();
                switch (ReceiveMode)
                {
                    case ReceiveMode.Queueing:
                        m_readQueue.Enqueue(packet);
                        break;
                    case ReceiveMode.Events:
                        PacketReceived?.Invoke(this, packet);
                        break;
                    case ReceiveMode.Blocking:
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                //{
                //    if (m_ctpStream.Results.Channel == 0)
                //    {
                //        var cmd = new CtpDocument(m_ctpStream.Results.Payload);
                //        if (!m_commandHandler.TryHandle(this, cmd))
                //        {
                //            CommandReceived?.Invoke(this, cmd);
                //        }
                //    }
                //    else
                //    {
                //        var dataHandler = m_dataChannelHandler[m_ctpStream.Results.Channel];
                //        if (dataHandler != null)
                //            dataHandler.ProcessData(this, m_ctpStream.Results.Payload);
                //        else
                //            DataReceived?.Invoke(this, m_ctpStream.Results.Channel, m_ctpStream.Results.Payload);
                //    }
                //}
            }

        }

        public bool TryRead(CtpPacket packet)
        {
            switch (ReceiveMode)
            {
                case ReceiveMode.Blocking:
                    packet = m_ctpStream.Read();
                    return true;
                case ReceiveMode.Queueing:
                    return m_readQueue.TryDequeue(out packet);
                case ReceiveMode.Events:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SendRaw(byte[] data, byte channel)
        {
            switch (SendMode)
            {
                case SendMode.Blocking:
                    m_ctpStream.Write(new CtpPacket(channel, true, data));
                    LastSentTime = ShortTime.Now;
                    break;
                case SendMode.Queueing:
                    m_writeQueue.Enqueue(new CtpPacket(channel, true, data));
                    m_processWrites.Start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Send(DocumentObject document, byte channel = 0)
        {
            switch (SendMode)
            {
                case SendMode.Blocking:
                    m_ctpStream.Write(new CtpPacket(channel, document));
                    LastSentTime = ShortTime.Now;
                    break;
                case SendMode.Queueing:
                    m_writeQueue.Enqueue(new CtpPacket(channel, document));
                    m_processWrites.Start();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public void Dispose()
        {
            m_ctpStream?.Dispose();
            Ssl?.Dispose();
            NetStream?.Dispose();
            Socket?.Dispose();
            m_stream?.Dispose();
            m_processReads?.Dispose();
            m_processWrites?.Dispose();
        }
    }
}
