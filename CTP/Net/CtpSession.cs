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

        private Dictionary<string, ICtpCommandHandler> m_rootHandlers = new Dictionary<string, ICtpCommandHandler>();
        private Dictionary<ulong, CtpStream> m_streamHandlers = new Dictionary<ulong, CtpStream>();

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

        public void RegisterHandler(ICtpCommandHandler handler)
        {
            foreach (var command in handler.CommandsHandled())
            {
                m_rootHandlers.Add(command, handler);
            }
        }

        /// <summary>
        /// Automatically handle all commands on a callback thread. 
        /// In general, this should only be called on the server component. When this occurs, all reads
        /// will be occurring on it's own thread on a callback. 
        /// </summary>
        public void DoEvents()
        {

        }

        public CtpWriteStream CreateWriteStream()
        {
            return new CtpWriteStream(CommandStream.GetNextRawChannelID(), this, Write);
        }

        private void Write(ulong streamID, byte[] buffer, int position, int length)
        {
            CommandStream.SendRaw(streamID, buffer, position, length);
        }

        public CtpReadStream OpenReadStream(ulong streamID)
        {
            return new CtpReadStream(streamID, this);
        }

        public CtpStream CreateStream()
        {

            throw new NotImplementedException();
            return new CtpStream(null, null);
        }
        public CtpStream OpenStream(int streamID)
        {
            throw new NotImplementedException();
            return new CtpStream(null, null);
        }

        public void SendDocument(CtpDocument document)
        {
            CommandStream.SendDocumentCommand(0, document);
        }

        public T ReadDocument<T>()
            where T : DocumentObject<T>
        {
            var document = ReadDocument();
            if (document.RootElement != DocumentObject<T>.CommandName)
            {
                throw new Exception("Unhandled command");
            }
            return DocumentObject<T>.FromDocument(document);
        }

        public CtpDocument ReadDocument()
        {
            TryAgain:
            var item = CommandStream.Read();
            if (item.CommandCode == CommandCode.Binary)
            {
                if (m_streamHandlers.TryGetValue(item.BinaryChannelID, out var stream))
                {
                    stream.Write(item.BinaryPayload);
                }
                goto TryAgain;
            }
            else
            {
                return item.DocumentPayload;
            }
        }

        public void StopWriteStream(ulong channelNumber)
        {
            throw new NotImplementedException();
        }

        public void StopReadStream(ulong channelNumber)
        {
            throw new NotImplementedException();
        }
    }
}
