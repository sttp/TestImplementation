using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Data;

namespace Sttp.Tests
{
    [TestClass]
    public class MetadataDatabaseSourceTest
    {
        [TestMethod]
        public void WriteToXML()
        {
            DataSet ds = new DataSet("openPDC");

            using (var fs = new FileStream("c:\\temp\\openPDC-sttp.xml", FileMode.Open))
            {
                ds.ReadXml(fs);
            }

            var db = new MetadataDatabaseSource(ds);



        }

    }
}
