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
using CTP.SRP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sttp.Tests.ClientServer
{
    [TestClass]
    public class TestSSL
    {
        ServerSessionAuthentication user = new ServerSessionAuthentication();

        [TestMethod]
        public void TestIPWithoutEncryption()
        {
            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            user.AddIPUser(IPAddress.Loopback, 32, "Myself", "CanRead", "CanWrite");
            listener.SetIPSpecificOptions(IPAddress.Loopback, 32, true, false);
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            client.RequireSSL = false;
            client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestIPWithEncryption()
        {
            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            user.AddIPUser(IPAddress.Loopback, 32, "Myself", "CanRead", "CanWrite");
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestCertificateUser()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var cert2 = store.Certificates[0];
            store.Close();

            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            user.AddSelfSignedCertificateUser(cert2, "Cert", "Perm1");
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            client.SetClientCertificate(cert2);
            client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestSrpUser()
        {
            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            user.AddSrpUser("U", "Pass1", "User", "Role1");
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            var s = client.Connect();
            ClientSessionAuthentication.AuthenticateWithSRP(s, new NetworkCredential("U", "Pass1"));
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestSrp2User()
        {
            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            user.SetSrpDefaults(null, SrpStrength.Bits2048);
            user.AddSrpUser("U", "Pass1", "User", "Role1");
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            var s = client.Connect();
            ClientSessionAuthentication.AuthenticateWithSRP(s, new NetworkCredential("U", "Pass1"));
            Thread.Sleep(100);
        }

        private void Listener_SessionCompleted(CtpSession token)
        {
            user.AuthenticateSession(token);
            Console.WriteLine(token.RemoteEndpoint.ToString());
            Console.WriteLine(token.LoginName);
            Console.WriteLine(string.Join(",", token.GrantedRoles));
        }
    }
}
