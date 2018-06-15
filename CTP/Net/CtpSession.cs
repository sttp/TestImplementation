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
    public class ControlHandler
    {
        private Dictionary<string, CtpCommandHandlerBase> handlers = new Dictionary<string, CtpCommandHandlerBase>();

        public void RegisterCommandHandler(CtpCommandHandlerBase handler)
        {
            foreach (var command in handler.SupportedCommands)
            {
                handlers.Add(command, handler);
            }
        }

        public bool TryHandle(CtpSession session, CtpDocument command)
        {
            if (handlers.TryGetValue(command.RootElement, out var handler))
            {
                handler.ProcessCommand(session, command);
                return true;
            }
            return false;
        }

    }

    public class CommandRootHandler
    {
        private Dictionary<string, CtpCommandHandlerBase> m_rootHandlers = new Dictionary<string, CtpCommandHandlerBase>();

        public void RegisterCommandHandler(CtpCommandHandlerBase handler)
        {
            foreach (var command in handler.SupportedCommands)
            {
                m_rootHandlers.Add(command, handler);
            }
        }
        public CtpCommandHandlerBase ProcessCommand(CtpSession session, CtpDocument command)
        {
            return m_rootHandlers[command.RootElement].ProcessCommand(session, command);
        }
    }

    public class CtpSession
    {
        private readonly bool IsTrustedConnection;
        private readonly TcpClient Socket;
        private readonly NetworkStream NetStream;
        private readonly SslStream Ssl;
        public NegotiateStream Win;

        public readonly ServerTrustMode TrustMode;
        public readonly EncryptionMode Mode;
        public string LoginName = string.Empty;
        public HashSet<string> GrantedRoles = new HashSet<string>();

        public readonly string HostName;
        public readonly bool IsClient;
        public IPEndPoint RemoteEndpoint => Socket.Client.RemoteEndPoint as IPEndPoint;
        public X509Certificate RemoteCertificate => Ssl?.RemoteCertificate;
        public X509Certificate LocalCertificate => Ssl?.LocalCertificate;

        private CtpDecoder m_packetDecoder;
        private CtpEncoder m_encoder;
        private Stream m_stream;
        private bool m_isReading;
        private object m_syncReceive = new object();
        private byte[] m_inBuffer = new byte[3000];

        private WaitCallback m_asyncRead;
        private AsyncCallback m_asyncReadCallback;

        public ShortTime LastSentTime { get; private set; }
        public ShortTime LastReceiveTime { get; private set; }

        private ControlHandler ControlHandler;
        private CommandRootHandler RootHandler;
        private CtpCommandHandlerBase ActiveHandler;

        public CtpSession(bool isTrustedConnection, bool isClient, string hostName, EncryptionMode mode, ServerTrustMode trustMode, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            IsTrustedConnection = isTrustedConnection;
            IsClient = isClient;
            HostName = hostName;
            Mode = mode;
            TrustMode = trustMode;
            Socket = socket;
            NetStream = netStream;
            Ssl = ssl;
            m_packetDecoder = new CtpDecoder();
            m_encoder = new CtpEncoder();
            m_encoder.NewPacket += EncoderOnNewPacket;
            m_stream = (Stream)ssl ?? netStream;
            m_asyncRead = AsyncRead;
            m_asyncReadCallback = AsyncReadCallback;
        }

        public void Start()
        {
            AsyncRead(null);
        }

        private void OnNewInboundSession(CtpDocument command)
        {

        }

        public void RegisterControlHandler(CtpCommandHandlerBase handler)
        {
            ControlHandler.RegisterCommandHandler(handler);
        }

        public void RegisterCommandHandler(CtpCommandHandlerBase handler)
        {
            RootHandler.RegisterCommandHandler(handler);
        }

        public void SendCommand(byte[] payload)
        {
            m_encoder.Send(payload);
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(CtpDocument document)
        {
            m_encoder.Send(document.ToArray());
            LastSentTime = ShortTime.Now;
        }

        public void SendCommand(DocumentObject document)
        {
            m_encoder.Send(document.ToDocument().ToArray());
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
                if (!m_isReading)
                {
                    m_isReading = true;
                    m_stream.BeginRead(m_inBuffer, 0, m_inBuffer.Length, m_asyncReadCallback, null);
                }
            }
        }

        private void AsyncReadCallback(IAsyncResult ar)
        {
            lock (m_syncReceive)
            {
                m_isReading = false;
                int length = m_stream.EndRead(ar);
                m_packetDecoder.FillBuffer(m_inBuffer, 0, length);
            }
            LastReceiveTime = ShortTime.Now;
            while (m_packetDecoder.ReadCommand())
            {
                var cmd = new CtpDocument(m_packetDecoder.Results.Payload);
                if (!ControlHandler.TryHandle(this, cmd))
                {
                    if (ActiveHandler != null)
                    {
                        ActiveHandler = ActiveHandler.ProcessCommand(this, cmd);
                    }
                    else
                    {
                        ActiveHandler = RootHandler.ProcessCommand(this, cmd);
                    }
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
