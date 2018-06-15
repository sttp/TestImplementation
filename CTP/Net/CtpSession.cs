using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
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
        public readonly CtpCommandStream CommandStream;

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
            CommandStream = new CtpCommandStream((Stream)Ssl ?? NetStream, OnNewInboundSession);
        }

        private void OnNewInboundSession(CtpDocument command)
        {
            if (ActiveHandler != null)
            {
                ActiveHandler = ActiveHandler.ProcessCommand(this, command);
            }
            else
            {
                ActiveHandler = RootHandler.ProcessCommand(this, command);
            }
        }

        public void RegisterCommandHandler(CtpCommandHandlerBase handler)
        {
            RootHandler.RegisterCommandHandler(handler);
        }

        public void SendCommand(CtpDocument document)
        {
            CommandStream.SendCommand(document.ToArray());
        }

    }
}
