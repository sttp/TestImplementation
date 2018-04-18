using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace CTP
{
    internal unsafe class CtpDocumentBitReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;
        private int m_lastPosition;

        private int m_currentPosition;

        public CtpDocumentBitReader(byte[] data, int position, int length)
        {
            SetBuffer(data, position, length);
        }

        public bool IsEos => m_currentPosition == m_lastPosition;

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_lastPosition = length + position;
            m_currentPosition = position;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowEndOfStreamException()
        {
            throw new EndOfStreamException();
        }

        private void EnsureCapacity(int length)
        {
            if (m_currentPosition + length > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
        }

        public bool ReadBoolean()
        {
            return ReadBits8() != 0;
        }

        public float ReadSingle()
        {
            var value = ReadBits32();
            return *(float*)&value;
        }

        public long ReadInt64()
        {
            return (long)ReadBits64();
        }

        public double ReadDouble()
        {
            var value = ReadBits64();
            return *(double*)&value;
        }

        public Guid ReadGuid()
        {
            if (m_currentPosition + 16 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            Guid rv = m_buffer.ToRfcGuid(m_currentPosition);
            m_currentPosition += 16;
            return rv;
        }

        public byte[] ReadBytes()
        {
            int length = (int)Read7BitInt();
            if (length == 0)
            {
                return Empty;
            }

            EnsureCapacity(length);

            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentPosition, rv, 0, length);
            m_currentPosition += length;
            return rv;
        }

        public string ReadString()
        {
            byte[] rv = ReadBytes();
            if (rv.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(rv);
        }

        public string ReadAscii()
        {
            if (m_currentPosition + 1 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            if (m_currentPosition + 1 + m_buffer[m_currentPosition] > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            char[] data = new char[m_buffer[m_currentPosition]];
            for (int x = 0; x < data.Length; x++)
            {
                data[x] = (char)m_buffer[m_currentPosition + 1 + x];
                if (data[x] > 127)
                    throw new Exception("Not an ASCII string");
            }
            m_currentPosition += 1 + data.Length;
            return new string(data);
        }

        #region [ Read Bits ]

        public uint ReadBits8()
        {
            if (m_currentPosition + 1 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentPosition];
            m_currentPosition++;
            return rv;
        }

        public uint ReadBits16()
        {
            if (m_currentPosition + 2 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentPosition] << 8
                    | (uint)m_buffer[m_currentPosition + 1];
            m_currentPosition += 2;
            return rv;
        }

        public uint ReadBits32()
        {
            if (m_currentPosition + 4 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentPosition] << 24
                      | (uint)m_buffer[m_currentPosition + 1] << 16
                      | (uint)m_buffer[m_currentPosition + 2] << 8
                      | (uint)m_buffer[m_currentPosition + 3];
            m_currentPosition += 4;
            return rv;
        }

        public ulong ReadBits64()
        {
            if (m_currentPosition + 8 > m_lastPosition)
            {
                ThrowEndOfStreamException();
            }
            ulong rv = (ulong)m_buffer[m_currentPosition + 0] << 56 |
                      (ulong)m_buffer[m_currentPosition + 1] << 48 |
                      (ulong)m_buffer[m_currentPosition + 2] << 40 |
                      (ulong)m_buffer[m_currentPosition + 3] << 32 |
                      (ulong)m_buffer[m_currentPosition + 4] << 24 |
                      (ulong)m_buffer[m_currentPosition + 5] << 16 |
                      (ulong)m_buffer[m_currentPosition + 6] << 8 |
                      (ulong)m_buffer[m_currentPosition + 7];
            m_currentPosition += 8;
            return rv;
        }

        public ulong Read7BitInt()
        {
            ulong value = 0;
            int bitPosition = 0;
            while (bitPosition < 64)
            {
                byte code = (byte)ReadBits8();
                value |= (ulong)(code & 127) << bitPosition;
                bitPosition += 7;
                if ((code & 128) == 0)
                {
                    return value;
                }
            }
            throw new FormatException("Bad 7-bit int");
        }


        #endregion
    }
}
