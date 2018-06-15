using GSF.IO;
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

        private Dictionary<string, ICtpCommandHandler> m_rootHandlers = new Dictionary<string, ICtpCommandHandler>();

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
            CommandStream = new CtpCommandStream((Stream)Ssl ?? NetStream, isClient, OnNewInboundSession);
        }

        private void OnNewInboundSession(CtpRequestHandler ctpRequestHandler)
        {
            throw new NotImplementedException();
        }

        public void RegisterHandler(ICtpCommandHandler handler)
        {
            foreach (var command in handler.CommandsHandled())
            {
                m_rootHandlers.Add(command, handler);
            }
        }

        //public void SendDocument(CtpDocument document)
        //{
        //    CommandStream.SendDocumentCommand(0, document);
        //}

        //public T ReadDocument<T>()
        //    where T : DocumentObject<T>
        //{
        //    var document = ReadDocument();
        //    if (document.RootElement != DocumentObject<T>.CommandName)
        //    {
        //        throw new Exception("Unhandled command");
        //    }
        //    return DocumentObject<T>.FromDocument(document);
        //}

        //public CtpDocument ReadDocument()
        //{
        //    TryAgain:
        //    var item = CommandStream.Read();
        //    if (false)
        //    {
        //        if (m_streamHandlers.TryGetValue(item.ChannelNumber, out var stream))
        //        {
        //            stream.Write(item.Payload);
        //        }
        //        goto TryAgain;
        //    }
        //    else
        //    {
        //        return item.DocumentPayload;
        //    }
        //}

        //public void StopWriteStream(ulong channelNumber)
        //{
        //    throw new NotImplementedException();
        //}

        //public void StopReadStream(ulong channelNumber)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
