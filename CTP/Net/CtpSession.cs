using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class CtpSession
    {
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
        private readonly CtpCommandStream CommandStream;

        public event Action<CtpReadResults> UnhandledCommands;

        public CtpSession(bool isClient, string hostName, EncryptionMode mode, ServerTrustMode trustMode, TcpClient socket, NetworkStream netStream, SslStream ssl)
        {
            IsClient = isClient;
            HostName = hostName;
            Mode = mode;
            TrustMode = trustMode;
            Socket = socket;
            NetStream = netStream;
            Ssl = ssl;
            CommandStream = new CtpCommandStream((Stream)Ssl ?? NetStream);
        }

        public CtpStream CreateStream()
        {
            throw new NotImplementedException();
            return new CtpStream();
        }
        public CtpStream OpenStream(int streamID)
        {
            throw new NotImplementedException();
            return new CtpStream();
        }

        public void SendDocument(CtpDocument document)
        {
            CommandStream.SendDocumentCommand(document);
        }

        public T ReadDocument<T>()
            where T : DocumentObject<T>
        {
            TryAgain:
            var item = CommandStream.Read();
            if (item.CommandCode != CommandCode.Document || item.DocumentPayload.RootElement != DocumentObject<T>.CommandName)
            {
                if (UnhandledCommands == null)
                {
                    throw new Exception("Unhandled command");
                }
                else
                {
                    UnhandledCommands?.Invoke(item);
                    goto TryAgain;
                }
            }
            return DocumentObject<T>.FromDocument(item.DocumentPayload);
        }

        public CtpDocument ReadDocument()
        {
            TryAgain:
            var item = CommandStream.Read();
            if (item.CommandCode != CommandCode.Document)
            {
                if (UnhandledCommands == null)
                {
                    throw new Exception("Unhandled command");
                }
                else
                {
                    UnhandledCommands?.Invoke(item);
                    goto TryAgain;
                }
            }
            return item.DocumentPayload;
        }




    }
}
