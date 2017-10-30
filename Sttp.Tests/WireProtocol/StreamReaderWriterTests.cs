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
        private PacketReader m_reader;
        private PacketWriter m_writer;

        [TestInitialize]
        public void Init()
        {
            m_writer = new PacketWriter(new SessionDetails());
            m_reader = new PacketReader(new SessionDetails());
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            Assert.AreEqual(testValue, m_reader.ReadByte());
        }

        [DataTestMethod]
        [DataRow(new byte[0])]
        [DataRow(new byte[] { 0, 1, 2, 3, 4, 5, 6 })]
        [DataRow(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255 })]
        [DataRow(null)]
        public void TestBytes(byte[] testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            MetadataEncoderTests.CompareCollection(testValue, m_reader.ReadBytes());
        }

        [DataTestMethod]
        [DataRow(new byte[] { 0, 1, 2, 3, 4, 5, 6 }, 2, 2)]
        public void TestBytesPosition(byte[] testValue, long start, int length)
        {
            m_writer.Clear();
            m_writer.Write(testValue, start, length);
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);

            byte[] sourceCompare = new byte[length];
            Array.Copy(testValue, start, sourceCompare, 0, length);

            var results = m_reader.ReadBytes();

            MetadataEncoderTests.CompareCollection(sourceCompare, results);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            Assert.AreEqual(testValue, m_reader.ReadSByte());
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void TestBoolean(bool testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            Assert.AreEqual(testValue, m_reader.ReadInt15());
            if (testValue < 128)
                Assert.IsTrue(m_writer.UserData == 1);
            else
                Assert.IsTrue(m_writer.UserData == 2);
        }

        [DataTestMethod]

        [DataRow(Int16.MaxValue)]
        [DataRow(Int16.MinValue)]
        [DataRow((Int16)(Int16.MaxValue / 2))]
        public void TestInt16(Int16 testValue)
        {
            m_writer.Clear();
            m_writer.Write(testValue);
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            Assert.AreEqual(testValue, m_reader.ReadInt32());
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(Int32.MaxValue)]
        [DataRow(Int32.MinValue)]
        [DataRow(Int32.MaxValue / 2 - 1)]
        public void TestInt7Bit(Int32 testValue)
        {
            m_writer.Clear();
            m_writer.WriteInt7Bit(testValue);
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            Assert.AreEqual(testValue, m_reader.ReadInt7Bit());
        }

        [DataTestMethod]
        [DataRow(0U)]
        [DataRow(1U)]
        [DataRow(UInt32.MaxValue)]
        [DataRow(UInt32.MinValue)]
        [DataRow(UInt32.MaxValue / 2 - 1)]
        public void TestUInt7Bit(UInt32 testValue)
        {
            m_writer.Clear();
            m_writer.WriteUInt7Bit(testValue);
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
            Assert.AreEqual(testValue, m_reader.ReadUInt7Bit());
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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
            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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


            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);
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

            m_reader.SetBuffer(CommandCode.Invalid, m_writer.ToArray(), 0, m_writer.UserData);


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
