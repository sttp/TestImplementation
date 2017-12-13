using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public unsafe class ByteWriter
    {
        private int m_reservedPrfixBytes;
        private byte[] m_buffer;
        private int m_position;

        private int m_bitStreamCacheBitCount;
        private uint m_bitStreamCache;
        private int m_positionOfReservedBits;

        public ByteWriter()
        {
            m_positionOfReservedBits = -1;
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
            m_reservedPrfixBytes = 0;
            m_buffer = new byte[64];
            Clear();
        }

        protected ByteWriter(int reservedPrefixBytes)
        {
            m_reservedPrfixBytes = reservedPrefixBytes;
            m_positionOfReservedBits = -1;
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
            m_buffer = new byte[64];
            Clear();
        }

        public int Length => m_position - m_reservedPrfixBytes;

        protected void GetBuffer(out byte[] data, out int offset, out int length)
        {
            //Flush any pending data to the stream, but don't reset any of the values since that would mess up the state if the 
            //user decides to continue to write data to the stream afterwords.
            if (m_bitStreamCacheBitCount > 0)
            {
                //Make up 8 bits by padding.
                uint cache = m_bitStreamCache << (8 - m_bitStreamCacheBitCount);
                m_buffer[m_positionOfReservedBits] = (byte)cache;
            }
            data = m_buffer;
            offset = m_reservedPrfixBytes;
            length = Length;
        }

        private void EnsureCapacity(int neededBytes)
        {
            if (m_position + neededBytes >= m_buffer.Length)
                Grow(neededBytes);
        }

        private void Grow(int neededBytes)
        {
            while (m_position + neededBytes >= m_buffer.Length)
            {
                byte[] newBuffer = new byte[m_buffer.Length * 2];
                m_buffer.CopyTo(newBuffer, 0);
                m_buffer = newBuffer;
            }
        }

        public byte[] ToArray()
        {
            GetBuffer(out byte[] origBuffer, out int offset, out int length);

            byte[] data = new byte[length];
            Array.Copy(origBuffer, offset, data, 0, length);
            return data;
        }

        public void Clear()
        {
            m_positionOfReservedBits = -1;
            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
            m_position = m_reservedPrfixBytes;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }

        #region [ 1 byte values ]

        public void Write(byte value)
        {
            EnsureCapacity(1);
            m_buffer[m_position] = value;
            m_position++;
        }

        public void Write(bool value)
        {
            if (value)
                Write((byte)1);
            else
                Write((byte)0);
        }

        public void Write(sbyte value)
        {
            Write((byte)value);
        }

        #endregion

        #region [ 2-byte values ]

        public void Write(short value)
        {
            EnsureCapacity(2);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(ushort value)
        {
            Write((short)value);
        }

        public void Write(char value)
        {
            Write((short)value);
        }

        #endregion

        #region [ 4-byte values ]

        public void Write(int value)
        {
            EnsureCapacity(4);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
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
            EnsureCapacity(8);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(ulong value)
        {
            Write((long)value);
        }

        public void Write(double value)
        {
            Write(*(long*)&value);
        }

        public void Write(DateTime value)
        {
            Write(value.Ticks);
        }

        #endregion

        #region [ 16-byte values ]

        public void Write(decimal value)
        {
            EnsureCapacity(16);
            m_position += BigEndian.CopyBytes(value, m_buffer, m_position);
        }

        public void Write(Guid value)
        {
            EnsureCapacity(16);
            Array.Copy(value.ToRfcBytes(), 0, m_buffer, m_position, 16);
            m_position += 16;
        }

        #endregion

        #region [ Variable Length Types ]

        public void Write(Stream stream, long start, int length)
        {
            if (length > 1024 * 1024)
                throw new ArgumentException("Encoding more than 1MB is prohibited", nameof(length));

            // write null and empty
            EnsureCapacity(1);

            if (stream == null)
            {
                Write((byte)0);
                return;
            }
            if (length == 0)
            {
                Write((byte)1);
                return;
            }

            EnsureCapacity(Encoding7Bit.GetSize((uint)(length + 1)) + length);
            WriteUInt7Bit((uint)(length + 1));

            stream.Read(m_buffer, m_position, length);
            m_position += length;
        }

        public void WriteWithoutLength(byte[] value, int start, int length)
        {
            value.ValidateParameters(start, length);
            EnsureCapacity(length);
            Array.Copy(value, start, m_buffer, m_position, length); // write data
            m_position += length;
        }

        public void Write(byte[] value, long start, int length)
        {
            if (length > 1024 * 1024)
                throw new ArgumentException("Encoding more than 1MB is prohibited", nameof(length));

            // write null and empty
            EnsureCapacity(1);

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

            EnsureCapacity(Encoding7Bit.GetSize((uint)(length + 1)) + length);
            WriteUInt7Bit((uint)(length + 1));

            Array.Copy(value, start, m_buffer, m_position, length); // write data
            m_position += length;
        }

        public void Write(byte[] value)
        {
            Write(value, 0, value?.Length ?? 0);
        }

        public void Write(string value)
        {
            EnsureCapacity(1);

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

        #region [ Write Bits ]

        public void WriteInt7Bit(int value)
        {
            WriteUInt7Bit((uint)value);
        }

        public void WriteInt7Bit(long value)
        {
            WriteUInt7Bit((ulong)value);
        }



        public void WriteUInt7Bit(uint value)
        {
            int size = Encoding7Bit.GetSize(value);
            EnsureCapacity(size);

            // position is incremented within method.
            Encoding7Bit.Write(m_buffer, ref m_position, value);
        }
        public void WriteUInt7Bit(ulong value)
        {
            int size = Encoding7Bit.GetSize(value);
            EnsureCapacity(size);

            // position is incremented within method.
            Encoding7Bit.Write(m_buffer, ref m_position, value);
        }

        #endregion

        public void Write(List<MetadataColumn> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<MetadataForeignKey> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }



        public void Write(List<SttpDataPointID> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<MetadataSchemaTables> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(List<MetadataSchemaTableUpdate> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(SttpValue value)
        {
            value.Save(this);
        }

        public void Write(SttpTime value)
        {
            value.Save(this);
        }

        public void Write(List<int> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                Write(list[x]);
            }
        }

        public void Write(List<SttpValue> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                list[x].Save(this);
            }
        }

        public void Write(int? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
            {
                Write(value.Value);
            }

        }

        public void Write(List<string> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                Write(list[x]);
            }
        }

        public void Write(SttpMarkup value)
        {
            value.Write(this);
        }

        public void Write(List<SttpMarkup> list)
        {
            if (list == null)
            {
                Write((byte)0);
                return;
            }
            WriteInt7Bit(list.Count);
            for (var x = 0; x < list.Count; x++)
            {
                Write(list[x]);
            }
        }

        public void Write(SttpBulkTransport value)
        {
            throw new NotImplementedException();
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
            EnsureCapacity(1);
            m_buffer[m_position + 0] = (byte)value;
            m_position += 1;
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
            EnsureCapacity(2);
            m_buffer[m_position + 0] = (byte)(value >> 8);
            m_buffer[m_position + 1] = (byte)value;
            m_position += 2;
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
            EnsureCapacity(3);
            m_buffer[m_position + 0] = (byte)(value >> 16);
            m_buffer[m_position + 1] = (byte)(value >> 8);
            m_buffer[m_position + 2] = (byte)value;
            m_position += 3;
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
            EnsureCapacity(4);
            m_buffer[m_position + 0] = (byte)(value >> 24);
            m_buffer[m_position + 1] = (byte)(value >> 16);
            m_buffer[m_position + 2] = (byte)(value >> 8);
            m_buffer[m_position + 3] = (byte)value;
            m_position += 4;
        }



        private void ValidateBitStream()
        {
            if (m_bitStreamCacheBitCount > 7 || m_positionOfReservedBits < 0)
                ProcessBitStream();
        }

        private void ProcessBitStream()
        {
            EnsureCapacity(8); //It's ok to be too large here. It just ensures that at least 8 bytes are free before doing anything.

            //Reserve an 8 byte boundary if data has been written 
            if (m_positionOfReservedBits < 0)
                m_positionOfReservedBits = m_position++;

            while (m_bitStreamCacheBitCount > 7)
            {
                m_buffer[m_positionOfReservedBits++] = (byte)(m_bitStreamCache >> (m_bitStreamCacheBitCount - 8));
                m_bitStreamCacheBitCount -= 8;

                if (m_bitStreamCacheBitCount == 0)
                    m_positionOfReservedBits = -1;
                else
                    m_positionOfReservedBits = m_position++;
            }
        }

        #endregion
    }
}

