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
        public void TestMethod()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates[0];
            store.Close();

          
        }
    }
}
