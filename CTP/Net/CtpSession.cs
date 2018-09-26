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

    public class CtpSession
    {
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

        //public event PacketReceivedEventHandler PacketReceived;

        public CtpSession(CtpStream stream, bool isServer, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
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
                m_readQueue.Enqueue(packet);
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

        /// <summary>
        /// Begins listening to and processing the incoming data.
        /// </summary>
        public void Start()
        {
            m_processReads.Start();
        }

        public bool TryRead(CtpPacket packet)
        {
            return m_readQueue.TryDequeue(out packet);
        }

        public void SendRaw(byte[] data, byte channel)
        {
            m_writeQueue.Enqueue(new CtpPacket(channel, true, data));
            m_processWrites.Start();
        }

        public void Send(DocumentObject document, byte channel = 0)
        {
            m_writeQueue.Enqueue(new CtpPacket(channel, document));
            m_processWrites.Start();
            LastSentTime = ShortTime.Now;
        }




    }
}
