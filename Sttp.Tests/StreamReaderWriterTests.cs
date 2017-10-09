using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sttp.WireProtocol;

namespace Sttp.Tests
{
    [TestClass]
    public class StreamReaderWriterTests
    {
        private StreamReader m_reader;
        private StreamWriter m_writer;

        [TestInitialize]
        public void Init()
        {
            m_writer = new StreamWriter();
            m_reader = new StreamReader();
        }



        [DataTestMethod]
        [DataRow((byte)1)]
        [DataRow(byte.MaxValue)]
        [DataRow(byte.MinValue)]
        [DataRow((byte)(byte.MaxValue / 2))]
        public void TestByte(byte testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadByte());
        }

        [DataTestMethod]
        [DataRow((sbyte)1)]
        [DataRow(sbyte.MaxValue)]
        [DataRow(sbyte.MinValue)]
        [DataRow((sbyte)(sbyte.MaxValue / 2))]
        public void TestSByte(sbyte testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadSByte());
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void TestBoolean(bool testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadBoolean());
        }

        [DataTestMethod]
        [DataRow('a')]
        [DataRow('A')]
        [DataRow('(')]
        [DataRow('$')]
        [DataRow('%')]
        [DataRow('\n')]
        [DataRow('\t')]
        [DataRow('\0')]
        [DataRow('λ')]
        [DataRow('Δ')]
        public void TestChar(char testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadChar());
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(127)]
        [DataRow(128)]
        [DataRow(1000)]
        [DataRow(Int16.MaxValue / 2)]
        public void TestInt15(int testValue)
        {
            m_writer.Clear();
            m_writer.WriteInt15(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadInt15());
            if (testValue < 128)
                Assert.IsTrue(m_writer.Length == 1);
            else
                Assert.IsTrue(m_writer.Length == 2);
        }

        [DataTestMethod]

        [DataRow(Int16.MaxValue)]
        [DataRow(Int16.MinValue)]
        [DataRow((Int16)(Int16.MaxValue / 2))]
        public void TestInt16(Int16 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadInt16());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(Int32.MaxValue)]
        [DataRow(Int32.MinValue)]
        [DataRow(Int32.MaxValue / 2)]
        public void TestInt32(Int32 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadInt32());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(Int64.MaxValue)]
        [DataRow(Int64.MinValue)]
        [DataRow((Int64)(Int64.MaxValue / 2))]
        public void TestInt64(Int64 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadInt64());
        }


        [DataTestMethod]

        [DataRow(UInt16.MaxValue)]
        [DataRow(UInt16.MinValue)]
        [DataRow((UInt16)(UInt16.MaxValue / 2))]
        public void TestUInt16(UInt16 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadUInt16());
        }

        [DataTestMethod]

        [DataRow(UInt32.MaxValue)]
        [DataRow(UInt32.MinValue)]
        [DataRow(UInt32.MaxValue / 2)]
        public void TestUInt32(UInt32 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadUInt32());
        }

        [DataTestMethod]

        [DataRow(UInt64.MaxValue)]
        [DataRow(UInt64.MinValue)]
        [DataRow((UInt64)(UInt64.MaxValue / 2))]
        public void TestUInt64(UInt64 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadUInt64());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-0)]
        [DataRow(1)]
        [DataRow(double.MaxValue)]
        [DataRow(double.MinValue)]
        [DataRow(double.MaxValue / 2)]
        public void TestDouble(double testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadDouble());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-0)]
        [DataRow(1)]
        [DataRow(float.MaxValue)]
        [DataRow(float.MinValue)]
        [DataRow(float.MaxValue / 2)]
        public void TestSingle(float testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadSingle());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-0)]
        [DataRow(1)]
        [DataRow(double.MinValue)]
        [DataRow(double.MaxValue)]
        [DataRow(double.MaxValue / 2)]
        public void TestSingle(Decimal testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadDecimal());
        }

        [TestMethod]
        public void TestDateTime()
        {

            TestDateTimeImpl(DateTime.Now);
            TestDateTimeImpl(DateTime.Now.AddYears(-5));
            TestDateTimeImpl(DateTime.MaxValue);
            TestDateTimeImpl(DateTime.MinValue);
        }
        private void TestDateTimeImpl(DateTime testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadDateTime());
        }

        [TestMethod]
        public void TestGuid()
        {
            TestGuidImpl(Guid.NewGuid());
            TestGuidImpl(new Guid());

        }
        private void TestGuidImpl(Guid testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadGuid());
        }

        [DataTestMethod]
        [DataRow("0")]
        [DataRow("\0")]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("long test data string with some long text")]
        [DataRow("short")]
        public void TestString(string testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            Assert.AreEqual(testValue, m_reader.ReadString());
        }

        [TestMethod]
        public void TestArray()
        {
            var str = new string[] { "one", "two", "three" };
            var shrt = new short[] { 1, 2, 3 };
            var flt = new float[] { 1.0f, 2.0f, 3.0f };
            var lots = Enumerable.Range(1, 500).ToArray();

            m_writer.Clear();
            m_writer.WriteArray(str);
            m_writer.WriteArray(shrt);
            m_writer.WriteArray(flt);
            m_writer.WriteArray(lots);
            

            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);
            var strResult = m_reader.ReadArray<string>();
            var shrtResult = m_reader.ReadArray<short>();
            var fltResult = m_reader.ReadArray<float>();
            var lotsResult = m_reader.ReadArray<int>();

            Assert.AreEqual(str.Length, strResult.Length);
            Assert.AreEqual(shrt.Length, shrtResult.Length);
            Assert.AreEqual(shrt.Length, fltResult.Length);
            Assert.AreEqual(lots.Length, lotsResult.Length);

            for (int i = 0; i < str.Length; i++)
            {
                Assert.AreEqual(str[i], strResult[i]);
            }

            for (int i = 0; i < shrt.Length; i++)
            {
                Assert.AreEqual(shrt[i], shrtResult[i]);
            }

            for (int i = 0; i < flt.Length; i++)
            {
                Assert.AreEqual(flt[i], fltResult[i]);
            }

            for (int i = 0; i < lots.Length; i++)
            {
                Assert.AreEqual(lots[i], lotsResult[i]);
            }



        }

        [TestMethod]
        public void TestGenerics()
        {
            var guid = Guid.NewGuid();
            bool @bool = true;
            byte @byte = 1;
            sbyte @sbyte = (sbyte)-1;
            char @char = 'a';
            DateTime dt = DateTime.Now;
            Decimal @decimal = Decimal.MaxValue;
            Single single = Single.MaxValue;
            Double @double = Double.MaxValue;
            Int16 int16 = Int16.MaxValue;
            Int32 int32 = Int32.MaxValue;
            Int64 int64 = Int64.MaxValue;
            UInt16 uint16 = UInt16.MaxValue;
            UInt32 uint32 = UInt32.MaxValue;
            UInt64 uint64 = UInt64.MaxValue;
            var @string = "test";

            // some enums
            var code = CommandCode.Metadata;
            var meta = MetadataCommand.AddColumn;


            m_writer.Clear();
            m_writer.Write<Guid>(guid);
            m_writer.Write<bool>(@bool);
            m_writer.Write<byte>(@byte);
            m_writer.Write<sbyte>(@sbyte);
            m_writer.Write<DateTime>(dt);
            m_writer.Write<Decimal>(@decimal);
            m_writer.Write<Single>(single);
            m_writer.Write<Double>(@double);
            m_writer.Write<Int16>(int16);
            m_writer.Write<Int32>(int32);
            m_writer.Write<Int64>(int64);
            m_writer.Write<UInt16>(uint16);
            m_writer.Write<UInt32>(uint32);
            m_writer.Write<UInt64>(uint64);
            m_writer.Write<string>(@string);
            m_writer.Write<CommandCode>(code); // explicit
            m_writer.Write(code); // implicit
            m_writer.Write<MetadataCommand>(meta);

            m_reader.Fill(m_writer.ToArray(), 0, m_writer.Length);


            Assert.AreEqual(guid, m_reader.Read<Guid>());
            Assert.AreEqual(@bool, m_reader.Read<bool>());
            Assert.AreEqual(@byte, m_reader.Read<byte>());
            Assert.AreEqual(@sbyte, m_reader.Read<sbyte>());
            Assert.AreEqual(dt, m_reader.Read<DateTime>());
            Assert.AreEqual(@decimal, m_reader.Read<Decimal>());
            Assert.AreEqual(single, m_reader.Read<Single>());
            Assert.AreEqual(@double, m_reader.Read<Double>());
            Assert.AreEqual(int16, m_reader.Read<Int16>());
            Assert.AreEqual(int32, m_reader.Read<Int32>());
            Assert.AreEqual(int64, m_reader.Read<Int64>());
            Assert.AreEqual(uint16, m_reader.Read<UInt16>());
            Assert.AreEqual(uint32, m_reader.Read<UInt32>());
            Assert.AreEqual(uint64, m_reader.Read<UInt64>());
            Assert.AreEqual(@string, m_reader.Read<string>());
            Assert.AreEqual(code, m_reader.Read<CommandCode>());
            Assert.AreEqual(code, m_reader.Read<CommandCode>());
            Assert.AreEqual(meta, m_reader.Read<MetadataCommand>());
        }
    }
}
