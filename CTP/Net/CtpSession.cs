using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using GSF;

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
        private CtpDecoder m_decoder;
        private CtpEncoder m_encoder;
        private Stream m_stream;
        private bool m_isReadingFromStream;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];
        private WaitCallback m_asyncRead;
        private AsyncCallback m_asyncReadCallback;
        private CommandHandler m_commandHandler;
        private ICtpDataChannelHandler[] m_dataChannelHandler;

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

        public event CommandReceivedEventHandler CommandReceived;

        public event DataReceivedEventHandler DataReceived;

        public CtpSession(bool isServer, CertificateTrustMode trustMode, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            IsServer = isServer;
            m_commandHandler = new CommandHandler();
            m_dataChannelHandler = new ICtpDataChannelHandler[32];
            TrustMode = trustMode;
            Socket = socket;
            NetStream = netStream;
            Ssl = ssl;
            m_decoder = new CtpDecoder();
            m_encoder = new CtpEncoder();
            m_encoder.NewPacket += EncoderOnNewPacket;
            m_stream = (Stream)ssl ?? netStream;
            m_asyncRead = AsyncRead;
            m_asyncReadCallback = AsyncReadCallback;
        }

        /// <summary>
        /// Begins listening to and processing the incoming data.
        /// </summary>
        public void Start()
        {
            AsyncRead(null);
        }

        public void RegisterCommandChannelHandler(ICtpCommandHandler handler)
        {
            m_commandHandler.RegisterCommandHandler(handler);
        }

        public void RegisterDataChannelHandler(byte channel, ICtpDataChannelHandler handler)
        {
            m_dataChannelHandler[channel] = handler;
        }

        public void SendData(byte channel, byte[] data)
        {
            m_encoder.Send(channel, data);
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(CtpDocument document)
        {
            m_encoder.Send(0, document.ToArray());
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(DocumentObject document)
        {
            m_encoder.Send(0, document.ToDocument().ToArray());
            LastSentTime = ShortTime.Now;
        }

        private void EncoderOnNewPacket(byte[] data, int position, int length)
        {
            m_stream.Write(data, position, length);
        }

        private void AsyncRead(object obj)
        {
            lock (m_syncReceive)
            {
                if (!m_isReadingFromStream)
                {
                    m_isReadingFromStream = true;
                    m_stream.BeginRead(m_inBuffer, 0, m_inBuffer.Length, m_asyncReadCallback, null);
                }
            }
        }

        private void AsyncReadCallback(IAsyncResult ar)
        {
            lock (m_syncReceive)
            {
                m_isReadingFromStream = false;
                int length = m_stream.EndRead(ar);
                m_decoder.FillBuffer(m_inBuffer, 0, length);
            }
            LastReceiveTime = ShortTime.Now;
            while (m_decoder.ReadCommand())
            {
                if (m_decoder.Results.PayloadKind == 0)
                {
                    var cmd = new CtpDocument(m_decoder.Results.Payload);
                    if (!m_commandHandler.TryHandle(this, cmd))
                    {
                        CommandReceived?.Invoke(this, cmd);
                    }
                }
                else
                {
                    var dataHandler = m_dataChannelHandler[m_decoder.Results.PayloadKind];
                    if (dataHandler != null)
                        dataHandler.ProcessData(this, m_decoder.Results.Payload);
                    else
                        DataReceived?.Invoke(this, m_decoder.Results.PayloadKind, m_decoder.Results.Payload);
                }
            }

            if (ar.CompletedSynchronously)
            {
                ThreadPool.QueueUserWorkItem(m_asyncRead);
            }
            else
            {
                AsyncRead(null);
            }
        }

    }
}
