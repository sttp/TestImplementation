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

namespace Sttp.Tests
{
    [TestClass]
    public class ArchiveFile
    {
        [TestMethod]
        public void CompareFiles()
        {
            using (var fs = new FileStream(@"C:\temp\C37Test\test.sttp", FileMode.Open))
            using (var fs2 = new FileStream(@"C:\temp\C37Test\test2.sttp", FileMode.Create))
            using (var ctp = new SttpFileReader(fs, false))
            using (var ctp2 = new SttpFileWriter(fs2, false, SttpCompressionMode.None))
            {
                MoveData(ctp, ctp2);
            }

            using (var fs = new FileStream(@"C:\temp\C37Test\test.sttp", FileMode.Open))
            using (var fs2 = new FileStream(@"C:\temp\C37Test\test3.sttp", FileMode.Create))
            using (var ctp = new SttpFileReader(fs, false))
            using (var ctp2 = new SttpFileWriter(fs2, false, SttpCompressionMode.Deflate))
            {
                MoveData(ctp, ctp2);
            }

            using (var fs = new FileStream(@"C:\temp\C37Test\test.sttp", FileMode.Open))
            using (var fs2 = new FileStream(@"C:\temp\C37Test\test4.sttp", FileMode.Create))
            using (var ctp = new SttpFileReader(fs, false))
            using (var ctp2 = new SttpFileWriter(fs2, false, SttpCompressionMode.Zlib))
            {
                MoveData(ctp, ctp2);
            }

        }

        private static void MoveData(SttpFileReader ctp, SttpFileWriter ctp2)
        {
            while (true)
            {
                switch (ctp.Next())
                {
                    case FileReaderItem.ProducerMetadata:
                        var md = ctp.GetMetadata();
                        ctp2.ProducerMetadata(md);
                        break;
                    case FileReaderItem.DataPoint:
                        var dp = new SttpDataPoint();
                        while (ctp.ReadDataPoint(dp))
                        {
                            ctp2.AddDataPoint(dp);
                        }
                        break;
                    case FileReaderItem.EndOfStream:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
