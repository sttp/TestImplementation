using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTP.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.ClientServer
{
    [TestClass]
    public class TestSSL
    {
        private NegotiateClient NegotS;
        private NegotiateClient NegotC;
        private ConnectionOptions ServerOptions;
        private ConnectionOptions ClientOptions;

        [TestMethod]
        public void TestMethod()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates[0];
            store.Close();

            ServerOptions = new ConnectionOptions();
            ServerOptions.ServerCertificate = cert;
            ServerOptions.SslCheckCertificateRevocation = false;
            ServerOptions.Encryption = EncryptionMode.SSL;
            ServerOptions.EncryptAsServer = true;
            ServerOptions.ServerAuthentication = ServerAuthenticationMode.TrustCA;

            NegotS = new NegotiateClient();
            NegotC = new NegotiateClient();

            var listener = new Listener(new IPEndPoint(IPAddress.Loopback, 29482));
            listener.NewClient += ListenerOnNewClient;
            listener.AssignOptions(IPAddress.Any, 0, ServerOptions);
            listener.Start();

            var client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Loopback, 29482));

            ClientOptions = new ConnectionOptions();
            ClientOptions.ServerName = "oge\\chishose";
            ClientOptions.TrustedCertificates = new X509CertificateCollection(new X509Certificate[] { cert });
            ClientOptions.SslCheckCertificateRevocation = false;
            ClientOptions.Encryption = EncryptionMode.SSL;
            ClientOptions.EncryptAsServer = false;
            ClientOptions.ServerAuthentication = ServerAuthenticationMode.TrustCA;

            var c = new ClientToken(client, ClientOptions);
            NegotC.ProcessClient(c);
            Thread.Sleep(1000);
        }

        private void ListenerOnNewClient(TcpClient client, ConnectionOptions options)
        {
            var c = new ClientToken(client, options);
            NegotS.ProcessClient(c);
        }
    }
}
