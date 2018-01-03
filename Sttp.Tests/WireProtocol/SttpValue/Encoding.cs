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
                SttpValueEncodingNative.Save(wr, (SttpValue)BigEndian.GetBytes(x));
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
                Assert.IsTrue(BigEndian.GetBytes(x).SequenceEqual((byte[])SttpValueEncodingNative.Load(rd)));
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
                SttpValueEncodingWithoutType.Save(wr, (SttpValue)BigEndian.GetBytes(x));
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
                Assert.IsTrue(BigEndian.GetBytes(x).SequenceEqual((byte[])SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.SttpBuffer)));
            }
        }


        //[TestMethod]
        //public void Test3()
        //{
        //    var wr = new ByteWriter();
        //    var rd = new ByteReader();
        //    for (int x = -100; x < 100; x++)
        //    {
        //        var y = -x;
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)x.ToString(), (SttpValue)y.ToString());
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)x, (SttpValue)y);
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)(float)x, (SttpValue)(float)y);
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)(double)x, (SttpValue)(float)y);
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)DateTime.Parse("1/1/2010").AddMinutes(x), (SttpValue)DateTime.Parse("1/1/2010").AddMinutes(y));
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10), (SttpValue)new Guid(y, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)((x & 1) == 1), (SttpValue)((y & 1) == 1));
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)(((x & 1) == 1) ? (bool?)null : (bool?)true), (SttpValue)(((y & 1) == 1) ? (bool?)null : (bool?)true));
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)(decimal)x, (SttpValue)(decimal)y);
        //        SttpValueEncodingDelta.Save(wr, (SttpValue)BigEndian.GetBytes(x), (SttpValue)BigEndian.GetBytes(y));
        //    }

        //    rd.SetBuffer(wr.ToArray());

        //    for (int x = -100; x < 100; x++)
        //    {
        //        var y = -x;
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)y.ToString()).AsString, x.ToString());
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)y).AsInt32, x);
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)(float)y).AsSingle, (float)x);
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)(float)y).AsDouble, (double)x);
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)DateTime.Parse("1/1/2010").AddMinutes(y)).AsDateTime, DateTime.Parse("1/1/2010").AddMinutes(x));
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)new Guid(y, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)).AsGuid, new Guid(x, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)((y & 1) == 1)).AsBoolean, ((x & 1) == 1));
        //        Assert.AreEqual((bool?)SttpValueEncodingDelta.Load(rd, (SttpValue)(((y & 1) == 1) ? (bool?)null : (bool?)true)), ((x & 1) == 1) ? (bool?)null : (bool?)true);
        //        Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)(decimal)y).AsDecimal, (decimal)x);
        //        Assert.IsTrue(BigEndian.GetBytes(x).SequenceEqual((byte[])SttpValueEncodingDelta.Load(rd, (SttpValue)BigEndian.GetBytes(y))));
        //    }
        //}

    }
}
