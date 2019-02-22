using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CTP;
using GSF;

namespace CTP
{
    public class BitStreamWriter
    {
        private static readonly Action<BitStreamWriter, uint>[] Write;

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

        static BitStreamWriter()
        {
            Write = new Action<BitStreamWriter, uint>[33];
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

        private byte[] m_bitBuffer;

        private int m_bitLength;

        /// <summary>
        /// The number of valid bits in <see cref="m_bitStreamCache"/>
        /// </summary>
        private int m_bitStreamCacheBitCount;
        /// <summary>
        /// A pending place to store bits. Bits are stored right aligned and appended on the right.
        /// </summary>
        private ulong m_bitStreamCache;

        public BitStreamWriter()
        {
            m_bitBuffer = new byte[8];
            Clear();
        }

        public int Length => m_bitLength + ((m_bitStreamCacheBitCount + 7) >> 3);

        /// <summary>
        /// Ensures that the byte stream has the room to store the specified number of bytes.
        /// </summary>
        /// <param name="neededBytes"></param>
        private void EnsureCapacityBits(int neededBytes)
        {
            if (m_bitLength + neededBytes >= m_bitBuffer.Length)
                GrowBits(neededBytes);
        }

        private void GrowBits(int neededBytes)
        {
            while (m_bitLength + neededBytes >= m_bitBuffer.Length)
            {
                byte[] newBuffer = new byte[m_bitBuffer.Length * 2];
                m_bitBuffer.CopyTo(newBuffer, 0);
                m_bitBuffer = newBuffer;
            }
        }

        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            CopyTo(data, 0);
            return data;
        }

        public void CopyTo(byte[] data, int offset)
        {
            int length = Length;
            FlushBitStream(); //This means only what is in the bit cache remains and is 0 to 7 bits.
            if (m_bitStreamCacheBitCount > 0)
            {
                //The final byte is right 0 padded.
                EnsureCapacityBits(1);
                m_bitBuffer[length - 1] = (byte)(m_bitStreamCache << (8 - m_bitStreamCacheBitCount));
            }
            m_bitBuffer[0] = (byte)((m_bitBuffer[0] & 31) + (m_bitStreamCacheBitCount * 32));
            Array.Copy(m_bitBuffer, 0, data, offset, length);
        }

        public void Clear()
        {
            m_bitLength = 0;
            m_bitStreamCacheBitCount = 3; //The upper 3 bits are reserved to indicate how many bits of the last byte are used.
            m_bitStreamCache = 0;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }

        #region [ Writing Bits ]

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

        public void WriteBits(int bits, uint value)
        {
            if (bits > 32 || bits < 0)
                throw new ArgumentOutOfRangeException(nameof(bits), "Must be between 0 and 32 inclusive");

            Write[bits](this, value);
        }

        public void WriteBits(int bits, ulong value)
        {
            if (bits > 64 || bits < 0)
                throw new ArgumentOutOfRangeException(nameof(bits), "Must be between 0 and 64 inclusive");

            if (bits > 32)
            {
                Write[bits - 32](this, (uint)(value >> 32));
                WriteBits32(this, (uint)value);
            }
            else
            {
                Write[bits](this, (uint)value);
            }
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
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits2(uint value)
        {
            const int bits = 2;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits3(uint value)
        {
            const int bits = 3;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits4(uint value)
        {
            const int bits = 4;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits5(uint value)
        {
            const int bits = 5;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits6(uint value)
        {
            const int bits = 6;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits7(uint value)
        {
            const int bits = 7;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits8(uint value)
        {
            const int bits = 8;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }
        public void WriteBits9(uint value)
        {
            const int bits = 9;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }
        public void WriteBits10(uint value)
        {
            const int bits = 10;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits11(uint value)
        {
            const int bits = 11;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits12(uint value)
        {
            const int bits = 12;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits13(uint value)
        {
            const int bits = 13;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits14(uint value)
        {
            const int bits = 14;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits15(uint value)
        {
            const int bits = 15;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits16(uint value)
        {
            const int bits = 16;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits17(uint value)
        {
            const int bits = 17;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits18(uint value)
        {
            const int bits = 18;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }
        public void WriteBits19(uint value)
        {
            const int bits = 19;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }
        public void WriteBits20(uint value)
        {
            const int bits = 20;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }
        public void WriteBits21(uint value)
        {
            const int bits = 21;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits22(uint value)
        {
            const int bits = 22;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits23(uint value)
        {
            const int bits = 23;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits24(uint value)
        {
            const int bits = 24;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits25(uint value)
        {
            const int bits = 25;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits26(uint value)
        {
            const int bits = 26;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits27(uint value)
        {
            const int bits = 27;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits28(uint value)
        {
            const int bits = 28;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }
        public void WriteBits29(uint value)
        {
            const int bits = 29;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits30(uint value)
        {
            const int bits = 30;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1 << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits31(uint value)
        {
            const int bits = 31;
            m_bitStreamCache = (m_bitStreamCache << bits) | (value & ((1u << bits) - 1));
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        public void WriteBits32(uint value)
        {
            const int bits = 32;
            m_bitStreamCache = (m_bitStreamCache << bits) | value;
            m_bitStreamCacheBitCount += bits;
            ValidateBitStream();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidateBitStream()
        {
            if (m_bitStreamCacheBitCount > 30)
                FlushBitStream();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void FlushBitStream()
        {
            EnsureCapacityBits(8); //It's ok to be too large here. It just ensures that at least 4 bytes are free before doing anything.
            while (m_bitStreamCacheBitCount > 7)
            {
                m_bitBuffer[m_bitLength] = (byte)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 8));
                m_bitLength++;
                m_bitStreamCacheBitCount -= 8;
            }
        }

        #endregion


    }
}

