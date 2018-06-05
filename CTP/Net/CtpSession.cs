using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace CTP.Net
{
    public class CtpSession
    {
        public ServerTrustMode TrustMode;
        public TcpClient Socket;
        public NetworkStream NetStream;
        public SslStream Ssl;
        public EncryptionMode Mode;
        public IPEndPoint RemoteEndpoint => Socket.Client.RemoteEndPoint as IPEndPoint;
        public string LoginName = string.Empty;
        public HashSet<string> GrantedRoles = new HashSet<string>();
        public NegotiateStream Win;
        public Stream FinalStream => (Stream)Ssl ?? NetStream;
        public CtpCommandStream CommandStream;
        public string HostName;

        public CtpSession(EncryptionMode mode, ServerTrustMode trustMode, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            Mode = mode;
            TrustMode = trustMode;
            Socket = socket;
            NetStream = netStream;
            Ssl = ssl;
            CommandStream = new CtpCommandStream(FinalStream);
        }


    }
}