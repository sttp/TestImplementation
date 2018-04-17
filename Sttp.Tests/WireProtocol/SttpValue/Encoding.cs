using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
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
                CtpValueEncodingNative.Save(wr, (CtpObject)x.ToString());
                CtpValueEncodingNative.Save(wr, (CtpObject)x);
                CtpValueEncodingNative.Save(wr, (CtpObject)(float)x);
                CtpValueEncodingNative.Save(wr, (CtpObject)(double)x);
                CtpValueEncodingNative.Save(wr, (CtpObject)DateTime.Parse("1/1/2010").AddMinutes(x));
                CtpValueEncodingNative.Save(wr, (CtpObject)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                CtpValueEncodingNative.Save(wr, (CtpObject)((x & 1) == 1));
                CtpValueEncodingNative.Save(wr, (CtpObject)(((x & 1) == 1) ? (bool?)null : (bool?)true));
                CtpValueEncodingNative.Save(wr, (CtpObject)GetBytes(x));
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsString, x.ToString());
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsInt32, x);
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsSingle, (float)x);
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsDouble, (double)x);
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                Assert.AreEqual(CtpValueEncodingNative.Load(rd).AsBoolean, ((x & 1) == 1));
                Assert.AreEqual((bool?)CtpValueEncodingNative.Load(rd), ((x & 1) == 1) ? (bool?)null : (bool?)true);
                Assert.IsTrue(GetBytes(x).SequenceEqual((byte[])CtpValueEncodingNative.Load(rd)));
            }
        }

        [TestMethod]
        public void Test2()
        {
            var wr = new ByteWriter();
            var rd = new ByteReader();
            for (int x = -100; x < 100; x++)
            {
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)x.ToString());
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)x);
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)(float)x);
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)(double)x);
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)DateTime.Parse("1/1/2010").AddMinutes(x));
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)((x & 1) == 1));
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)(((x & 1) == 1) ? (bool?)null : (bool?)true));
                CtpValueEncodingWithoutType.Save(wr, (CtpObject)GetBytes(x));
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.String).AsString, x.ToString());
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.Int64).AsInt32, x);
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.Single).AsSingle, (float)x);
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.Double).AsDouble, (double)x);
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.CtpTime).AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.Guid).AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                Assert.AreEqual(CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.Boolean).AsBoolean, ((x & 1) == 1));
                Assert.AreEqual((bool?)CtpValueEncodingWithoutType.Load(rd, ((x & 1) == 1) ? CtpTypeCode.Null : CtpTypeCode.Boolean), ((x & 1) == 1) ? (bool?)null : (bool?)true);
                Assert.IsTrue(GetBytes(x).SequenceEqual((byte[])CtpValueEncodingWithoutType.Load(rd, CtpTypeCode.CtpBuffer)));
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
