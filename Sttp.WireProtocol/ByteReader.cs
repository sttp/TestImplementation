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
        private byte m_usedBitsForLastBitWord;

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

        public bool IsEmpty
        {
            get
            {
                if (m_currentBitPosition != m_currentBytePosition)
                    return false;
                if (m_bitStreamCacheBitCount == 0)
                    return true;

                if (m_bitStreamCacheBitCount >= 8 - m_usedBitsForLastBitWord)
                {
                    return true;
                }

                return false;
            }
        }

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
            m_usedBitsForLastBitWord = 0;
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
            if (m_currentBytePosition + 16 > m_currentBitPosition)
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
            int length = (int)Read4BitSegments();
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

        public string ReadAsciiShort()
        {
            if (m_currentBytePosition + 1 > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            if (m_currentBytePosition + 1 + m_buffer[m_currentBytePosition] > m_currentBitPosition)
            {
                ThrowEndOfStreamException();
            }
            char[] data = new char[m_buffer[m_currentBytePosition]];
            for (int x = 0; x < data.Length; x++)
            {
                data[x] = (char)m_buffer[m_currentBytePosition + 1 + x];
                if (data[x] > 127)
                    throw new Exception("Not an ASCII string");
            }
            m_currentBytePosition += 1 + data.Length;
            return new string(data);
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
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits2()
        {
            const int bits = 2;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }

        public uint ReadBits3()
        {
            const int bits = 3;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits4()
        {
            const int bits = 4;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits5()
        {
            const int bits = 5;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits6()
        {
            const int bits = 6;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
            m_bitStreamCacheBitCount -= bits;
            return (uint)((m_bitStreamCache >> m_bitStreamCacheBitCount) & ((1 << bits) - 1));
        }
        public uint ReadBits7()
        {
            const int bits = 7;
            if (m_bitStreamCacheBitCount < bits)
                ReadMoreBits(bits);
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

        public ulong ReadBits40()
        {
            if (m_currentBytePosition + 5 > m_currentBitPosition)
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
            if (m_currentBytePosition + 6 > m_currentBitPosition)
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
            if (m_currentBytePosition + 7 > m_currentBitPosition)
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
            if (m_currentBytePosition + 8 > m_currentBitPosition)
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

        private void ReadMoreBits(int len)
        {
            if (m_currentBitPosition - m_currentBytePosition < 1)
            {
                ThrowEndOfStreamException();
            }
            if (m_currentBitPosition == m_lastPosition)
            {
                m_currentBitPosition--;
                m_bitStreamCacheBitCount = 5;
                m_bitStreamCache = m_buffer[m_currentBitPosition];
                m_usedBitsForLastBitWord = (byte)(m_bitStreamCache >> 5);
                if (len > 5)
                {
                    ReadMoreBits(len - 5);
                }
            }
            else
            {
                m_currentBitPosition--;
                m_bitStreamCacheBitCount += 8;
                m_bitStreamCache = (m_bitStreamCache << 8) | m_buffer[m_currentBitPosition];
            }

        }

        public ulong Read8BitSegments()
        {
            int bits = 0;
            while (ReadBits1() == 1)
            {
                bits += 8;
            }
            return ReadBits(bits);
        }

        public ulong Read4BitSegments()
        {
            int bits = 0;
            while (ReadBits1() == 1)
            {
                bits += 4;
            }
            return ReadBits(bits);
        }

        public ulong ReadBits(int bits)
        {
            ulong value = 0;
            if (bits > 64 || bits < 0)
                throw new ArgumentOutOfRangeException(nameof(bits), "Must be between 0 and 64 inclusive");

            switch (bits & 7)
            {
                case 0:
                    break;
                case 1:
                    value = ReadBits1();
                    break;
                case 2:
                    value = ReadBits2();
                    break;
                case 3:
                    value = ReadBits3();
                    break;
                case 4:
                    value = ReadBits4();
                    break;
                case 5:
                    value = ReadBits5();
                    break;
                case 6:
                    value = ReadBits6();
                    break;
                case 7:
                    value = ReadBits7();
                    break;
            }

            switch (bits >> 3)
            {
                case 0:
                    return value;
                case 1:
                    return value | ((ulong)ReadBits8() << (bits & 7));
                case 2:
                    return value | ((ulong)ReadBits16() << (bits & 7));
                case 3:
                    return value | ((ulong)ReadBits24() << (bits & 7));
                case 4:
                    return value | ((ulong)ReadBits32() << (bits & 7));
                case 5:
                    return value | ((ulong)ReadBits40() << (bits & 7));
                case 6:
                    return value | ((ulong)ReadBits48() << (bits & 7));
                case 7:
                    return value | ((ulong)ReadBits56() << (bits & 7));
                case 8:
                    return ReadBits64();
            }
            throw new InvalidOperationException("Should never happen");
        }

        #endregion
    }
}
