using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CTP;
using GSF;

namespace CTP
{
    public unsafe class ByteReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;
        /// <summary>
        /// The position 1 beyond the last byte of the byte stream
        /// </summary>
        private int m_endOfByteStream;

        private int m_startBytePosition;

        private int m_currentBytePosition;

        public ByteReader()
        {
            m_buffer = Empty;
        }

        public bool IsEmpty
        {
            get
            {
                return m_currentBytePosition == m_endOfByteStream;
            }
        }

        public int Position
        {
            get
            {
                return m_currentBytePosition - m_startBytePosition;
            }
            set
            {
                value += m_startBytePosition;
                if (value < m_startBytePosition || value > m_endOfByteStream)
                    throw new ArgumentOutOfRangeException();
                m_currentBytePosition = value;
            }
        }

        public void SetBuffer(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_endOfByteStream = position + length;
            m_currentBytePosition = position;
            m_startBytePosition = position;
        }

        private void ThrowEndOfStreamException()
        {
            throw new EndOfStreamException();
        }

        private void EnsureCapacity(int length)
        {
            if (m_currentBytePosition + length > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
        }

        #region [ Read Methods ]

        #region [ 1 byte values ]

        public byte ReadByte()
        {
            return (byte)ReadBits8();
        }

        #endregion

        #region [ 2-byte values ]

        public short ReadInt16()
        {
            return (short)(ushort)ReadBits16();
        }

        #endregion

        #region [ 4-byte values ]

        public int ReadInt32()
        {
            return (int)ReadBits32();
        }

        public uint ReadUInt32()
        {
            return ReadBits32();
        }

        public float ReadSingle()
        {
            var value = ReadBits32();
            return *(float*)&value;
        }

        #endregion

        #region [ 8-byte values ]

        public long ReadInt64()
        {
            return (long)ReadBits64();
        }

        public double ReadDouble()
        {
            var value = ReadBits64();
            return *(double*)&value;
        }

        public ulong ReadUInt64()
        {
            return ReadBits64();
        }

        #endregion

        #region [ 16-byte values ]

        public Guid ReadGuid()
        {
            if (m_currentBytePosition + 16 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            Guid rv = m_buffer.ToRfcGuid(m_currentBytePosition);
            m_currentBytePosition += 16;
            return rv;
        }

        #endregion

        #region [ Variable Length ]

        public byte[] ReadBytes()
        {
            int length = (int)Read7BitInt();
            if (length == 0)
            {
                return Empty;
            }

            EnsureCapacity(length);

            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            return rv;
        }

        public string ReadString()
        {
            byte[] rv = ReadBytes();
            if (rv.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(rv);
        }

        #endregion

        #endregion

        #region [ Read Bits ]

        public uint ReadBits0()
        {
            return 0;
        }

        public uint ReadBits8()
        {
            if (m_currentBytePosition + 1 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentBytePosition];
            m_currentBytePosition++;
            return rv;
        }

        public uint ReadBits16()
        {
            if (m_currentBytePosition + 2 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 8
                    | (uint)m_buffer[m_currentBytePosition + 1];
            m_currentBytePosition += 2;
            return rv;
        }

        public uint ReadBits24()
        {
            if (m_currentBytePosition + 3 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 16
                      | (uint)m_buffer[m_currentBytePosition + 1] << 8
                      | (uint)m_buffer[m_currentBytePosition + 2];
            m_currentBytePosition += 3;
            return rv;
        }

        public uint ReadBits32()
        {
            if (m_currentBytePosition + 4 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 24
                      | (uint)m_buffer[m_currentBytePosition + 1] << 16
                      | (uint)m_buffer[m_currentBytePosition + 2] << 8
                      | (uint)m_buffer[m_currentBytePosition + 3];
            m_currentBytePosition += 4;
            return rv;
        }

        public ulong ReadBits40()
        {
            if (m_currentBytePosition + 5 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            ulong rv = (ulong)m_buffer[m_currentBytePosition + 0] << 32 |
                       (ulong)m_buffer[m_currentBytePosition + 1] << 24 |
                       (ulong)m_buffer[m_currentBytePosition + 2] << 16 |
                       (ulong)m_buffer[m_currentBytePosition + 3] << 8 |
                       (ulong)m_buffer[m_currentBytePosition + 4];
            m_currentBytePosition += 5;
            return rv;
        }

        public ulong ReadBits48()
        {
            if (m_currentBytePosition + 6 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            ulong rv = (ulong)m_buffer[m_currentBytePosition + 0] << 40 |
                       (ulong)m_buffer[m_currentBytePosition + 1] << 32 |
                       (ulong)m_buffer[m_currentBytePosition + 2] << 24 |
                       (ulong)m_buffer[m_currentBytePosition + 3] << 16 |
                       (ulong)m_buffer[m_currentBytePosition + 4] << 8 |
                       (ulong)m_buffer[m_currentBytePosition + 5];
            m_currentBytePosition += 6;
            return rv;
        }

        public ulong ReadBits56()
        {
            if (m_currentBytePosition + 7 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            ulong rv = (ulong)m_buffer[m_currentBytePosition + 0] << 48 |
                       (ulong)m_buffer[m_currentBytePosition + 1] << 40 |
                       (ulong)m_buffer[m_currentBytePosition + 2] << 32 |
                       (ulong)m_buffer[m_currentBytePosition + 3] << 24 |
                       (ulong)m_buffer[m_currentBytePosition + 4] << 16 |
                       (ulong)m_buffer[m_currentBytePosition + 5] << 8 |
                       (ulong)m_buffer[m_currentBytePosition + 6];
            m_currentBytePosition += 7;
            return rv;
        }

        public ulong ReadBits64()
        {
            if (m_currentBytePosition + 8 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            ulong rv = (ulong)m_buffer[m_currentBytePosition + 0] << 56 |
                      (ulong)m_buffer[m_currentBytePosition + 1] << 48 |
                      (ulong)m_buffer[m_currentBytePosition + 2] << 40 |
                      (ulong)m_buffer[m_currentBytePosition + 3] << 32 |
                      (ulong)m_buffer[m_currentBytePosition + 4] << 24 |
                      (ulong)m_buffer[m_currentBytePosition + 5] << 16 |
                      (ulong)m_buffer[m_currentBytePosition + 6] << 8 |
                      (ulong)m_buffer[m_currentBytePosition + 7];
            m_currentBytePosition += 8;
            return rv;
        }

        public uint Read7BitInt()
        {
            return Encoding7Bit.ReadUInt32(m_buffer, ref m_currentBytePosition);
        }


        #endregion

    }
}
