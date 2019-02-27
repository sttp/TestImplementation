using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTP;
using GSF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.Core;
using Sttp.DataPointEncoding;
using Sttp.Services;

namespace Sttp.Tests
{
    [TestClass]
    public class ArchiveFile
    {
        [TestMethod]
        public void TestFloat()
        {
            byte[] min = new byte[] { 0, 0, 0, 0 };
            byte[] max = new byte[] { 0, 255, 255, 255 };

            for (int x = 0; x < 256; x++)
            {
                min[0] = (byte)x;
                max[0] = (byte)x;

                double mind = Math.Abs(BigEndian.ToSingle(min, 0));
                double maxd = Math.Abs(BigEndian.ToSingle(max, 0));

                if (maxd < 0.00000001)
                    continue;
                if (mind > 1_000_000_000_000)
                    continue;

                Console.WriteLine($"{x}\t{BigEndian.ToSingle(min, 0)}\t{BigEndian.ToSingle(max, 0)}");
            }
        }

        [TestMethod]
        public void TestDouble()
        {
            byte[] min = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] max = new byte[] { 0, 255, 255, 255, 255, 255, 255, 255 };

            for (int x = 0; x < 256; x++)
            {
                min[0] = (byte)x;
                max[0] = (byte)x;

                double mind = Math.Abs(BigEndian.ToDouble(min, 0));
                double maxd = Math.Abs(BigEndian.ToDouble(max, 0));

                if (maxd < 0.00000001)
                    continue;
                if (mind > 1_000_000_000_000)
                    continue;

                Console.WriteLine($"{x}\t{BigEndian.ToDouble(min, 0)}\t{BigEndian.ToDouble(max, 0)}");
            }
        }

        [TestMethod]
        public void TestDateTime()
        {
            byte[] min = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] max = new byte[] { 0, 255, 255, 255, 0, 0, 0, 0 };

            for (int x = 0; x < 128; x++)
            {
                min[0] = (byte)x;
                max[0] = (byte)x;

                long mind = BigEndian.ToInt64(min, 0);
                long maxd = BigEndian.ToInt64(max, 0);

                if (maxd > CtpTime.MaxValue.Ticks)
                    continue;

                Console.WriteLine($"{x}\t{new CtpTime(mind)}\t{new CtpTime(maxd)}");
            }
        }

        [TestMethod]
        public void CompareFiles()
        {
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test1.sttp", CtpCompressionMode.None, EncodingMethod.Basic);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test2.sttp", CtpCompressionMode.Deflate, EncodingMethod.Basic);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test3.sttp", CtpCompressionMode.Zlib, EncodingMethod.Basic);

            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test4.sttp", CtpCompressionMode.None, EncodingMethod.Raw);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test5.sttp", CtpCompressionMode.Deflate, EncodingMethod.Raw);
            MakeFile(@"C:\temp\C37Test\test.sttp", @"C:\temp\C37Test\test6.sttp", CtpCompressionMode.Zlib, EncodingMethod.Raw);
        }

        private static void MakeFile(string source, string dest, CtpCompressionMode mode, EncodingMethod encoding)
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
            BenchmarkFile(@"C:\temp\C37Test\benchmark.sttp", @"C:\temp\C37Test\benchmark1.sttp", CtpCompressionMode.None, EncodingMethod.Raw);
            PointCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int x = 0; x < 0; x++)
            {
                BenchmarkFile(@"C:\temp\C37Test\benchmark1.sttp", @"C:\temp\C37Test\benchmark2.sttp", CtpCompressionMode.None, EncodingMethod.Raw);
            }

            Console.WriteLine(PointCount);
            Console.WriteLine(PointCount / sw.Elapsed.TotalSeconds);
        }

        [TestMethod]
        public void Profile()
        {
            PointCount = 0;
            BenchmarkFile(@"C:\temp\C37Test\benchmark1.sttp", @"C:\temp\C37Test\benchmark2.sttp", CtpCompressionMode.None, EncodingMethod.Raw);
            Console.WriteLine($"None: " + new FileInfo(@"C:\temp\C37Test\benchmark2.sttp").Length / 1024);
            Console.WriteLine(new FileInfo(@"C:\temp\C37Test\benchmark2.sttp").Length / (float)PointCount);
            BenchmarkFile(@"C:\temp\C37Test\benchmark2.sttp", @"C:\temp\C37Test\benchmark3.sttp", CtpCompressionMode.None, EncodingMethod.Raw);

            using (var sha = new SHA1Managed())
            {
                Console.WriteLine(GuidExtensions.ToRfcGuid(sha.ComputeHash(File.ReadAllBytes(@"C:\temp\C37Test\benchmark1.sttp")),0));
                Console.WriteLine(GuidExtensions.ToRfcGuid(sha.ComputeHash(File.ReadAllBytes(@"C:\temp\C37Test\benchmark2.sttp")),0));
            }

            return;

            string[] file1 = File.ReadAllLines(@"C:\temp\C37Test\benchmark1.txt");
            string[] file2 = File.ReadAllLines(@"C:\temp\C37Test\benchmark2.txt");

            int cnt = 0;
            int l = Math.Max(file1.Length, file2.Length);
            //int l = Math.Min(file1.Length, file2.Length);
            for (int x = 0; x < l; x++)
            {
                if (x >= file1.Length)
                {
                    Console.WriteLine(x);
                    Console.WriteLine(file2[x]);
                    cnt++;
                }
                else if (x >= file2.Length)
                {
                    Console.WriteLine(x);
                    Console.WriteLine(file1[x]);
                    cnt++;
                }
                else if (file1[x] != file2[x])
                {
                    Console.WriteLine(x);
                    Console.WriteLine(file1[x]);
                    Console.WriteLine(file2[x]);
                    cnt++;
                }

                if (cnt == 10)
                    return;
            }

            //Console.WriteLine(Names.Average(x => x.Length));
            //BenchmarkFile(@"C:\temp\C37Test\benchmark1.sttp", @"C:\temp\C37Test\benchmark2.sttp", SttpCompressionMode.Deflate, EncodingMethod.Adaptive);
            //Console.WriteLine($"None: " + new FileInfo(@"C:\temp\C37Test\benchmark2.sttp").Length / 1024);
            //BenchmarkFile(@"C:\temp\C37Test\benchmark1.sttp", @"C:\temp\C37Test\benchmark2.sttp", SttpCompressionMode.Zlib, EncodingMethod.Adaptive);
            //Console.WriteLine($"None: " + new FileInfo(@"C:\temp\C37Test\benchmark2.sttp").Length / 1024);
        }

        private HashSet<string> Names = new HashSet<string>();

        private void BenchmarkFile(string source, string dest, CtpCompressionMode mode, EncodingMethod encoding)
        {
            string newFileName = Path.ChangeExtension(source, ".txt");

            using (var raw = new StreamWriter(newFileName, false))
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
                            //ctp2.ProducerMetadata(md);
                            break;
                        case FileReaderItem.DataPoint:
                            var dp = new SttpDataPoint();
                            while (ctp.ReadDataPoint(dp))
                            {
                                //if (dp.Value.AsSingle > 3000000)
                                //{
                                //    dp.Value = dp.Value;
                                //}
                                //if (dp.Metadata.DataPointID.AsString.EndsWith(":DFreq0"))
                                //{
                                //    dp.Value = new CtpNumeric((long)(dp.Value.AsSingle * 100), 2);
                                //}
                                //Names.Add(dp.Metadata.DataPointID.AsString);
                                raw.WriteLine(dp.ToString());
                                PointCount++;
                                //dp.Value = (double)dp.Value;
                                //dp.Value = (long)dp.Value*1000;
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
