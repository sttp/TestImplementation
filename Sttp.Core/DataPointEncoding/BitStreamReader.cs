using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CTP;
using GSF;

namespace CTP
{
    public unsafe class BitStreamReader
    {
        private static readonly byte[] Empty = new byte[0];
        private static readonly Func<BitStreamReader, uint>[] Read;
        private static uint ReadBits0(BitStreamReader reader) { return reader.ReadBits0(); }
        private static uint ReadBits1(BitStreamReader reader) { return reader.ReadBits1(); }
        private static uint ReadBits2(BitStreamReader reader) { return reader.ReadBits2(); }
        private static uint ReadBits3(BitStreamReader reader) { return reader.ReadBits3(); }
        private static uint ReadBits4(BitStreamReader reader) { return reader.ReadBits4(); }
        private static uint ReadBits5(BitStreamReader reader) { return reader.ReadBits5(); }
        private static uint ReadBits6(BitStreamReader reader) { return reader.ReadBits6(); }
        private static uint ReadBits7(BitStreamReader reader) { return reader.ReadBits7(); }
        private static uint ReadBits8(BitStreamReader reader) { return reader.ReadBits8(); }
        private static uint ReadBits9(BitStreamReader reader) { return reader.ReadBits9(); }
        private static uint ReadBits10(BitStreamReader reader) { return reader.ReadBits10(); }
        private static uint ReadBits11(BitStreamReader reader) { return reader.ReadBits11(); }
        private static uint ReadBits12(BitStreamReader reader) { return reader.ReadBits12(); }
        private static uint ReadBits13(BitStreamReader reader) { return reader.ReadBits13(); }
        private static uint ReadBits14(BitStreamReader reader) { return reader.ReadBits14(); }
        private static uint ReadBits15(BitStreamReader reader) { return reader.ReadBits15(); }
        private static uint ReadBits16(BitStreamReader reader) { return reader.ReadBits16(); }
        private static uint ReadBits17(BitStreamReader reader) { return reader.ReadBits17(); }
        private static uint ReadBits18(BitStreamReader reader) { return reader.ReadBits18(); }
        private static uint ReadBits19(BitStreamReader reader) { return reader.ReadBits19(); }
        private static uint ReadBits20(BitStreamReader reader) { return reader.ReadBits20(); }
        private static uint ReadBits21(BitStreamReader reader) { return reader.ReadBits21(); }
        private static uint ReadBits22(BitStreamReader reader) { return reader.ReadBits22(); }
        private static uint ReadBits23(BitStreamReader reader) { return reader.ReadBits23(); }
        private static uint ReadBits24(BitStreamReader reader) { return reader.ReadBits24(); }
        private static uint ReadBits25(BitStreamReader reader) { return reader.ReadBits25(); }
        private static uint ReadBits26(BitStreamReader reader) { return reader.ReadBits26(); }
        private static uint ReadBits27(BitStreamReader reader) { return reader.ReadBits27(); }
        private static uint ReadBits28(BitStreamReader reader) { return reader.ReadBits28(); }
        private static uint ReadBits29(BitStreamReader reader) { return reader.ReadBits29(); }
        private static uint ReadBits30(BitStreamReader reader) { return reader.ReadBits30(); }
        private static uint ReadBits31(BitStreamReader reader) { return reader.ReadBits31(); }
        private static uint ReadBits32(BitStreamReader reader) { return reader.ReadBits32(); }

        static BitStreamReader()
        {
            Read = new Func<BitStreamReader, uint>[33];
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

        private byte[] m_buffer;
        /// <summary>
        /// The position 1 beyond the last byte of the bit stream
        /// </summary>
        private int m_endOfBitStream;

        private int m_currentBitPosition;

        private int m_bitStreamCacheBitCount;
        private ulong m_bitStreamCache;
        private int m_bitsInLastByte;

        public BitStreamReader()
        {
            m_buffer = Empty;
        }
        public BitStreamReader(byte[] buffer)
        {
            SetBuffer(buffer);
        }

        public bool IsEmpty
        {
            get
            {
                return m_currentBitPosition == m_endOfBitStream && m_bitStreamCacheBitCount == 0;
            }
        }

        public void SetBuffer(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_endOfBitStream = position + length;
            m_currentBitPosition = position;
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;

            m_bitsInLastByte = m_buffer[position] >> 5;
            if (m_bitsInLastByte == 0)
                m_bitsInLastByte = 8;

            //Advance the pointer 3 bits. This cannot be moved above since this method relies on m_bitsInLastByte to be properly set.
            ReadBits3();
        }

        public ulong Read8BitSegments()
        {
            int bits = 0;
            while (ReadBits1() == 1)
            {
                bits += 8;
            }
            return ReadBits(bits);
        }

        public ulong Read4BitSegments()
        {
            int bits = 0;
            while (ReadBits1() == 1)
            {
                bits += 4;
            }
            return ReadBits(bits);
        }

        public ulong ReadBits(int bits)
        {
            if (bits > 64 || bits < 0)
                throw new ArgumentOutOfRangeException(nameof(bits), "Must be between 0 and 64 inclusive");

            if (bits > 32)
            {
                ulong upper = (ulong)Read[bits - 32](this) << 32;
                return upper | ReadBits32(this);
            }
            else
            {
                return Read[bits](this);
            }
        }

        #region [ Read Bits ]

        public uint ReadBits0()
        {
            return 0;
        }

        public uint ReadBits1()
        {
            const int bits = 1;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits2()
        {
            const int bits = 2;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits3()
        {
            const int bits = 3;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits4()
        {
            const int bits = 4;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits5()
        {
            const int bits = 5;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits6()
        {
            const int bits = 6;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits7()
        {
            const int bits = 7;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits8()
        {
            const int bits = 8;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits9()
        {
            const int bits = 9;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits10()
        {
            const int bits = 10;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits11()
        {
            const int bits = 11;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits12()
        {
            const int bits = 12;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits13()
        {
            const int bits = 13;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits14()
        {
            const int bits = 14;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits15()
        {
            const int bits = 15;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits16()
        {
            const int bits = 16;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits17()
        {
            const int bits = 17;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits18()
        {
            const int bits = 18;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits19()
        {
            const int bits = 19;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits20()
        {
            const int bits = 20;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits21()
        {
            const int bits = 21;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits22()
        {
            const int bits = 22;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits23()
        {
            const int bits = 23;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits24()
        {
            const int bits = 24;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits25()
        {
            const int bits = 25;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits26()
        {
            const int bits = 26;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits27()
        {
            const int bits = 27;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits28()
        {
            const int bits = 28;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits29()
        {
            const int bits = 29;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits30()
        {
            const int bits = 30;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits31()
        {
            const int bits = 31;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1u << bits) - 1));
        }
        public uint ReadBits32()
        {
            const int bits = 32;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)(m_bitStreamCache >> m_bitStreamCacheBitCount);
        }

        private void ReadMoreBits(int requiredBits)
        {
            while (m_bitStreamCacheBitCount < 50 && m_currentBitPosition != m_endOfBitStream)
            {
                if (m_currentBitPosition + 1 == m_endOfBitStream)
                {
                    m_bitStreamCacheBitCount += m_bitsInLastByte;
                    m_bitStreamCache = (m_bitStreamCache << m_bitsInLastByte) | ((uint)m_buffer[m_currentBitPosition] >> (8 - m_bitsInLastByte));
                }
                else
                {
                    m_bitStreamCacheBitCount += 8;
                    m_bitStreamCache = (m_bitStreamCache << 8) | m_buffer[m_currentBitPosition];
                }
                m_currentBitPosition++;
            }

            if (m_bitStreamCacheBitCount < requiredBits)
                throw new EndOfStreamException();
        }



        #endregion

    }
}
