using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [TestMethod]
        public void TestIPWithoutEncryption()
        {
            var listener = new CtpServer(new IPEndPoint(IPAddress.Loopback, 29348), false);
            listener.Authentication.AddIPUser(IPAddress.Loopback, 32, "Myself", "CanRead", "CanWrite");
            listener.SetIPSpecificOptions(IPAddress.Loopback, 32);
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient(false);
            client.SetHost(IPAddress.Loopback, 29348);
            client.RequireTrustedServers = false;
            client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestIPWithEncryption()
        {
            var listener = new CtpServer(new IPEndPoint(IPAddress.Loopback, 29348), true);
            listener.Authentication.AddIPUser(IPAddress.Loopback, 32, "Myself", "CanRead", "CanWrite");
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient(true);
            client.RequireTrustedServers = false;
            client.SetHost(IPAddress.Loopback, 29348);
            client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestSrpUser()
        {
            var listener = new CtpServer(new IPEndPoint(IPAddress.Loopback, 29349), true);
            listener.Authentication.AddCredential(new SrpCredential("U", "Pass1", SrpStrength.Bits1024, "User", new string[] { "Role1" }));
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient(true);
            client.RequireTrustedServers = false;
            client.SetHost(IPAddress.Loopback, 29349);
            client.SetCredentials(new NetworkCredential("U", "Pass1"));
            var s = client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestSrp2User()
        {
            var listener = new CtpServer(new IPEndPoint(IPAddress.Loopback, 29348), true);
            listener.Authentication.AddCredential(new SrpCredential("U", "Pass1", SrpStrength.Bits1024, "User", new string[] { "Role1" }));
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient(true);
            client.RequireTrustedServers = false;
            client.SetHost(IPAddress.Loopback, 29348);
            client.SetCredentials(new NetworkCredential("U", "Pass1"));
            var s = client.Connect();
            Thread.Sleep(100);
        }

        private void Listener_SessionCompleted(CtpSession token)
        {
            token.Start();
            Console.WriteLine(token.RemoteEndpoint.ToString());
            Console.WriteLine(token.LoginName);
            Console.WriteLine(string.Join(",", token.GrantedRoles));
        }
    }
}
