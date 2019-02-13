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
        /// <summary>
        /// The position 1 beyond the last byte of the bit stream
        /// </summary>
        private int m_endOfBitStream;

        private int m_currentBytePosition;
        private int m_currentBitPosition;

        private int m_bitStreamCacheBitCount;
        private uint m_bitStreamCache;
        private int m_bitsInLastByte;

        public ByteReader()
        {
            m_buffer = Empty;
        }

        public bool IsEmpty
        {
            get
            {
                if (m_currentBytePosition != m_endOfByteStream)
                    return false;

                if (m_currentBitPosition != m_endOfBitStream)
                    return false;

                return m_bitStreamCacheBitCount == 0;
            }
        }

        public void SetBuffer(byte[] data)
        {
            SetBuffer(data, 0, data.Length);
        }

        public void SetBuffer(byte[] data, int position, int length)
        {
            m_buffer = data;
            m_endOfBitStream = position + length;

            uint bits = Encoding7Bit.ReadUInt32(data, ref position);
            m_bitsInLastByte = (int)(bits & 7);
            if (m_bitsInLastByte == 0)
                m_bitsInLastByte = 8;

            m_currentBytePosition = position;
            m_endOfByteStream = m_currentBitPosition = m_endOfBitStream - (int)((bits + 7) >> 3);

            m_bitStreamCacheBitCount = 0;
            m_bitStreamCache = 0;
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

        #endregion

        #endregion

        public CtpCommand ReadCtpCommand()
        {
            return CtpCommand.Load(ReadBytes(), false);
        }

        public CtpBuffer ReadCtpBuffer()
        {
            return new CtpBuffer(ReadBytes());
        }

        public CtpTime ReadCtpTime()
        {
            return new CtpTime(ReadInt64());
        }

        public CtpNumeric ReadNumeric()
        {
            byte code = (byte)ReadBits8();

            int flags = (code & 31) << 16;
            if (code > 127)
            {
                flags |= unchecked((int)Bits.Bit31);
            }

            int high = 0;
            int mid = 0;
            int low = 0;

            if ((flags & 64) != 0)
                high = (int)ReadBits8();

            if ((flags & 32) != 0)
                mid = (int)ReadBits8();

            low = (int)ReadBits8();
            return new CtpNumeric(flags, high, mid, low);
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
            if (m_currentBytePosition + 1 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentBytePosition];
            m_currentBytePosition++;
            return rv;
        }

        public uint ReadBits12()
        {
            return (ReadBits8() << 4) | ReadBits4();
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

        public uint ReadBits20()
        {
            return (ReadBits16() << 4) | ReadBits4();
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

        public uint ReadBits28()
        {
            return (ReadBits24() << 4) | ReadBits4();
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

        private void ReadMoreBits(int requiredBits)
        {
            if (m_currentBitPosition == m_endOfBitStream)
                ThrowEndOfStreamException();

            while (m_bitStreamCacheBitCount < 20 && m_currentBitPosition != m_endOfBitStream)
            {
                if (m_currentBitPosition + 1 == m_endOfBitStream)
                {
                    m_bitStreamCacheBitCount += m_bitsInLastByte;
                    m_bitStreamCache = (m_bitStreamCache << m_bitsInLastByte) | ((uint)m_buffer[m_currentBitPosition] >> (8 - m_bitsInLastByte));
                }
                else
                {
                    m_bitStreamCacheBitCount += 8;
                    m_bitStreamCache = (m_bitStreamCache << 8) | m_buffer[m_currentBitPosition];
                }
                m_currentBitPosition++;
            }

            if (m_bitStreamCacheBitCount < requiredBits)
                ThrowEndOfStreamException();
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

        public CtpObject ReadObjectWithoutType(CtpTypeCode code)
        {
            switch (code)
            {
                case CtpTypeCode.Null:
                    return CtpObject.Null;
                case CtpTypeCode.Int64:
                    if (ReadBits1() == 1)
                    {
                        return ~(long)Read8BitSegments();
                    }
                    else
                    {
                        return (long)Read8BitSegments();
                    }
                case CtpTypeCode.Single:
                    return (CtpObject)ReadSingle();
                case CtpTypeCode.Double:
                    return (CtpObject)ReadDouble();
                case CtpTypeCode.Numeric:
                    return (CtpObject)ReadNumeric();
                case CtpTypeCode.CtpTime:
                    return (CtpObject)ReadCtpTime();
                case CtpTypeCode.Boolean:
                    return (CtpObject)(ReadBits1() == 1);
                case CtpTypeCode.Guid:
                    return (CtpObject)ReadGuid();
                case CtpTypeCode.String:
                    return (CtpObject)ReadString();
                case CtpTypeCode.CtpBuffer:
                    return (CtpObject)ReadCtpBuffer();
                case CtpTypeCode.CtpCommand:
                    return (CtpObject)ReadCtpCommand();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public CtpObject ReadObject()
        {
            return ReadObjectWithoutType((CtpTypeCode)ReadBits4());
        }
    }
}
