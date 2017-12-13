using System;
using System.Collections.Generic;
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
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsString, x.ToString());
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsInt32, x);
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsSingle, (float)x);
                Assert.AreEqual(SttpValueEncodingNative.Load(rd).AsDouble, (double)x);
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
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.String).AsString, x.ToString());
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Int64).AsInt32, x);
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Single).AsSingle, (float)x);
                Assert.AreEqual(SttpValueEncodingWithoutType.Load(rd, SttpValueTypeCode.Double).AsDouble, (double)x);
            }
        }


        [TestMethod]
        public void Test3()
        {
            var wr = new ByteWriter();
            var rd = new ByteReader();
            for (int x = -100; x < 100; x++)
            {
                var y = -x;
                SttpValueEncodingDelta.Save(wr, (SttpValue)x.ToString(), (SttpValue)y.ToString());
                SttpValueEncodingDelta.Save(wr, (SttpValue)x, (SttpValue)y);
                SttpValueEncodingDelta.Save(wr, (SttpValue)(float)x, (SttpValue)(float)y);
                SttpValueEncodingDelta.Save(wr, (SttpValue)(double)x, (SttpValue)(float)y);
            }

            rd.SetBuffer(wr.ToArray());

            for (int x = -100; x < 100; x++)
            {
                var y = -x;
                Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)y.ToString()).AsString, x.ToString());
                Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)y).AsInt32, x);
                Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)(float)y).AsSingle, (float)x);
                Assert.AreEqual(SttpValueEncodingDelta.Load(rd, (SttpValue)(float)y).AsDouble, (double)x);
            }
        }
    }
}
