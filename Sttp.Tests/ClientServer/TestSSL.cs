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
        [TestMethod]
        public void TestIPWithoutEncryption()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates[0];
            store.Close();

            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            listener.Permissions.AddIPUser(IPAddress.Loopback, 32, "Myself", "CanRead", "CanWrite");
            listener.Permissions.AssignEncriptionOptions(IPAddress.Any, 0, cert);
            listener.Permissions.AssignEncriptionOptions(IPAddress.Loopback, 32, null);
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            client.TurnOffSSL();
            client.Connect();
            Thread.Sleep(100);
        }

        [TestMethod]
        public void TestIPWithEncryption()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates[0];
            store.Close();

            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            listener.Permissions.AddIPUser(IPAddress.Loopback, 32, "Myself", "CanRead", "CanWrite");
            listener.Permissions.AssignEncriptionOptions(IPAddress.Any, 0, cert);
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
            var cert = store.Certificates[0];
            var cert2 = store.Certificates[0];
            store.Close();

            var listener = new CtpListener(new IPEndPoint(IPAddress.Loopback, 29348));
            listener.Permissions.AddSelfSignedCertificateUser(cert2, "Cert", "Perm1");
            listener.Permissions.AssignEncriptionOptions(IPAddress.Any, 0, cert);
            listener.SessionCompleted += Listener_SessionCompleted;
            listener.Start();

            var client = new CtpClient();
            client.SetHost(IPAddress.Loopback, 29348);
            client.SetUserCredentials(cert2);
            client.Connect();
            Thread.Sleep(100);
        }

        private void Listener_SessionCompleted(SessionToken token)
        {
            Console.WriteLine(token.Client.Client.RemoteEndPoint.ToString());
            Console.WriteLine(token.LoginName);
            Console.WriteLine(string.Join(",", token.GrantedRoles));

        }
    }
}
