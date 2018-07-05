using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using GSF;
using GSF.Threading;

namespace CTP.Net
{
    public delegate void CommandReceivedEventHandler(CtpSession sender, CtpDocument command);

    public delegate void DataReceivedEventHandler(CtpSession sender, byte channel, byte[] data);

    public class CtpSession
    {
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
        private CommandHandler m_commandHandler;
        private ICtpDataChannelHandler[] m_dataChannelHandler;
        private CtpStream m_ctpStream;

        /// <summary>
        /// Gets how the remote endpoint is trusted.
        /// </summary>
        public readonly CertificateTrustMode TrustMode;

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

        public event CommandReceivedEventHandler CommandReceived;

        public event DataReceivedEventHandler DataReceived;

        public CtpSession(CtpStream stream, bool isServer, CertificateTrustMode trustMode, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            m_ctpStream = stream;
            IsServer = isServer;
            m_commandHandler = new CommandHandler();
            m_dataChannelHandler = new ICtpDataChannelHandler[32];
            TrustMode = trustMode;
            Socket = socket;
            NetStream = netStream;
            Ssl = ssl;
            m_stream = (Stream)ssl ?? netStream;
            m_processReads = new ScheduledTask();
            m_processReads.Running += M_processReads_Running;
            //m_processReads.Start();
        }

        private void M_processReads_Running(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            if (e.Argument == ScheduledTaskRunningReason.Disposing)
                return;

            LastReceiveTime = ShortTime.Now;
            while (m_ctpStream.Read())
            {
                if (m_ctpStream.Results.PayloadKind == 0)
                {
                    var cmd = new CtpDocument(m_ctpStream.Results.Payload);
                    if (!m_commandHandler.TryHandle(this, cmd))
                    {
                        CommandReceived?.Invoke(this, cmd);
                    }
                }
                else
                {
                    var dataHandler = m_dataChannelHandler[m_ctpStream.Results.PayloadKind];
                    if (dataHandler != null)
                        dataHandler.ProcessData(this, m_ctpStream.Results.Payload);
                    else
                        DataReceived?.Invoke(this, m_ctpStream.Results.PayloadKind, m_ctpStream.Results.Payload);
                }
            }
        }

        /// <summary>
        /// Begins listening to and processing the incoming data.
        /// </summary>
        public void Start()
        {
            m_ctpStream.DataReceived += Start;
            m_processReads.Start();
        }

        public void RegisterCommandChannelHandler(ICtpCommandHandler handler)
        {
            m_commandHandler.RegisterCommandHandler(handler);
        }
        public void UnRegisterCommandChannelHandler(ICtpCommandHandler handler)
        {
            m_commandHandler.UnRegisterCommandHandler(handler);
        }

        public void RegisterDataChannelHandler(byte channel, ICtpDataChannelHandler handler)
        {
            m_dataChannelHandler[channel] = handler;
        }

        public void SendData(byte channel, byte[] data)
        {
            m_ctpStream.Send(channel, data);
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(CtpDocument document)
        {
            m_ctpStream.Send(0, document.ToArray());
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(DocumentObject document)
        {
            m_ctpStream.Send(0, document.ToDocument().ToArray());
            LastSentTime = ShortTime.Now;
        }

        private void EncoderOnNewPacket(byte[] data, int position, int length)
        {
            m_stream.Write(data, position, length);
        }

    }
}
