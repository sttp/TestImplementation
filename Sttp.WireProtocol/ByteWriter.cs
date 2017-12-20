using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public unsafe class ByteWriter
    {
        private byte[] m_byteBuffer;
        private byte[] m_bitBuffer;
        private int m_byteLength;
        private int m_bitStreamCacheBitCount;
        private uint m_bitStreamCache;
        private int m_bitLength;
        private bool m_hasReservedUnusedBitsHeader;

        public ByteWriter()
        {
            m_bitLength = 0;
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
            m_byteBuffer = new byte[64];
            m_bitBuffer = new byte[8];
            Clear();
        }

        public int Length => m_byteLength + m_bitLength + (m_bitStreamCacheBitCount + 7) >> 3;

        protected void GetBuffer(out byte[] data, out int offset, out int length)
        {
            //ToDo: Come back and fix this

            //Copy the bit stream to the end of the byte stream.

            //Flush any pending data to the stream, but don't reset any of the values since that would mess up the state if the 
            //user decides to continue to write data to the stream afterwords.
            int bitLength = m_bitLength;
            if (m_bitStreamCacheBitCount > 0)
            {
                EnsureCapacityBits(1);
                bitLength++;
                //Make up 8 bits by padding.
                uint cache = m_bitStreamCache << (8 - m_bitStreamCacheBitCount);
                m_bitBuffer[m_bitBuffer.Length - bitLength] = (byte)cache;
            }
            if (m_hasReservedUnusedBitsHeader)
            {
                m_bitBuffer[m_bitBuffer.Length - 1] &= 31; //Clear bits 6,7,8
                m_bitBuffer[m_bitBuffer.Length - 1] |= (byte)(m_bitStreamCache << 5);
            }
            EnsureCapacityBytes(m_bitLength);
            Array.Copy(m_bitBuffer, m_bitBuffer.Length - bitLength, m_byteBuffer, m_byteLength, bitLength);
            data = m_byteBuffer;
            offset = 0;
            length = m_byteLength + bitLength;
        }

        /// <summary>
        /// Ensures that the byte stream has the room to store the specified number of bytes.
        /// </summary>
        /// <param name="neededBytes"></param>
        private void EnsureCapacityBytes(int neededBytes)
        {
            if (m_byteLength + neededBytes >= m_byteBuffer.Length)
                GrowBytes(neededBytes);
        }

        private void GrowBytes(int neededBytes)
        {
            while (m_byteLength + neededBytes >= m_byteBuffer.Length)
            {
                byte[] newBuffer = new byte[m_byteBuffer.Length * 2];
                m_byteBuffer.CopyTo(newBuffer, 0);
                m_byteBuffer = newBuffer;
            }
        }

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
                m_bitBuffer.CopyTo(newBuffer, newBuffer.Length - m_bitBuffer.Length);
                m_bitBuffer = newBuffer;
            }
        }


        public byte[] ToArray()
        {
            //ToDo: There might be a better CopyTo alternative.
            GetBuffer(out byte[] origBuffer, out int offset, out int length);

            byte[] data = new byte[length];
            Array.Copy(origBuffer, offset, data, 0, length);
            return data;
        }

        public void Clear()
        {
            m_bitLength = 0;
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
            m_byteLength = 0;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }

        #region [ 1 byte values ]

        public void Write(byte value)
        {
            EnsureCapacityBytes(1);
            m_byteBuffer[m_byteLength] = value;
            m_byteLength++;
        }

        #endregion

        #region [ 2-byte values ]

        public void Write(short value)
        {
            EnsureCapacityBytes(2);
            m_byteBuffer[m_byteLength] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 1] = (byte)value;
            m_byteLength += 2;
        }

        #endregion

        #region [ 4-byte values ]

        public void Write(int value)
        {
            EnsureCapacityBytes(4);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 3] = (byte)value;
            m_byteLength += 4;
        }

        public void Write(uint value)
        {
            Write((int)value);
        }

        public void Write(float value)
        {
            Write(*(int*)&value);
        }

        #endregion

        #region [ 8-byte values ]

        public void Write(long value)
        {
            EnsureCapacityBytes(8);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 56);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 48);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 40);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 4] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 5] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 6] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 7] = (byte)value;
            m_byteLength += 8;
        }

        public void Write(ulong value)
        {
            Write((long)value);
        }

        public void Write(double value)
        {
            Write(*(long*)&value);
        }

        #endregion

        #region [ 16-byte values ]

        public void Write(decimal value)
        {
            EnsureCapacityBytes(16);
            m_byteLength += BigEndian.CopyBytes(value, m_byteBuffer, m_byteLength);
        }

        public void Write(Guid value)
        {
            EnsureCapacityBytes(16);
            Array.Copy(value.ToRfcBytes(), 0, m_byteBuffer, m_byteLength, 16);
            m_byteLength += 16;
        }

        #endregion

        #region [ Variable Length Types ]

        public void Write(byte[] value, long start, int length)
        {
            //ToDo: Rework this method.
            if (length > 1024 * 1024)
                throw new ArgumentException("Encoding more than 1MB is prohibited", nameof(length));

            // write null and empty
            EnsureCapacityBytes(1);

            if (value == null)
            {
                Write((byte)0);
                return;
            }
            if (value.Length == 0)
            {
                Write((byte)1);
                return;
            }

            EnsureCapacityBytes(Encoding7Bit.GetSize((uint)(length + 1)) + length);
            Write4BitSegments((uint)(length + 1));

            Array.Copy(value, start, m_byteBuffer, m_byteLength, length); // write data
            m_byteLength += length;
        }

        public void Write(byte[] value)
        {
            Write(value, 0, value?.Length ?? 0);
        }

        public void Write(string value)
        {
            //ToDo: Rework this method. Nulls should probably throw an exception

            EnsureCapacityBytes(1);

            if (value == null)
            {
                Write((byte)0);
                return;
            }
            if (value.Length == 0)
            {
                Write((byte)1);
                return;
            }

            Write(Encoding.UTF8.GetBytes(value));
        }

        #endregion

        public void Write(SttpTime value)
        {
            value.Save(this);
        }

        public void Write(SttpBuffer value)
        {
            value.Write(this);
        }

        public void Write(SttpMarkup value)
        {
            value.Write(this);
        }

        public void Write(SttpBulkTransport value)
        {
            value.Write(this);
        }


        #region [ Writing Bits ]

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
                WriteBits8((byte)value);
                value >>= 8;
                goto TryAgain;
            }
        }

        /// <summary>
        /// While (NotZero), WriteBits4
        /// </summary>
        /// <param name="value"></param>
        public void Write4BitSegments(ulong value)
        {
            TryAgain:
            if (value <= 0)
            {
                WriteBits1(0);
            }
            else
            {
                WriteBits1(1);
                WriteBits4((uint)value);
                value >>= 4;
                goto TryAgain;
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
            EnsureCapacityBytes(1);
            m_byteBuffer[m_byteLength + 0] = (byte)value;
            m_byteLength += 1;
        }
        public void WriteBits9(uint value)
        {
            WriteBits8(value);
            WriteBits1(value >> 8);
        }
        public void WriteBits10(uint value)
        {
            WriteBits8(value);
            WriteBits2(value >> 8);
        }
        public void WriteBits11(uint value)
        {
            WriteBits8(value);
            WriteBits3(value >> 8);
        }
        public void WriteBits12(uint value)
        {
            WriteBits8(value);
            WriteBits4(value >> 8);
        }
        public void WriteBits13(uint value)
        {
            WriteBits8(value);
            WriteBits5(value >> 8);
        }
        public void WriteBits14(uint value)
        {
            WriteBits8(value);
            WriteBits6(value >> 8);
        }
        public void WriteBits15(uint value)
        {
            WriteBits8(value);
            WriteBits7(value >> 8);
        }
        public void WriteBits16(uint value)
        {
            EnsureCapacityBytes(2);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 1] = (byte)value;
            m_byteLength += 2;
        }
        public void WriteBits17(uint value)
        {
            WriteBits16(value);
            WriteBits1(value >> 16);
        }
        public void WriteBits18(uint value)
        {
            WriteBits16(value);
            WriteBits2(value >> 16);
        }
        public void WriteBits19(uint value)
        {
            WriteBits16(value);
            WriteBits3(value >> 16);
        }
        public void WriteBits20(uint value)
        {
            WriteBits16(value);
            WriteBits4(value >> 16);
        }
        public void WriteBits21(uint value)
        {
            WriteBits16(value);
            WriteBits5(value >> 16);
        }
        public void WriteBits22(uint value)
        {
            WriteBits16(value);
            WriteBits6(value >> 16);
        }
        public void WriteBits23(uint value)
        {
            WriteBits16(value);
            WriteBits7(value >> 16);
        }
        public void WriteBits24(uint value)
        {
            EnsureCapacityBytes(3);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 2] = (byte)value;
            m_byteLength += 3;
        }
        public void WriteBits25(uint value)
        {
            WriteBits24(value);
            WriteBits1(value >> 24);
        }
        public void WriteBits26(uint value)
        {
            WriteBits24(value);
            WriteBits2(value >> 24);
        }
        public void WriteBits27(uint value)
        {
            WriteBits24(value);
            WriteBits3(value >> 24);
        }
        public void WriteBits28(uint value)
        {
            WriteBits24(value);
            WriteBits4(value >> 24);
        }
        public void WriteBits29(uint value)
        {
            WriteBits24(value);
            WriteBits5(value >> 24);
        }
        public void WriteBits30(uint value)
        {
            WriteBits24(value);
            WriteBits6(value >> 24);
        }
        public void WriteBits31(uint value)
        {
            WriteBits24(value);
            WriteBits7(value >> 24);
        }
        public void WriteBits32(uint value)
        {
            EnsureCapacityBytes(4);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 3] = (byte)value;
            m_byteLength += 4;
        }

        private void ValidateBitStream()
        {
            if (!m_hasReservedUnusedBitsHeader || m_bitStreamCacheBitCount > 7)
                ProcessBitStream();
        }

        private void ProcessBitStream()
        {
            if (!m_hasReservedUnusedBitsHeader)
            {
                //reserve 3 bits for the number of unused bits in the bitstream.
                //To reserve bits at the beginning of the bit stream, all we have to do is say there were 3 more bits.
                m_bitStreamCacheBitCount += 3;
                m_hasReservedUnusedBitsHeader = true;
            }

            EnsureCapacityBits(2); //It's ok to be too large here. It just ensures that at least 2 bytes are free before doing anything.
            while (m_bitStreamCacheBitCount > 7)
            {


                m_bitLength++;
                m_bitBuffer[m_bitBuffer.Length - m_bitLength] = (byte)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 8));
                m_bitStreamCacheBitCount -= 8;
            }
        }

        #endregion
    }
}

