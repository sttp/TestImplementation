using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public unsafe class ByteReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;
        private int m_startingPosition;
        private int m_lastPosition;

        private int m_currentBytePosition;
        private int m_currentBitPosition;

        private int m_bitStreamCacheBitCount;
        private uint m_bitStreamCache;

        public ByteReader()
        {
            m_buffer = Empty;
        }

        public ByteReader(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public ByteReader(byte[] data, int position, int length)
        {
            SetBuffer(data, position, length);
        }

        public bool IsEmpty => false; //ToDo: Fix this.

        public void SetBuffer(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_startingPosition = position;
            m_lastPosition = length + position;

            m_currentBytePosition = position;
            m_currentBitPosition = m_lastPosition;

            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
        }

        private void ThrowEndOfStreamException()
        {
            throw new EndOfStreamException();
        }

        private void EnsureCapacity(int length)
        {
            if (m_currentBytePosition + length > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
        }

        #region [ Read Methods ]

        #region [ 1 byte values ]

        public byte ReadByte()
        {
            if (m_currentBytePosition + 1 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentBytePosition];
            m_currentBytePosition++;
            return rv;
        }

        #endregion

        #region [ 2-byte values ]

        public short ReadInt16()
        {
            if (m_currentBytePosition + 2 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            short rv = (short)(m_buffer[m_currentBytePosition] << 8 | m_buffer[m_currentBytePosition + 1]);
            m_currentBytePosition += 2;
            return rv;
        }

        #endregion

        #region [ 4-byte values ]

        public int ReadInt32()
        {
            if (m_currentBytePosition + 4 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            int rv = m_buffer[m_currentBytePosition + 0] << 24 |
                     m_buffer[m_currentBytePosition + 1] << 16 |
                     m_buffer[m_currentBytePosition + 2] << 8 |
                     m_buffer[m_currentBytePosition + 3];
            m_currentBytePosition += 4;
            return rv;
        }

        public uint ReadUInt32()
        {
            return (uint)ReadInt32();
        }

        public float ReadSingle()
        {
            var value = ReadInt32();
            return *(float*)&value;
        }

        #endregion

        #region [ 8-byte values ]

        public long ReadInt64()
        {
            if (m_currentBytePosition + 8 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            long rv = (long)m_buffer[m_currentBytePosition + 0] << 56 |
                      (long)m_buffer[m_currentBytePosition + 1] << 48 |
                      (long)m_buffer[m_currentBytePosition + 2] << 40 |
                      (long)m_buffer[m_currentBytePosition + 3] << 32 |
                      (long)m_buffer[m_currentBytePosition + 4] << 24 |
                      (long)m_buffer[m_currentBytePosition + 5] << 16 |
                      (long)m_buffer[m_currentBytePosition + 6] << 8 |
                      (long)m_buffer[m_currentBytePosition + 7];
            m_currentBytePosition += 8;
            return rv;
        }

        public double ReadDouble()
        {
            var value = ReadInt64();
            return *(double*)&value;
        }

        public ulong ReadUInt64()
        {
            return (ulong)ReadInt64();
        }

        #endregion

        #region [ 16-byte values ]

        public decimal ReadDecimal()
        {
            if (m_currentBytePosition + 16 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            decimal rv = BigEndian.ToDecimal(m_buffer, m_currentBytePosition);
            m_currentBytePosition += 16;
            return rv;
        }

        public Guid ReadGuid()
        {
            if (m_currentBytePosition + 16 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }

            Guid rv = GuidExtensions.ToRfcGuid(m_buffer, m_currentBytePosition);
            m_currentBytePosition += 16;
            return rv;
        }

        #endregion

        #region [ Variable Length ]

        public byte[] ReadBytes()
        {
            int length = (int)Read4BitSegments();
            if (length == 0)
            {
                return null;
            }
            if (length == 1)
            {
                return Empty;
            }

            length--; // minus one, because 1 added to len before writing since (1) is used for empty.
            EnsureCapacity(length);

            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            return rv;
        }

        public string ReadString()
        {
            byte[] rv = ReadBytes();
            if (rv == null)
                return null;
            if (rv.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(rv);
        }

        #endregion

        #endregion

        public SttpMarkup ReadSttpMarkup()
        {
            return new SttpMarkup(this);
        }

        public SttpBuffer ReadSttpBuffer()
        {
            return new SttpBuffer(this);
        }

        public SttpTime ReadSttpTime()
        {
            return new SttpTime(this);
        }

        public SttpBulkTransport ReadSttpBulkTransport()
        {
            return new SttpBulkTransport(this);
        }

        #region [ Read Bits ]

        public uint ReadBits0()
        {
            return 0;
        }

        public uint ReadBits1()
        {
            const int bits = 1;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits2()
        {
            const int bits = 2;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits3()
        {
            const int bits = 3;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits4()
        {
            const int bits = 4;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits5()
        {
            const int bits = 5;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits6()
        {
            const int bits = 6;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits7()
        {
            const int bits = 7;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits();
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits8()
        {
            if (m_currentBytePosition + 1 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentBytePosition];
            m_currentBytePosition++;
            return rv;
        }
        public uint ReadBits9()
        {
            //Note, .NET evaluates Left to Right. If ported, be sure to correct this for the target language.
            return ReadBits8() | (ReadBits1() << 8);
        }
        public uint ReadBits10()
        {
            return ReadBits8() | (ReadBits2() << 8);
        }
        public uint ReadBits11()
        {
            return ReadBits8() | (ReadBits3() << 8);
        }
        public uint ReadBits12()
        {
            return ReadBits8() | (ReadBits4() << 8);
        }
        public uint ReadBits13()
        {
            return ReadBits8() | (ReadBits5() << 8);
        }
        public uint ReadBits14()
        {
            return ReadBits8() | (ReadBits6() << 8);
        }
        public uint ReadBits15()
        {
            return ReadBits8() | (ReadBits7() << 8);
        }
        public uint ReadBits16()
        {
            if (m_currentBytePosition + 2 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 8
                    | (uint)m_buffer[m_currentBytePosition + 1];
            m_currentBytePosition += 2;
            return rv;
        }
        public uint ReadBits17()
        {
            return ReadBits16() | (ReadBits1() << 16);

        }
        public uint ReadBits18()
        {
            return ReadBits16() | (ReadBits2() << 16);

        }
        public uint ReadBits19()
        {
            return ReadBits16() | (ReadBits3() << 16);

        }

        public uint ReadBits20()
        {
            return ReadBits16() | (ReadBits4() << 16);

        }
        public uint ReadBits21()
        {
            return ReadBits16() | (ReadBits5() << 16);

        }
        public uint ReadBits22()
        {
            return ReadBits16() | (ReadBits6() << 16);
        }

        public uint ReadBits23()
        {
            return ReadBits16() | (ReadBits7() << 16);

        }
        public uint ReadBits24()
        {
            if (m_currentBytePosition + 3 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 16
                      | (uint)m_buffer[m_currentBytePosition + 1] << 8
                      | (uint)m_buffer[m_currentBytePosition + 2];
            m_currentBytePosition += 3;
            return rv;
        }
        public uint ReadBits25()
        {
            return ReadBits24() | (ReadBits1() << 24);
        }
        public uint ReadBits26()
        {
            return ReadBits24() | (ReadBits2() << 24);
        }
        public uint ReadBits27()
        {
            return ReadBits24() | (ReadBits3() << 24);
        }
        public uint ReadBits28()
        {
            return ReadBits24() | (ReadBits4() << 24);
        }
        public uint ReadBits29()
        {
            return ReadBits24() | (ReadBits5() << 24);
        }

        public uint ReadBits30()
        {
            return ReadBits24() | (ReadBits6() << 24);
        }

        public uint ReadBits31()
        {
            return ReadBits24() | (ReadBits7() << 24);
        }

        public uint ReadBits32()
        {
            if (m_currentBytePosition + 4 > m_currentBitPosition)
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

        private void ReadMoreBits()
        {
            if (m_currentBitPosition - m_currentBytePosition < 1)
            {
                ThrowEndOfStreamException();
            }
            m_currentBitPosition--;
            m_bitStreamCacheBitCount += 8;
            m_bitStreamCache = (m_bitStreamCache << 8) | m_buffer[m_currentBitPosition];
        }

        public ulong Read8BitSegments()
        {
            ulong value = 0;
            int bits = 0;
            while (ReadBits1() == 1)
            {
                value = value | ((ulong)ReadByte() << bits);
                bits += 8;
            }
            return value;
        }
        public ulong Read4BitSegments()
        {
            ulong value = 0;
            int bits = 0;
            while (ReadBits1() == 1)
            {
                value = value | ((ulong)ReadBits4() << bits);
                bits += 4;
            }
            return value;
        }

        #endregion
    }
}
