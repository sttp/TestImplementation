using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CTP
{
    /// <summary>
    /// This class is used to write a CtpDocument.
    /// </summary>
    internal unsafe class CtpDocumentBitWriter
    {
        private byte[] m_buffer;
        private int m_length;

        public CtpDocumentBitWriter()
        {
            m_buffer = new byte[64];
            Clear();
        }

        public int Length => m_length;

        /// <summary>
        /// Ensures that the byte stream has the room to store the specified number of bytes.
        /// </summary>
        /// <param name="neededBytes"></param>
        private void EnsureCapacityBytes(int neededBytes)
        {
            if (m_length + neededBytes >= m_buffer.Length)
                GrowBytes(neededBytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowBytes(int neededBytes)
        {
            while (m_length + neededBytes >= m_buffer.Length)
            {
                byte[] newBuffer = new byte[m_buffer.Length * 2];
                m_buffer.CopyTo(newBuffer, 0);
                m_buffer = newBuffer;
            }
        }

        public void Clear()
        {
            m_length = 0;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
        }

        public void Write(bool value)
        {
            WriteBits8(value ? 1u : 0u);
        }

        public void Write(float value)
        {
            WriteBits32(*(uint*)&value);
        }

        public void Write(long value)
        {
            WriteBits64((ulong)value);
        }

        public void Write(double value)
        {
            WriteBits64(*(ulong*)&value);
        }

        public void Write(Guid value)
        {
            EnsureCapacityBytes(16);
            value.ToRfcBytes(m_buffer, m_length);
            m_length += 16;
        }

        public void Write(byte[] value, int start, int length)
        {
            value.ValidateParameters(start, length);
            Write7BitInt((uint)length);
            if (length == 0)
                return;

            EnsureCapacityBytes(length);
            Array.Copy(value, start, m_buffer, m_length, length); // write data
            m_length += length;
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

        #region [ Writing Bits ]

        /// <summary>
        /// 7-bit encoded int
        /// </summary>
        /// <param name="value"></param>
        public void Write7BitInt(ulong value)
        {
            while (value >= 128)
            {
                WriteBits8((byte)(value | 128u));
                value >>= 7;
            }
            WriteBits8((byte)value);
        }

        public void WriteBits8(uint value)
        {
            EnsureCapacityBytes(1);
            m_buffer[m_length + 0] = (byte)value;
            m_length += 1;
        }

        private void WriteBits32(uint value)
        {
            EnsureCapacityBytes(4);
            m_buffer[m_length + 0] = (byte)(value >> 24);
            m_buffer[m_length + 1] = (byte)(value >> 16);
            m_buffer[m_length + 2] = (byte)(value >> 8);
            m_buffer[m_length + 3] = (byte)value;
            m_length += 4;
        }

        private void WriteBits64(ulong value)
        {
            EnsureCapacityBytes(8);
            m_buffer[m_length + 0] = (byte)(value >> 56);
            m_buffer[m_length + 1] = (byte)(value >> 48);
            m_buffer[m_length + 2] = (byte)(value >> 40);
            m_buffer[m_length + 3] = (byte)(value >> 32);
            m_buffer[m_length + 4] = (byte)(value >> 24);
            m_buffer[m_length + 5] = (byte)(value >> 16);
            m_buffer[m_length + 6] = (byte)(value >> 8);
            m_buffer[m_length + 7] = (byte)value;
            m_length += 8;
        }

        public void CopyTo(byte[] rv, int index)
        {
            Array.Copy(m_buffer, 0, rv, index, m_length);
        }

        internal void Write(CtpDocument value)
        {
            Write7BitInt((uint)value.Length);
            EnsureCapacityBytes(value.Length);
            value.CopyTo(m_buffer,m_length);
            m_length += value.Length;
        }

        internal void Write(CtpBuffer value)
        {
            Write7BitInt((uint)value.Length);
            EnsureCapacityBytes(value.Length);
            value.CopyTo(m_buffer, m_length);
            m_length += value.Length;
        }


        #endregion


    }
}

