﻿using System;
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

        public ByteWriter()
        {
            m_reservedPrfixBytes = 0;
            m_buffer = new byte[64];
            Clear();
        }

        protected ByteWriter(int reservedPrefixBytes)
        {
            m_reservedPrfixBytes = reservedPrefixBytes;
            m_buffer = new byte[64];
            Clear();
        }


        public int Length => m_position - m_reservedPrfixBytes;

        protected void GetBounds(out byte[] data, out int offset, out int length)
        {
            data = m_buffer;
            offset = m_reservedPrfixBytes;
            length = Length;
        }

        private void EnsureCapacity(int neededBytes)
        {
            if (m_position + neededBytes >= m_buffer.Length)
                Grow2(neededBytes);
        }

        private void Grow2(int neededBytes)
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
            int length = Length;
            int offset = m_reservedPrfixBytes;

            byte[] data = new byte[length];
            Array.Copy(m_buffer, offset, data, 0, length);
            return data;
        }

        public void Clear()
        {
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
    }
}
