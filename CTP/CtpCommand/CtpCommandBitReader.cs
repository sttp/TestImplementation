using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using GSF;

namespace CTP
{
    internal unsafe class CtpCommandBitReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;
        private int m_length;
        private int m_position;

        public CtpCommandBitReader(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_length = length + position;
            m_position = position;
        }

        public bool IsEos => m_position == m_length;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowEndOfStreamException()
        {
            throw new EndOfStreamException();
        }

        public float ReadSingle()
        {
            var value = ReadBits32();
            return *(float*)&value;
        }

        public double ReadDouble()
        {
            var value = ReadBits64();
            return *(double*)&value;
        }

        public Guid ReadGuid()
        {
            if (m_position + 16 > m_length)
            {
                ThrowEndOfStreamException();
            }
            Guid rv = m_buffer.ToRfcGuid(m_position);
            m_position += 16;
            return rv;
        }

        private byte[] ReadBytes()
        {
            int length = (int)Read7BitInt();
            if (length == 0)
            {
                return Empty;
            }

            if (m_position + length > m_length)
            {
                ThrowEndOfStreamException();
            }

            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_position, rv, 0, length);
            m_position += length;
            return rv;
        }

        public CtpTime ReadTime()
        {
            return new CtpTime((long)ReadBits64());
        }

        public CtpBuffer ReadBuffer()
        {
            return new CtpBuffer(ReadBytes(), false);
        }

        public CtpCommand ReadDocument()
        {
            return new CtpCommand(ReadBytes(), false);
        }

        public string ReadString()
        {
            int length = (int)Read7BitInt();
            if (length == 0)
            {
                return string.Empty;
            }

            if (m_position + length > m_length)
            {
                ThrowEndOfStreamException();
            }
            var rv = Encoding.UTF8.GetString(m_buffer, m_position, length);
            m_position += length;
            return rv;
        }

        public CtpCommandKeyword ReadDocumentName()
        {
            if (m_position + 1 > m_length)
            {
                ThrowEndOfStreamException();
            }
            if (m_position + 1 + m_buffer[m_position] > m_length)
            {
                ThrowEndOfStreamException();
            }
            var rv = CtpCommandKeyword.Lookup(m_buffer, m_position);
            m_position += m_buffer[m_position] + 1;
            return rv;
        }

        #region [ Read Bits ]

        public uint ReadBits8()
        {
            if (m_position + 1 > m_length)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_position];
            m_position++;
            return rv;
        }

        public uint ReadBits16()
        {
            if (m_position + 2 > m_length)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_position] << 8
                    | (uint)m_buffer[m_position + 1];
            m_position += 2;
            return rv;
        }

        private uint ReadBits32()
        {
            if (m_position + 4 > m_length)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_position] << 24
                      | (uint)m_buffer[m_position + 1] << 16
                      | (uint)m_buffer[m_position + 2] << 8
                      | (uint)m_buffer[m_position + 3];
            m_position += 4;
            return rv;
        }

        private ulong ReadBits64()
        {
            if (m_position + 8 > m_length)
            {
                ThrowEndOfStreamException();
            }
            ulong rv = (ulong)m_buffer[m_position + 0] << 56 |
                      (ulong)m_buffer[m_position + 1] << 48 |
                      (ulong)m_buffer[m_position + 2] << 40 |
                      (ulong)m_buffer[m_position + 3] << 32 |
                      (ulong)m_buffer[m_position + 4] << 24 |
                      (ulong)m_buffer[m_position + 5] << 16 |
                      (ulong)m_buffer[m_position + 6] << 8 |
                      (ulong)m_buffer[m_position + 7];
            m_position += 8;
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
