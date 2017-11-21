using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.DataPointEncoding
{
    public class BitStreamWriter
    {
        /// <summary>
        /// The number of bits in m_bitStreamCache that are valid. 0 Means the bitstream is empty.
        /// </summary>
        private int m_bitStreamCacheBitCount;
        /// <summary>
        /// A cache of bits that need to be flushed to m_buffer when full. Bits filled starting from the right moving left.
        /// </summary>
        private ulong m_bitStreamCache;
        private int m_position;
        private byte[] m_data;

        public Action<uint>[] Write;

        public BitStreamWriter()
        {
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
            m_data = new byte[128];
            Write = new Action<uint>[33];
            Write[0] = WriteBits0;
            Write[1] = WriteBits1;
            Write[2] = WriteBits2;
            Write[3] = WriteBits3;
            Write[4] = WriteBits4;
            Write[5] = WriteBits5;
            Write[6] = WriteBits6;
            Write[7] = WriteBits7;
            Write[8] = WriteBits8;
            Write[9] = WriteBits9;
            Write[10] = WriteBits10;
            Write[11] = WriteBits11;
            Write[12] = WriteBits12;
            Write[13] = WriteBits13;
            Write[14] = WriteBits14;
            Write[15] = WriteBits15;
            Write[16] = WriteBits16;
            Write[17] = WriteBits17;
            Write[18] = WriteBits18;
            Write[19] = WriteBits19;
            Write[20] = WriteBits20;
            Write[21] = WriteBits21;
            Write[22] = WriteBits22;
            Write[23] = WriteBits23;
            Write[24] = WriteBits24;
            Write[25] = WriteBits25;
            Write[26] = WriteBits26;
            Write[27] = WriteBits27;
            Write[28] = WriteBits28;
            Write[29] = WriteBits29;
            Write[30] = WriteBits30;
            Write[31] = WriteBits31;
            Write[32] = WriteBits32;
        }

        public void Reset()
        {
            m_bitStreamCacheBitCount = 0;
            m_position = 0;
            m_bitStreamCache = 0;
        }

        public int RemainingBits => (8 - (m_bitStreamCacheBitCount & 7)) & 7;

        public int BitCount => m_position + m_bitStreamCacheBitCount;

        public void WriteBits0(uint value)
        {

        }

        /// <summary>
        /// While (NotZero), WriteBits8
        /// </summary>
        /// <param name="value"></param>
        public void Write8BitSegments(ulong value)
        {
            TryAgain:
            if (value <= 0)
            {
                WriteBits1(0);
            }
            else
            {
                WriteBits1(1);
                WriteBits8((uint)value);
                value >>= 8;
                goto TryAgain;
            }
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


        public void GetBuffer(out byte[] buffer, out int length)
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
                    m_data[m_position++] = (byte)m_bitStreamCache;
                    m_bitStreamCache = 0;
                    m_bitStreamCacheBitCount = 0;
                }
            }
            buffer = m_data;
            length = m_position;
        }

        private void InternalFlush()
        {
            while (m_bitStreamCacheBitCount > 7)
            {
                m_data[m_position++] = (byte)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 8));
                m_bitStreamCacheBitCount -= 8;

                if (m_position >= m_data.Length)
                {
                    byte[] data = new byte[m_data.Length * 2];
                    m_data.CopyTo(data, 0);
                    m_data = data;
                }
            }
        }

    }

}
