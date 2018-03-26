using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Core;
using Sttp.Services;

namespace Sttp.Tests.ClientServer
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestMethod()
        {
            var listener = new TcpListener(IPAddress.Loopback, 29482);
            listener.Start();

            var client = new TcpClient();
            var t1 = client.ConnectAsync(IPAddress.Loopback, 29482);

            var server = listener.AcceptTcpClient();
            t1.Wait();

            var server1 = new SttpServer(server.GetStream());
            DataSet ds = new DataSet("openPDC");

            using (var fs = new FileStream("c:\\temp\\openPDC-sttp.xml", FileMode.Open))
            {
                ds.ReadXml(fs);
            }
            server1.MetadataServer.DefineSchema(ds);
            server1.MetadataServer.FillData(ds);
            server1.MetadataServer.CommitData();

            server1.Start();

            var client1 = new SttpClient(client.GetStream());

            var tables = client1.GetMetaDataTableList();
            Console.WriteLine(tables.Count);
            foreach (var table in tables)
            {
                Console.WriteLine(table);
            }

            foreach (var table in client1.GetMetaDataFieldList("Device"))
            {
                Console.WriteLine(table);
            }

        }

    }
}
