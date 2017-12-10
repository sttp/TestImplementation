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

        public Func<uint>[] Read;

        protected BitStreamReader()
        {
            m_bitCount = 0;
            m_cache = 0;
            Read = new Func<uint>[33];
            Read[0] = ReadBits0;
            Read[1] = ReadBits1;
            Read[2] = ReadBits2;
            Read[3] = ReadBits3;
            Read[4] = ReadBits4;
            Read[5] = ReadBits5;
            Read[6] = ReadBits6;
            Read[7] = ReadBits7;
            Read[8] = ReadBits8;
            Read[9] = ReadBits9;
            Read[10] = ReadBits10;
            Read[11] = ReadBits11;
            Read[12] = ReadBits12;
            Read[13] = ReadBits13;
            Read[14] = ReadBits14;
            Read[15] = ReadBits15;
            Read[16] = ReadBits16;
            Read[17] = ReadBits17;
            Read[18] = ReadBits18;
            Read[19] = ReadBits19;
            Read[20] = ReadBits20;
            Read[21] = ReadBits21;
            Read[22] = ReadBits22;
            Read[23] = ReadBits23;
            Read[24] = ReadBits24;
            Read[25] = ReadBits25;
            Read[26] = ReadBits26;
            Read[27] = ReadBits27;
            Read[28] = ReadBits28;
            Read[29] = ReadBits29;
            Read[30] = ReadBits30;
            Read[31] = ReadBits31;
            Read[32] = ReadBits32;
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

        protected virtual void InternalRead(int bitsRequested)
        {
            while (m_bitCount < bitsRequested)
            {
                m_bitCount += 8;
                m_cache = (m_cache << 8) | ReadByte();
            }
        }

        private byte ReadByte()
        {
            return 0;
        }

        public ulong Read8BitSegments()
        {
            ulong value = 0;
            int bits = 0;
            while (ReadBits1() == 1)
            {
                value = value | (((ulong)ReadBits8()) << bits);
                bits += 8;
            }
            return value;

        }
    }

}
