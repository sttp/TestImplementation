using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp;

namespace Sttp.Tests.WireProtocol
{
    [TestClass]
    public class Encoding
    {
        [TestMethod]
        public void Test()
        {

            var wr = new ByteWriter();
            var rd = new ByteReader();
            for (int x = -100; x < 100; x++)
            {
                SttpValueEncodingNative.Save(wr, (SttpValue)x.ToString());
                SttpValueEncodingNative.Save(wr, (SttpValue)x);
                SttpValueEncodingNative.Save(wr, (SttpValue)(float)x);
                SttpValueEncodingNative.Save(wr, (SttpValue)(double)x);
                SttpValueEncodingNative.Save(wr, (SttpValue)DateTime.Parse("1/1/2010").AddMinutes(x));
                SttpValueEncodingNative.Save(wr, (SttpValue)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                SttpValueEncodingNative.Save(wr, (SttpValue)((x & 1) == 1));
                SttpValueEncodingNative.Save(wr, (SttpValue)(((x & 1) == 1) ? (bool?)null : (bool?)true));
                SttpValueEncodingNative.Save(wr, (SttpValue)GetBytes(x));
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsString, x.ToString());
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsInt32, x);
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsSingle, (float)x);
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsDouble, (double)x);
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsBoolean, ((x & 1) == 1));
                Assert.AreEqual((bool?)SttpValueEncodingNative.Load(rd), ((x & 1) == 1) ? (bool?)null : (bool?)true);
                Assert.IsTrue(GetBytes(x).SequenceEqual((byte[])SttpValueEncodingNative.Load(rd)));
            }
        }

        [TestMethod]
        public void Test2()
        {
            var wr = new ByteWriter();
            var rd = new ByteReader();
            for (int x = -100; x < 100; x++)
            {
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)x.ToString());
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)x);
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)(float)x);
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)(double)x);
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)DateTime.Parse("1/1/2010").AddMinutes(x));
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)((x & 1) == 1));
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)(((x & 1) == 1) ? (bool?)null : (bool?)true));
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)GetBytes(x));
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.String).AsString, x.ToString());
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Int64).AsInt32, x);
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Single).AsSingle, (float)x);
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Double).AsDouble, (double)x);
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.SttpTime).AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Guid).AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Boolean).AsBoolean, ((x & 1) == 1));
                Assert.AreEqual((bool?)SttpValueEncodingWithoutType.Load(rd, ((x & 1) == 1) ? SttpValueTypeCode.Null : SttpValueTypeCode.Boolean), ((x & 1) == 1) ? (bool?)null : (bool?)true);
                Assert.IsTrue(GetBytes(x).SequenceEqual((byte[])SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.SttpBuffer)));
            }
        }


        public static byte[] GetBytes(int value)
        {
            return new[]
                   {
                       (byte)(value >> 24),
                       (byte)(value >> 16),
                       (byte)(value >> 8),
                       (byte)(value)
                   };
        }

    }
}
