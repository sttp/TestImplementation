//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using CTP;
//using GSF;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Sttp;

//namespace Sttp.Tests.Object_Serialzation
//{
//    [TestClass]
//    public class BitReadWrite
//    {
//        [TestMethod]
//        public void TestValues()
//        {
//            Test(0);
//            for (int x = 10; x < 100000; x = (int)(x * 1.2))
//            {
//                Test(x);
//            }
//        }

//        private void Test(int count)
//        {
//            var r = new Random(count);
//            var wr = new BitStreamWriter();
//            for (var x = 0; x < count; x++)
//            {
//                CreateRandom(r, out var bits, out var value);
//                wr.WriteBits(bits, value);
//            }

//            r = new Random(count);
//            var rd = new BitStreamReader(wr.ToArray());
//            for (var x = 0; x < count; x++)
//            {
//                CreateRandom(r, out var bits, out var value);
//                var oo = rd.ReadBits(bits);
//                if (value != oo)
//                {
//                    throw new Exception($"Not Equal {value} {oo} on record {x}");
//                }
//            }
//            if (!rd.IsEmpty)
//                throw new Exception();
//        }

//        private void CreateRandom(Random r, out int bits, out ulong value)
//        {
//            byte[] buffer = new byte[8];
//            r.NextBytes(buffer);
//            bits = r.Next(65);
//            if (bits == 0)
//            {
//                value = 0;
//            }
//            else
//            {
//                value = BigEndian.ToUInt64(buffer, 0) >> (64 - bits);
//            }
//        }

//    }
//}
