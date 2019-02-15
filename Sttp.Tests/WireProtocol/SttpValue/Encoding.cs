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

            var wr = new BitWriter();
            var rd = new BitReader();
            for (int x = -100; x < 100; x++)
            {
                wr.WriteObject((CtpObject)x.ToString());
                wr.WriteObject((CtpObject)x);
                wr.WriteObject((CtpObject)(float)x);
                wr.WriteObject((CtpObject)(double)x);
                wr.WriteObject((CtpObject)(CtpNumeric)(decimal)x);
                wr.WriteObject((CtpObject)DateTime.Parse("1/1/2010").AddMinutes(x));
                wr.WriteObject((CtpObject)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                wr.WriteObject((CtpObject)((x & 1) == 1));
                wr.WriteObject((CtpObject)(((x & 1) == 1) ? (bool?)null : (bool?)true));
                wr.WriteObject((CtpObject)GetBytes(x));
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(rd.ReadObject().AsString, x.ToString());
                Assert.AreEqual(rd.ReadObject().AsInt32, x);
                Assert.AreEqual(rd.ReadObject().AsSingle, (float)x);
                Assert.AreEqual(rd.ReadObject().AsDouble, (double)x);
                Assert.AreEqual(rd.ReadObject().AsNumeric, (CtpNumeric)(decimal)x);
                Assert.AreEqual(rd.ReadObject().AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
                Assert.AreEqual(rd.ReadObject().AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                Assert.AreEqual(rd.ReadObject().AsBoolean, ((x & 1) == 1));
                Assert.AreEqual((bool?)rd.ReadObject(), ((x & 1) == 1) ? (bool?)null : (bool?)true);
                Assert.IsTrue(GetBytes(x).SequenceEqual((byte[])rd.ReadObject()));
            }
        }

        [TestMethod]
        public void Test2()
        {
            var wr = new BitWriter();
            var rd = new BitReader();
            for (int x = -100; x < 100; x++)
            {
                wr.WriteObjectWithoutType((CtpObject)x.ToString());
                wr.WriteObjectWithoutType((CtpObject)x);
                wr.WriteObjectWithoutType((CtpObject)(float)x);
                wr.WriteObjectWithoutType((CtpObject)(double)x);
                wr.WriteObjectWithoutType((CtpObject)(CtpNumeric)(decimal)x);
                wr.WriteObjectWithoutType((CtpObject)DateTime.Parse("1/1/2010").AddMinutes(x));
                wr.WriteObjectWithoutType((CtpObject)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                wr.WriteObjectWithoutType((CtpObject)((x & 1) == 1));
                wr.WriteObjectWithoutType((CtpObject)(((x & 1) == 1) ? (bool?)null : (bool?)true));
                wr.WriteObjectWithoutType((CtpObject)GetBytes(x));
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.String).AsString, x.ToString());
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.Int64).AsInt32, x);
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.Single).AsSingle, (float)x);
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.Double).AsDouble, (double)x);
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.Numeric).AsNumeric, (CtpNumeric)(decimal)x);
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.CtpTime).AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.Guid).AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
                Assert.AreEqual(rd.ReadObjectWithoutType(CtpTypeCode.Boolean).AsBoolean, ((x & 1) == 1));
                Assert.AreEqual((bool?)rd.ReadObjectWithoutType(((x & 1) == 1) ? CtpTypeCode.Null : CtpTypeCode.Boolean), ((x & 1) == 1) ? (bool?)null : (bool?)true);
                Assert.IsTrue(GetBytes(x).SequenceEqual((byte[])rd.ReadObjectWithoutType(CtpTypeCode.CtpBuffer)));
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
