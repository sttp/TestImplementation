using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CTP;
using GSF;

namespace CTP
{
    public unsafe class ByteWriter
    {
        private byte[] m_byteBuffer;
        private int m_byteLength;

        public ByteWriter()
        {
            m_byteBuffer = new byte[64];
            Clear();
        }

        public int Length => m_byteLength;
        
        /// <summary>
        /// Ensures that the byte stream has the room to store the specified number of bytes.
        /// </summary>
        /// <param name="neededBytes"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacityBytes(int neededBytes)
        {
            if (m_byteLength + neededBytes >= m_byteBuffer.Length)
                GrowBytes(neededBytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowBytes(int neededBytes)
        {
            while (m_byteLength + neededBytes >= m_byteBuffer.Length)
            {
                byte[] newBuffer = new byte[m_byteBuffer.Length * 2];
                m_byteBuffer.CopyTo(newBuffer, 0);
                m_byteBuffer = newBuffer;
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
            Array.Copy(m_byteBuffer, 0, data, offset, m_byteLength);
        }

        public void Clear()
        {
            m_byteLength = 0;
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
            Write7BitInt((uint)length);
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
                Write7BitInt(0);
                return;
            }

            Write(Encoding.UTF8.GetBytes(value));
        }

        public void Write7BitInt(uint value)
        {
            EnsureCapacityBytes(5);
            Encoding7Bit.Write(m_byteBuffer, ref m_byteLength, value);
        }

        #endregion

        #region [ Writing Bits ]

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

        #endregion


    }
}

