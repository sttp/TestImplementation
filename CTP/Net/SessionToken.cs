using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Net
{
    public class SessionToken
    {
        public TcpClient Client;
        public NetworkStream NetworkStream;
        public IPEndPoint RemoteEndpoint;
        public SslStream SSL;
        public NegotiateStream Win;
        public string LoginName;
        public HashSet<string> GrantedRoles = new HashSet<string>();

        public Stream FinalStream => (Stream)SSL ?? NetworkStream;

        public SessionToken(TcpClient client)
        {
            Client = client;
            NetworkStream = client.GetStream();
            RemoteEndpoint = client.Client.RemoteEndPoint as IPEndPoint;

        }


    }
}
