using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp
{
    public class BitStreamReader
    {
        /// <summary>
        /// The number of bits in m_bitStreamCache that are valid. 0 Means the bitstream is empty.
        /// </summary>
        protected int m_bitCount;
        /// <summary>
        /// A cache of bits that need to be flushed to m_buffer when full. Bits filled starting from the right moving left.
        /// </summary>
        protected ulong m_cache;

        private byte[] m_buffer;
        private int m_position;
        private int m_length;

        private Func<uint>[] m_readBits;

        public BitStreamReader(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_position = position;
            m_length = length;
            m_bitCount = 0;
            m_cache = 0;
            m_readBits = new Func<uint>[33];
            m_readBits[0] = ReadBits0;
            m_readBits[1] = ReadBits1;
            m_readBits[2] = ReadBits2;
            m_readBits[3] = ReadBits3;
            m_readBits[4] = ReadBits4;
            m_readBits[5] = ReadBits5;
            m_readBits[6] = ReadBits6;
            m_readBits[7] = ReadBits7;
            m_readBits[8] = ReadBits8;
            m_readBits[9] = ReadBits9;
            m_readBits[10] = ReadBits10;
            m_readBits[11] = ReadBits11;
            m_readBits[12] = ReadBits12;
            m_readBits[13] = ReadBits13;
            m_readBits[14] = ReadBits14;
            m_readBits[15] = ReadBits15;
            m_readBits[16] = ReadBits16;
            m_readBits[17] = ReadBits17;
            m_readBits[18] = ReadBits18;
            m_readBits[19] = ReadBits19;
            m_readBits[20] = ReadBits20;
            m_readBits[21] = ReadBits21;
            m_readBits[22] = ReadBits22;
            m_readBits[23] = ReadBits23;
            m_readBits[24] = ReadBits24;
            m_readBits[25] = ReadBits25;
            m_readBits[26] = ReadBits26;
            m_readBits[27] = ReadBits27;
            m_readBits[28] = ReadBits28;
            m_readBits[29] = ReadBits29;
            m_readBits[30] = ReadBits30;
            m_readBits[31] = ReadBits31;
            m_readBits[32] = ReadBits32;
        }

        private byte ReadByte()
        {
            if (m_length == 0)
                throw new Exception("End of stream encoutered");
            m_length--;
            return m_buffer[m_position++];
        }

        public uint ReadBits(int bitCount)
        {
            return m_readBits[bitCount]();
        }

        public int RemainingBits => m_bitCount & 7;

        public byte PeakBits8()
        {
            const int bits = 8;
            if (m_bitCount < bits)
                InternalRead(bits);
            return (byte)((m_cache >> (m_bitCount - bits)) & ((1 << bits) - 1));
        }

        public uint ReadBits0()
        {
            return 0;
        }

        public uint ReadBits1()
        {
            const int bits = 1;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits2()
        {
            const int bits = 2;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits3()
        {
            const int bits = 3;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits4()
        {
            const int bits = 4;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits5()
        {
            const int bits = 5;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits6()
        {
            const int bits = 6;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits7()
        {
            const int bits = 7;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits8()
        {
            const int bits = 8;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits9()
        {
            const int bits = 9;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits10()
        {
            const int bits = 10;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits11()
        {
            const int bits = 11;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits12()
        {
            const int bits = 12;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits13()
        {
            const int bits = 13;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits14()
        {
            const int bits = 14;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits15()
        {
            const int bits = 15;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits16()
        {
            const int bits = 16;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits17()
        {
            const int bits = 17;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits18()
        {
            const int bits = 18;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits19()
        {
            const int bits = 19;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits20()
        {
            const int bits = 20;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits21()
        {
            const int bits = 21;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits22()
        {
            const int bits = 22;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits23()
        {
            const int bits = 23;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits24()
        {
            const int bits = 24;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits25()
        {
            const int bits = 25;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits26()
        {
            const int bits = 26;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits27()
        {
            const int bits = 27;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits28()
        {
            const int bits = 28;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits29()
        {
            const int bits = 29;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits30()
        {
            const int bits = 30;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits31()
        {
            const int bits = 31;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)((m_cache >> m_bitCount) & ((1u << bits) - 1));
        }

        public uint ReadBits32()
        {
            const int bits = 32;
            if (m_bitCount < bits)
                InternalRead(bits);
            m_bitCount -= bits;
            return (uint)(m_cache >> m_bitCount);
        }

        private void InternalRead(int bitsRequested)
        {
            while (m_bitCount < bitsRequested)
            {
                m_bitCount += 8;
                m_cache = (m_cache << 8) | ReadByte();
            }
        }

        public ulong Read8BitSegments(ByteReader reader)
        {
            ulong value = 0;
            int bits = 0;
            while (ReadBits1() == 1)
            {
                value = value | (((ulong)reader.ReadByte()) << bits);
                bits += 8;
            }
            return value;

        }
    }

}
