using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test1.sttp", SttpCompressionMode.None, EncodingMethod.Basic);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test2.sttp", SttpCompressionMode.Deflate, EncodingMethod.Basic);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test3.sttp", SttpCompressionMode.Zlib, EncodingMethod.Basic);

            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test4.sttp", SttpCompressionMode.None, EncodingMethod.Raw);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test5.sttp", SttpCompressionMode.Deflate, EncodingMethod.Raw);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test6.sttp", SttpCompressionMode.Zlib, EncodingMethod.Raw);
        }

        private static void MakeFile(string source, string dest, SttpCompressionMode mode, EncodingMethod encoding)
        {
            using (var fs = new FileStream(source, FileMode.Open))
            using (var fs2 = new FileStream(dest, FileMode.Create))
            using (var ctp = new SttpFileReader(fs, false))
            using (var ctp2 = new SttpFileWriter(fs2, false, mode, encoding))
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

        public long PointCount;

        [TestMethod]
        public void BenchmarkFiles()
        {
            //BenchmarkFile(@"C:\temp\C37Test\benchmark.sttp", @"C:\temp\C37Test\benchmark1.sttp", SttpCompressionMode.None, EncodingMethod.Basic);
            PointCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int x = 0; x < 5; x++)
            {
                BenchmarkFile(@"C:\temp\C37Test\benchmark1.sttp", @"C:\temp\C37Test\benchmark2.sttp", SttpCompressionMode.None, EncodingMethod.Basic);
            }

            Console.WriteLine(PointCount);
            Console.WriteLine(PointCount / sw.Elapsed.TotalSeconds);

        }

        [TestMethod]
        public void Profile()
        {
            BenchmarkFile(@"C:\temp\C37Test\benchmark1.sttp", @"C:\temp\C37Test\benchmark2.sttp", SttpCompressionMode.None, EncodingMethod.Adaptive);
        }

        private void BenchmarkFile(string source, string dest, SttpCompressionMode mode, EncodingMethod encoding)
        {
            using (var fs = new FileStream(source, FileMode.Open))
            using (var fs2 = new FileStream(dest, FileMode.Create))
            using (var ctp = new SttpFileReader(fs, false))
            using (var ctp2 = new SttpFileWriter(fs2, false, mode, encoding))
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
                                PointCount++;
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
}
