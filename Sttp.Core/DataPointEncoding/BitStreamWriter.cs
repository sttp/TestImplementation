using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP;

namespace Sttp.DataPointEncoding
{
    public unsafe class BitStreamWriter : IDisposable
    {
        private static readonly Action<BitStreamWriter, uint>[] Write1;
        private static readonly Action<BitStreamWriter, ulong>[] Write2;

        private static void WriteBits0(BitStreamWriter writer, uint value) { writer.WriteBits0(value); }
        private static void WriteBits1(BitStreamWriter writer, uint value) { writer.WriteBits1(value); }
        private static void WriteBits2(BitStreamWriter writer, uint value) { writer.WriteBits2(value); }
        private static void WriteBits3(BitStreamWriter writer, uint value) { writer.WriteBits3(value); }
        private static void WriteBits4(BitStreamWriter writer, uint value) { writer.WriteBits4(value); }
        private static void WriteBits5(BitStreamWriter writer, uint value) { writer.WriteBits5(value); }
        private static void WriteBits6(BitStreamWriter writer, uint value) { writer.WriteBits6(value); }
        private static void WriteBits7(BitStreamWriter writer, uint value) { writer.WriteBits7(value); }
        private static void WriteBits8(BitStreamWriter writer, uint value) { writer.WriteBits8(value); }
        private static void WriteBits9(BitStreamWriter writer, uint value) { writer.WriteBits9(value); }
        private static void WriteBits10(BitStreamWriter writer, uint value) { writer.WriteBits10(value); }
        private static void WriteBits11(BitStreamWriter writer, uint value) { writer.WriteBits11(value); }
        private static void WriteBits12(BitStreamWriter writer, uint value) { writer.WriteBits12(value); }
        private static void WriteBits13(BitStreamWriter writer, uint value) { writer.WriteBits13(value); }
        private static void WriteBits14(BitStreamWriter writer, uint value) { writer.WriteBits14(value); }
        private static void WriteBits15(BitStreamWriter writer, uint value) { writer.WriteBits15(value); }
        private static void WriteBits16(BitStreamWriter writer, uint value) { writer.WriteBits16(value); }
        private static void WriteBits17(BitStreamWriter writer, uint value) { writer.WriteBits17(value); }
        private static void WriteBits18(BitStreamWriter writer, uint value) { writer.WriteBits18(value); }
        private static void WriteBits19(BitStreamWriter writer, uint value) { writer.WriteBits19(value); }
        private static void WriteBits20(BitStreamWriter writer, uint value) { writer.WriteBits20(value); }
        private static void WriteBits21(BitStreamWriter writer, uint value) { writer.WriteBits21(value); }
        private static void WriteBits22(BitStreamWriter writer, uint value) { writer.WriteBits22(value); }
        private static void WriteBits23(BitStreamWriter writer, uint value) { writer.WriteBits23(value); }
        private static void WriteBits24(BitStreamWriter writer, uint value) { writer.WriteBits24(value); }
        private static void WriteBits25(BitStreamWriter writer, uint value) { writer.WriteBits25(value); }
        private static void WriteBits26(BitStreamWriter writer, uint value) { writer.WriteBits26(value); }
        private static void WriteBits27(BitStreamWriter writer, uint value) { writer.WriteBits27(value); }
        private static void WriteBits28(BitStreamWriter writer, uint value) { writer.WriteBits28(value); }
        private static void WriteBits29(BitStreamWriter writer, uint value) { writer.WriteBits29(value); }
        private static void WriteBits30(BitStreamWriter writer, uint value) { writer.WriteBits30(value); }
        private static void WriteBits31(BitStreamWriter writer, uint value) { writer.WriteBits31(value); }
        private static void WriteBits32(BitStreamWriter writer, uint value) { writer.WriteBits32(value); }
        private static void WriteBits33(BitStreamWriter writer, ulong value) { writer.WriteBits33(value); }
        private static void WriteBits34(BitStreamWriter writer, ulong value) { writer.WriteBits34(value); }
        private static void WriteBits35(BitStreamWriter writer, ulong value) { writer.WriteBits35(value); }
        private static void WriteBits36(BitStreamWriter writer, ulong value) { writer.WriteBits36(value); }
        private static void WriteBits37(BitStreamWriter writer, ulong value) { writer.WriteBits37(value); }
        private static void WriteBits38(BitStreamWriter writer, ulong value) { writer.WriteBits38(value); }
        private static void WriteBits39(BitStreamWriter writer, ulong value) { writer.WriteBits39(value); }
        private static void WriteBits40(BitStreamWriter writer, ulong value) { writer.WriteBits40(value); }
        private static void WriteBits41(BitStreamWriter writer, ulong value) { writer.WriteBits41(value); }
        private static void WriteBits42(BitStreamWriter writer, ulong value) { writer.WriteBits42(value); }
        private static void WriteBits43(BitStreamWriter writer, ulong value) { writer.WriteBits43(value); }
        private static void WriteBits44(BitStreamWriter writer, ulong value) { writer.WriteBits44(value); }
        private static void WriteBits45(BitStreamWriter writer, ulong value) { writer.WriteBits45(value); }
        private static void WriteBits46(BitStreamWriter writer, ulong value) { writer.WriteBits46(value); }
        private static void WriteBits47(BitStreamWriter writer, ulong value) { writer.WriteBits47(value); }
        private static void WriteBits48(BitStreamWriter writer, ulong value) { writer.WriteBits48(value); }
        private static void WriteBits49(BitStreamWriter writer, ulong value) { writer.WriteBits49(value); }
        private static void WriteBits50(BitStreamWriter writer, ulong value) { writer.WriteBits50(value); }
        private static void WriteBits51(BitStreamWriter writer, ulong value) { writer.WriteBits51(value); }
        private static void WriteBits52(BitStreamWriter writer, ulong value) { writer.WriteBits52(value); }
        private static void WriteBits53(BitStreamWriter writer, ulong value) { writer.WriteBits53(value); }
        private static void WriteBits54(BitStreamWriter writer, ulong value) { writer.WriteBits54(value); }
        private static void WriteBits55(BitStreamWriter writer, ulong value) { writer.WriteBits55(value); }
        private static void WriteBits56(BitStreamWriter writer, ulong value) { writer.WriteBits56(value); }
        private static void WriteBits57(BitStreamWriter writer, ulong value) { writer.WriteBits57(value); }
        private static void WriteBits58(BitStreamWriter writer, ulong value) { writer.WriteBits58(value); }
        private static void WriteBits59(BitStreamWriter writer, ulong value) { writer.WriteBits59(value); }
        private static void WriteBits60(BitStreamWriter writer, ulong value) { writer.WriteBits60(value); }
        private static void WriteBits61(BitStreamWriter writer, ulong value) { writer.WriteBits61(value); }
        private static void WriteBits62(BitStreamWriter writer, ulong value) { writer.WriteBits62(value); }
        private static void WriteBits63(BitStreamWriter writer, ulong value) { writer.WriteBits63(value); }
        private static void WriteBits64(BitStreamWriter writer, ulong value) { writer.WriteBits64(value); }

        static BitStreamWriter()
        {
            Write1 = new Action<BitStreamWriter, uint>[33];
            Write1[0] = WriteBits0;
            Write1[1] = WriteBits1;
            Write1[2] = WriteBits2;
            Write1[3] = WriteBits3;
            Write1[4] = WriteBits4;
            Write1[5] = WriteBits5;
            Write1[6] = WriteBits6;
            Write1[7] = WriteBits7;
            Write1[8] = WriteBits8;
            Write1[9] = WriteBits9;
            Write1[10] = WriteBits10;
            Write1[11] = WriteBits11;
            Write1[12] = WriteBits12;
            Write1[13] = WriteBits13;
            Write1[14] = WriteBits14;
            Write1[15] = WriteBits15;
            Write1[16] = WriteBits16;
            Write1[17] = WriteBits17;
            Write1[18] = WriteBits18;
            Write1[19] = WriteBits19;
            Write1[20] = WriteBits20;
            Write1[21] = WriteBits21;
            Write1[22] = WriteBits22;
            Write1[23] = WriteBits23;
            Write1[24] = WriteBits24;
            Write1[25] = WriteBits25;
            Write1[26] = WriteBits26;
            Write1[27] = WriteBits27;
            Write1[28] = WriteBits28;
            Write1[29] = WriteBits29;
            Write1[30] = WriteBits30;
            Write1[31] = WriteBits31;
            Write1[32] = WriteBits32;
            Write2 = new Action<BitStreamWriter, ulong>[32];
            Write2[0] = WriteBits33;
            Write2[1] = WriteBits34;
            Write2[2] = WriteBits35;
            Write2[3] = WriteBits36;
            Write2[4] = WriteBits37;
            Write2[5] = WriteBits38;
            Write2[6] = WriteBits39;
            Write2[7] = WriteBits40;
            Write2[8] = WriteBits41;
            Write2[9] = WriteBits42;
            Write2[10] = WriteBits43;
            Write2[11] = WriteBits44;
            Write2[12] = WriteBits45;
            Write2[13] = WriteBits46;
            Write2[14] = WriteBits47;
            Write2[15] = WriteBits48;
            Write2[16] = WriteBits49;
            Write2[17] = WriteBits50;
            Write2[18] = WriteBits51;
            Write2[19] = WriteBits52;
            Write2[20] = WriteBits53;
            Write2[21] = WriteBits54;
            Write2[22] = WriteBits55;
            Write2[23] = WriteBits56;
            Write2[24] = WriteBits57;
            Write2[25] = WriteBits58;
            Write2[26] = WriteBits59;
            Write2[27] = WriteBits50;
            Write2[28] = WriteBits61;
            Write2[29] = WriteBits62;
            Write2[30] = WriteBits63;
            Write2[31] = WriteBits64;
        }

        /// <summary>
        /// The number of bits in m_bitStreamCache that are valid. 0 Means the bitstream is empty.
        /// </summary>
        private int m_bitStreamCacheBitCount;
        /// <summary>
        /// A cache of bits that need to be flushed to m_buffer when full. Bits filled starting from the right moving left.
        /// </summary>
        private ulong m_bitStreamCache;

        private SnapNodeWriter m_writer;

        public BitStreamWriter()
        {
            m_writer = new SnapNodeWriter();
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
        }

        public void Clear()
        {
            m_bitStreamCache = 0;
            m_bitStreamCacheBitCount = 0;
        }

        public int Length
        {
            get
            {
                return (m_bitStreamCacheBitCount + 7) >> 3 //Round up to nearest byte
                       + m_writer.NodeLength;
            }
        }

        #region Custom Types
        #region [ 1 byte values ]

        public void Write(byte value)
        {
            WriteBits8(value);
        }

        #endregion

        #region [ 2-byte values ]

        public void Write(short value)
        {
            WriteBits16((ushort)value);
        }

        #endregion

        #region [ 4-byte values ]

        public void Write(int value)
        {
            WriteBits32((uint)value);
        }

        public void Write(uint value)
        {
            WriteBits32(value);
        }

        public void Write(float value)
        {
            WriteBits32(*(uint*)&value);
        }

        #endregion

        #region [ 8-byte values ]

        public void Write(long value)
        {
            WriteBits64((ulong)value);
        }

        public void Write(ulong value)
        {
            WriteBits64(value);
        }

        public void Write(double value)
        {
            WriteBits64(*(ulong*)&value);
        }

        #endregion

        #region [ 16-byte values ]

        public void Write(Guid value)
        {
            foreach (var b in value.ToRfcBytes())
            {
                WriteBits8(b);
            }
        }

        #endregion

        public void Write(byte[] value, int start, int length)
        {
            value.ValidateParameters(start, length);
            Write4BitSegments((uint)length);
            if (length == 0)
                return;

            for (int x = start; x < length; x++)
            {
                WriteBits8(value[x]);
            }
        }

        public void Write(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Write(value, 0, value.Length);
        }

        public void Write(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
            {
                Write4BitSegments(0);
                return;
            }

            Write(Encoding.UTF8.GetBytes(value));
        }

        public void Write(CtpTime value)
        {
            Write(value.Ticks);
        }

        public void Write(CtpBuffer value)
        {
            Write4BitSegments((uint)value.Length);
            if (value.Length == 0)
                return;

            Write(value.ToBuffer());
        }

        public void Write(CtpCommand value)
        {
            Write4BitSegments((uint)value.Length);
            if (value.Length == 0)
                return;

            Write(value.ToArray());
        }

        #endregion

        /// <summary>
        /// While (NotZero), WriteBits8
        /// </summary>
        /// <param name="value"></param>
        public void Write8BitSegments(ulong value)
        {
            int bits = 0;
            ulong tmpValue = value;
            while (tmpValue > 0)
            {
                bits += 8;
                tmpValue >>= 8;
                WriteBits1(1);
            }
            WriteBits1(0);
            WriteBits(bits, value);
        }

        /// <summary>
        /// While (NotZero), WriteBits4
        /// </summary>
        /// <param name="value"></param>
        public void Write4BitSegments(ulong value)
        {
            int bits = 0;
            ulong tmpValue = value;
            while (tmpValue > 0)
            {
                bits += 4;
                tmpValue >>= 4;
                WriteBits1(1);
            }
            WriteBits1(0);
            WriteBits(bits, value);
        }

        public void WriteBits(int bitCount, uint value)
        {
            Write1[bitCount](this, value);
        }

        public void WriteBits(int bitCount, ulong value)
        {
            if (bitCount < 33)
                Write1[bitCount](this, (uint)value);
            else
                Write2[bitCount - 33](this, value);
        }

        public void WriteBits0(uint value)
        {

        }

        public void WriteBits1(bool value)
        {
            WriteBits1(value ? 1u : 0u);
        }

        public void WriteBits1(uint value)
        {
            const int bits = 1;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits2(uint value)
        {
            const int bits = 2;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits3(uint value)
        {
            const int bits = 3;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits4(uint value)
        {
            const int bits = 4;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits5(uint value)
        {
            const int bits = 5;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits6(uint value)
        {
            const int bits = 6;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits7(uint value)
        {
            const int bits = 7;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits8(uint value)
        {
            const int bits = 8;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits9(uint value)
        {
            const int bits = 9;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits10(uint value)
        {
            const int bits = 10;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits11(uint value)
        {
            const int bits = 11;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits12(uint value)
        {
            const int bits = 12;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits13(uint value)
        {
            const int bits = 13;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits14(uint value)
        {
            const int bits = 14;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits15(uint value)
        {
            const int bits = 15;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits16(uint value)
        {
            const int bits = 16;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits17(uint value)
        {
            const int bits = 17;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits18(uint value)
        {
            const int bits = 18;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits19(uint value)
        {
            const int bits = 19;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits20(uint value)
        {
            const int bits = 20;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits21(uint value)
        {
            const int bits = 21;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits22(uint value)
        {
            const int bits = 22;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits23(uint value)
        {
            const int bits = 23;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits24(uint value)
        {
            const int bits = 24;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits25(uint value)
        {
            const int bits = 25;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits26(uint value)
        {
            const int bits = 26;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits27(uint value)
        {
            const int bits = 27;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits28(uint value)
        {
            const int bits = 28;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits29(uint value)
        {
            const int bits = 29;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits30(uint value)
        {
            const int bits = 30;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits31(uint value)
        {
            const int bits = 31;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1u << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits32(uint value)
        {
            const int bits = 32;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | value;
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits33(ulong value)
        {
            const int bits = 33;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits34(ulong value)
        {
            const int bits = 34;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits35(ulong value)
        {
            const int bits = 35;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits36(ulong value)
        {
            const int bits = 36;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits37(ulong value)
        {
            const int bits = 37;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits38(ulong value)
        {
            const int bits = 38;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits39(ulong value)
        {
            const int bits = 39;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits40(ulong value)
        {
            const int bits = 40;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits41(ulong value)
        {
            const int bits = 41;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits42(ulong value)
        {
            const int bits = 42;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits43(ulong value)
        {
            const int bits = 43;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits44(ulong value)
        {
            const int bits = 44;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits45(ulong value)
        {
            const int bits = 45;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits46(ulong value)
        {
            const int bits = 46;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits47(ulong value)
        {
            const int bits = 47;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits48(ulong value)
        {
            const int bits = 48;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits49(ulong value)
        {
            const int bits = 49;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits50(ulong value)
        {
            const int bits = 50;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits51(ulong value)
        {
            const int bits = 51;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits52(ulong value)
        {
            const int bits = 52;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1u << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits53(ulong value)
        {
            const int bits = 53;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits54(ulong value)
        {
            const int bits = 54;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits55(ulong value)
        {
            const int bits = 55;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits56(ulong value)
        {
            const int bits = 56;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits57(ulong value)
        {
            const int bits = 57;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits58(ulong value)
        {
            const int bits = 58;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits59(ulong value)
        {
            const int bits = 59;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits60(ulong value)
        {
            const int bits = 60;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits61(ulong value)
        {
            const int bits = 61;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits62(ulong value)
        {
            const int bits = 62;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits63(ulong value)
        {
            const int bits = 63;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1ul << bits) - 1));
            m_bitStreamCacheBitCount += bits;
        }
        public void WriteBits64(ulong value)
        {
            const int bits = 64;
            if (m_bitStreamCacheBitCount > 64 - bits)
                InternalFlush();

            m_bitStreamCache = (m_bitStreamCache << bits) | value;
            m_bitStreamCacheBitCount += bits;
        }

        private void InternalFlush()
        {
            if (m_bitStreamCacheBitCount == 64)
            {
                m_writer.WriteBits64(m_bitStreamCache);
                m_bitStreamCacheBitCount -= 64;
            }
            else if (m_bitStreamCacheBitCount >= 56)
            {
                m_writer.WriteBits56(m_bitStreamCache >> (m_bitStreamCacheBitCount - 56));
                m_bitStreamCacheBitCount -= 56;
            }
            else if (m_bitStreamCacheBitCount >= 48)
            {
                m_writer.WriteBits48(m_bitStreamCache >> (m_bitStreamCacheBitCount - 48));
                m_bitStreamCacheBitCount -= 48;
            }
            else if (m_bitStreamCacheBitCount >= 40)
            {
                m_writer.WriteBits40(m_bitStreamCache >> (m_bitStreamCacheBitCount - 40));
                m_bitStreamCacheBitCount -= 40;
            }
            else if (m_bitStreamCacheBitCount >= 32)
            {
                m_writer.WriteBits32((uint)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 32)));
                m_bitStreamCacheBitCount -= 32;
            }
            else if (m_bitStreamCacheBitCount >= 24)
            {
                m_writer.WriteBits24((uint)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 24)));
                m_bitStreamCacheBitCount -= 24;
            }
            else if (m_bitStreamCacheBitCount >= 16)
            {
                m_writer.WriteBits16((uint)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 16)));
                m_bitStreamCacheBitCount -= 16;
            }
            else if (m_bitStreamCacheBitCount >= 8)
            {
                m_writer.WriteBits8((uint)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 8)));
                m_bitStreamCacheBitCount -= 8;
            }
        }

        public void Dispose()
        {
            if (m_bitStreamCacheBitCount > 0)
            {
                if (m_bitStreamCacheBitCount > 7)
                {
                    InternalFlush();
                }

                if (m_bitStreamCacheBitCount > 0)
                {
                    //Make up 8 bits by padding.
                    m_bitStreamCache <<= 8 - m_bitStreamCacheBitCount;
                    m_writer.WriteBits8((byte)m_bitStreamCache);
                    m_bitStreamCache = 0;
                    m_bitStreamCacheBitCount = 0;
                }
            }

            m_writer = null;
        }


        private class SnapNodeWriter : IDisposable
        {
            //Layout:
            //Guid NodeType
            //int NodeLength
            //int Crc32
            //byte[] Bytes; 
            public const int HeaderSize = 24;
            private byte[] m_buffer;
            private int m_position;
            private int m_endOfStreamPosition;
            private BitStreamWriter m_wr;

            internal SnapNodeWriter()
            {
                m_buffer = new byte[256];
                m_position = HeaderSize;
                m_endOfStreamPosition = HeaderSize;
            }

            /// <summary>
            /// The current byte position. This value can be used to hold the location of a block
            /// whose value will be written later using the OverWrite commands.
            /// </summary>
            public int Position
            {
                get => m_position - HeaderSize;
                set
                {
                    m_endOfStreamPosition = NodeLength;

                    if (value < 0 || value > m_endOfStreamPosition - HeaderSize)
                        throw new ArgumentOutOfRangeException(nameof(value));
                    m_position = value + HeaderSize;

                }
            }

            /// <summary>
            /// Gets the current size of this node.
            /// </summary>
            public int NodeLength => Math.Max(m_position, m_endOfStreamPosition);

            public void Dispose()
            {
            }

            /// <summary>
            /// Ensures that the byte stream has the room to store the specified number of bytes.
            /// </summary>
            /// <param name="neededBytes"></param>
            private void EnsureCapacityBytes(int neededBytes)
            {
                if (m_position + neededBytes >= m_buffer.Length)
                    GrowBytes(neededBytes);
            }

            private void GrowBytes(int neededBytes)
            {
                while (m_position + neededBytes >= m_buffer.Length)
                {
                    byte[] newBuffer = new byte[m_buffer.Length * 2];
                    m_buffer.CopyTo(newBuffer, 0);
                    m_buffer = newBuffer;
                }
            }


            #region [ Writing Bits ]


            public void WriteBits8(uint value)
            {
                EnsureCapacityBytes(1);
                m_buffer[m_position + 0] = (byte)value;
                m_position += 1;
            }
            public void WriteBits16(uint value)
            {
                EnsureCapacityBytes(2);
                m_buffer[m_position + 0] = (byte)(value >> 8);
                m_buffer[m_position + 1] = (byte)value;
                m_position += 2;
            }
            public void WriteBits24(uint value)
            {
                EnsureCapacityBytes(3);
                m_buffer[m_position + 0] = (byte)(value >> 16);
                m_buffer[m_position + 1] = (byte)(value >> 8);
                m_buffer[m_position + 2] = (byte)value;
                m_position += 3;
            }
            public void WriteBits32(uint value)
            {
                EnsureCapacityBytes(4);
                m_buffer[m_position + 0] = (byte)(value >> 24);
                m_buffer[m_position + 1] = (byte)(value >> 16);
                m_buffer[m_position + 2] = (byte)(value >> 8);
                m_buffer[m_position + 3] = (byte)value;
                m_position += 4;
            }
            public void WriteBits40(ulong value)
            {
                EnsureCapacityBytes(5);
                m_buffer[m_position + 0] = (byte)(value >> 32);
                m_buffer[m_position + 1] = (byte)(value >> 24);
                m_buffer[m_position + 2] = (byte)(value >> 16);
                m_buffer[m_position + 3] = (byte)(value >> 8);
                m_buffer[m_position + 4] = (byte)value;
                m_position += 5;
            }
            public void WriteBits48(ulong value)
            {
                EnsureCapacityBytes(6);
                m_buffer[m_position + 0] = (byte)(value >> 40);
                m_buffer[m_position + 1] = (byte)(value >> 32);
                m_buffer[m_position + 2] = (byte)(value >> 24);
                m_buffer[m_position + 3] = (byte)(value >> 16);
                m_buffer[m_position + 4] = (byte)(value >> 8);
                m_buffer[m_position + 5] = (byte)value;
                m_position += 6;
            }
            public void WriteBits56(ulong value)
            {
                EnsureCapacityBytes(7);
                m_buffer[m_position + 0] = (byte)(value >> 48);
                m_buffer[m_position + 1] = (byte)(value >> 40);
                m_buffer[m_position + 2] = (byte)(value >> 32);
                m_buffer[m_position + 3] = (byte)(value >> 24);
                m_buffer[m_position + 4] = (byte)(value >> 16);
                m_buffer[m_position + 5] = (byte)(value >> 8);
                m_buffer[m_position + 6] = (byte)value;
                m_position += 7;
            }
            public void WriteBits64(ulong value)
            {
                EnsureCapacityBytes(8);
                m_buffer[m_position + 0] = (byte)(value >> 56);
                m_buffer[m_position + 1] = (byte)(value >> 48);
                m_buffer[m_position + 2] = (byte)(value >> 40);
                m_buffer[m_position + 3] = (byte)(value >> 32);
                m_buffer[m_position + 4] = (byte)(value >> 24);
                m_buffer[m_position + 5] = (byte)(value >> 16);
                m_buffer[m_position + 6] = (byte)(value >> 8);
                m_buffer[m_position + 7] = (byte)value;
                m_position += 8;
            }

            #endregion
        }


    }

}
