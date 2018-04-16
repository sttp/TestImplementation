using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CTP
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
            m_byteBuffer = new byte[64];
            m_bitBuffer = new byte[8];
            Clear();
        }

        public int Length => m_byteLength + m_bitLength + ((m_bitStreamCacheBitCount + 7) >> 3);

        protected void GetBuffer(out byte[] data, out int offset, out int length)
        {
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
                m_bitBuffer[m_bitBuffer.Length - 1] |= (byte)(m_bitStreamCacheBitCount << 5);
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
            m_hasReservedUnusedBitsHeader = false;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }

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
            EnsureCapacityBytes(16);
            Array.Copy(value.ToRfcBytes(), 0, m_byteBuffer, m_byteLength, 16);
            m_byteLength += 16;
        }

        #endregion

        #region [ Variable Length Types ]

        public void Write(byte[] value, int start, int length)
        {
            value.ValidateParameters(start, length);
            Write4BitSegments((uint)length);
            if (length == 0)
                return;

            EnsureCapacityBytes(length);
            Array.Copy(value, start, m_byteBuffer, m_byteLength, length); // write data
            m_byteLength += length;
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

        public void WriteAsciiShort(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value.Length > 255)
                throw new Exception("Length of string cannot exceed 255 characters");

            EnsureCapacityBytes(value.Length + 1);

            m_byteBuffer[m_byteLength + 0] = (byte)value.Length;

            for (var x = 0; x < value.Length; x++)
            {
                if ((ushort)value[x] > 127) //casting to ushort also takes care of negative numbers if they exist.
                    throw new Exception("Not an ASCII string");
                m_byteBuffer[m_byteLength + 1 + x] = (byte)value[x];
            }

            m_byteLength += 1 + value.Length;
        }

        #endregion

        public void Write(CtpTime value)
        {
            Write(value.Ticks);
        }

        public void Write(CtpBuffer value)
        {
            value.Write(this);
        }

        public void Write(CtpDocument value)
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

        private void WriteBits(int bits, ulong value)
        {
            if (bits > 64 || bits < 0)
                throw new ArgumentOutOfRangeException(nameof(bits), "Must be between 0 and 64 inclusive");

            //Since the lowest order bits are most chaotic, these should be stored in the bit stream.

            switch (bits & 7)
            {
                case 0:
                    break;
                case 1:
                    WriteBits1((uint)value);
                    break;
                case 2:
                    WriteBits2((uint)value);
                    break;
                case 3:
                    WriteBits3((uint)value);
                    break;
                case 4:
                    WriteBits4((uint)value);
                    break;
                case 5:
                    WriteBits5((uint)value);
                    break;
                case 6:
                    WriteBits6((uint)value);
                    break;
                case 7:
                    WriteBits7((uint)value);
                    break;
            }

            value >>= bits & 7;

            switch (bits >> 3)
            {
                case 0:
                    return;
                case 1:
                    WriteBits8((uint)value);
                    return;
                case 2:
                    WriteBits16((uint)value);
                    return;
                case 3:
                    WriteBits24((uint)value);
                    return;
                case 4:
                    WriteBits32((uint)value);
                    return;
                case 5:
                    WriteBits40(value);
                    return;
                case 6:
                    WriteBits48(value);
                    return;
                case 7:
                    WriteBits56(value);
                    return;
                case 8:
                    WriteBits64(value);
                    return;
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
        public void WriteBits16(uint value)
        {
            EnsureCapacityBytes(2);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 1] = (byte)value;
            m_byteLength += 2;
        }
        public void WriteBits24(uint value)
        {
            EnsureCapacityBytes(3);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 2] = (byte)value;
            m_byteLength += 3;
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
        public void WriteBits40(ulong value)
        {
            EnsureCapacityBytes(5);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 4] = (byte)value;
            m_byteLength += 5;
        }
        public void WriteBits48(ulong value)
        {
            EnsureCapacityBytes(6);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 40);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 4] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 5] = (byte)value;
            m_byteLength += 6;
        }
        public void WriteBits56(ulong value)
        {
            EnsureCapacityBytes(7);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 48);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 40);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 4] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 5] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 6] = (byte)value;
            m_byteLength += 7;
        }
        public void WriteBits64(ulong value)
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

