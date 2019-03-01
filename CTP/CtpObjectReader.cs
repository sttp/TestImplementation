using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CTP;
using GSF;

namespace CTP
{
    public unsafe class CtpObjectReader
    {
        private static readonly byte[] Empty = new byte[0];

        private byte[] m_buffer;

        /// <summary>
        /// The position 1 beyond the last byte of the byte stream
        /// </summary>
        private int m_endOfByteStream;

        private int m_startBytePosition;

        private int m_currentBytePosition;

        private CtpObject m_currentValue;

        public CtpObjectReader()
        {
            SetBuffer(Empty);
        }

        public CtpObjectReader(byte[] data)
        {
            SetBuffer(data);
        }

        public CtpObjectReader(byte[] data, int position, int length)
        {
            SetBuffer(data, position, length);
        }

        public bool IsEmpty => m_currentBytePosition == m_endOfByteStream;

        public int Position
        {
            get => m_currentBytePosition - m_startBytePosition;
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

        public CtpObject Read()
        {
            int oldPosition = m_currentBytePosition;
            ReadInternal();
            if (m_currentBytePosition > m_endOfByteStream)
            {
                m_currentBytePosition = oldPosition;
                throw new EndOfStreamException();
            }
            return m_currentValue;
        }

        #region [ Static Methods ]

        /// <summary>
        /// Attempts to read the next object stored on the buffer.
        /// Note: if reading past the end of the stream, the return value will be <see cref="CtpObject.Null"/>
        /// and <see cref="m_currentBytePosition"/> will be greater than <see cref="m_endOfByteStream"/>.
        /// </summary>
        /// <returns></returns>
        private void ReadInternal()
        {
            CtpObjectSymbols symbol = (CtpObjectSymbols)ReadBits8();
            if (m_currentBytePosition > m_endOfByteStream)
            {
                m_currentValue = CtpObject.Null;
                return;
            }
            if (symbol <= CtpObjectSymbols.Null)
            {
                m_currentValue = CtpObject.Null;
                return;
            }
            if (symbol <= CtpObjectSymbols.IntElse)
            {
                ReadInteger(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.SingleElse)
            {
                ReadSingle(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.DoubleElse)
            {
                ReadDouble(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.NumericElse)
            {
                ReadNumeric(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.CtpTimeElse)
            {
                ReadTime(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.BoolElse)
            {
                m_currentValue = (symbol == CtpObjectSymbols.BoolTrue);
                return;
            }

            if (symbol <= CtpObjectSymbols.GuidElse)
            {
                ReadGuid(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.StringElse)
            {
                ReadString(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.CtpBufferElse)
            {
                ReadBuffer(symbol);
                return;
            }

            if (symbol <= CtpObjectSymbols.CtpCommandElse)
            {
                ReadCommand(symbol);
                return;
            }
            throw new ArgumentOutOfRangeException();
        }

        private void ReadInteger(CtpObjectSymbols symbol)
        {
            if (symbol <= CtpObjectSymbols.IntMaxRunLen)
            {
                m_currentValue = (int)symbol - (int)CtpObjectSymbols.Int0;
                return;
            }

            if (symbol == CtpObjectSymbols.IntBits64)
            {
                m_currentValue = (long)ReadBits64();
                return;
            }

            ulong negate = 0;
            if (symbol >= CtpObjectSymbols.IntBits8Neg)
            {
                negate = ulong.MaxValue;
                symbol -= (CtpObjectSymbols.IntBits8Neg - CtpObjectSymbols.IntBits8Pos);
            }

            switch (symbol)
            {
                case CtpObjectSymbols.IntBits8Pos:
                    {
                        m_currentValue = (long)(ReadBits8() ^ negate);
                        return;
                    }
                case CtpObjectSymbols.IntBits16Pos:
                    {
                        m_currentValue = (long)(ReadBits16() ^ negate);
                        return;
                    }
                case CtpObjectSymbols.IntBits24Pos:
                    {
                        m_currentValue = (long)(ReadBits24() ^ negate);
                        return;
                    }
                case CtpObjectSymbols.IntBits32Pos:
                    {
                        m_currentValue = (long)(ReadBits32() ^ negate);
                        return;
                    }
                case CtpObjectSymbols.IntBits40Pos:
                    {
                        m_currentValue = (long)(ReadBits40() ^ negate);
                        return;
                    }
                case CtpObjectSymbols.IntBits48Pos:
                    {
                        m_currentValue = (long)(ReadBits48() ^ negate);
                        return;
                    }
                case CtpObjectSymbols.IntBits56Pos:
                    {
                        m_currentValue = (long)(ReadBits56() ^ negate);
                        return;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private void ReadSingle(CtpObjectSymbols symbol)
        {
            uint v;
            if (symbol == CtpObjectSymbols.SingleNeg1)
            {
                m_currentValue = -1f;
                return;
            }
            if (symbol == CtpObjectSymbols.Single0)
            {
                m_currentValue = 0f;
                return;
            }
            if (symbol == CtpObjectSymbols.Single1)
            {
                m_currentValue = 1f;
                return;
            }
            if (symbol <= CtpObjectSymbols.Single79)
            {
                v = ReadBits24() | ((56u + (symbol - CtpObjectSymbols.Single56)) << 24);
                m_currentValue = *(float*)&v;
                return;
            }
            if (symbol <= CtpObjectSymbols.Single207)
            {
                v = ReadBits24() | ((184u + (symbol - CtpObjectSymbols.Single184)) << 24);
                m_currentValue = *(float*)&v;
                return;
            }

            v = ReadBits32();
            m_currentValue = *(float*)&v;
            return;
        }

        private void ReadDouble(CtpObjectSymbols symbol)
        {
            ulong v;
            if (symbol == CtpObjectSymbols.DoubleNeg1)
            {
                m_currentValue = -1.0;
                return;
            }
            if (symbol == CtpObjectSymbols.Double0)
            {
                m_currentValue = 0.0;
                return;
            }
            if (symbol == CtpObjectSymbols.Double1)
            {
                m_currentValue = 1.0;
                return;
            }
            if (symbol <= CtpObjectSymbols.Double65)
            {
                v = ReadBits56() | ((63ul + (symbol - CtpObjectSymbols.Double63)) << 56);
                m_currentValue = *(double*)&v;
                return;
            }
            if (symbol <= CtpObjectSymbols.Double193)
            {
                v = ReadBits56() | ((191ul + (symbol - CtpObjectSymbols.Double191)) << 56);
                m_currentValue = *(double*)&v;
                return;
            }

            v = ReadBits64();
            m_currentValue = *(double*)&v;
            return;
        }

        private void ReadNumeric(CtpObjectSymbols symbol)
        {
            byte flags;
            uint low;
            uint mid;
            uint high;
            switch (symbol)
            {
                case CtpObjectSymbols.NumericHigh:
                    flags = (byte)ReadBits8();
                    high = ReadNumericHelper(flags);
                    mid = ReadBits32();
                    low = ReadBits32();
                    m_currentValue = new CtpNumeric((byte)(flags & 63), high, mid, low);
                    return;
                case CtpObjectSymbols.NumericMid:
                    flags = (byte)ReadBits8();
                    mid = ReadNumericHelper(flags);
                    low = ReadBits32();
                    m_currentValue = new CtpNumeric((byte)(flags & 63), 0, mid, low);
                    return;
                case CtpObjectSymbols.NumericLow:
                    flags = (byte)ReadBits8();
                    low = ReadNumericHelper(flags);
                    m_currentValue = new CtpNumeric((byte)(flags & 63), 0, 0, low);
                    return;
                case CtpObjectSymbols.NumericNone:
                    flags = (byte)ReadBits8();
                    m_currentValue = new CtpNumeric((byte)(flags & 63), 0, 0, 0);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private uint ReadNumericHelper(byte flags)
        {
            switch (flags >> 6)
            {
                case 0:
                    return ReadBits8();
                case 1:
                    return ReadBits16();
                case 2:
                    return ReadBits24();
                default:
                    return ReadBits32();
            }
        }

        private void ReadTime(CtpObjectSymbols symbol)
        {
            if (symbol == CtpObjectSymbols.CtpTimeZero)
            {
                m_currentValue = new CtpTime(0);
                return;

            }

            ulong v;
            if (symbol <= CtpObjectSymbols.CtpTime17)
            {
                v = ReadBits56() | ((14ul + (symbol - CtpObjectSymbols.CtpTime14)) << 56);
                m_currentValue = new CtpTime((long)v);
                return;
            }

            v = ReadBits64();
            m_currentValue = new CtpTime((long)v);
            return;
        }

        private void ReadGuid(CtpObjectSymbols symbol)
        {
            if (symbol == CtpObjectSymbols.GuidEmpty)
            {
                m_currentValue = Guid.Empty;
                return;
            }

            if (m_currentBytePosition + 16 > m_endOfByteStream)
            {
                m_currentBytePosition += 16;
                m_currentValue = CtpObject.Null;
                return;
            }
            Guid rv = GuidExtensions.ToRfcGuid(m_buffer, m_currentBytePosition);
            m_currentBytePosition += 16;
            m_currentValue = rv;
            return;
        }

        private void ReadString(CtpObjectSymbols symbol)
        {
            int length;
            if (symbol <= CtpObjectSymbols.String30)
                length = symbol - CtpObjectSymbols.String0;
            else
                length = ReadArrayLength(symbol - CtpObjectSymbols.String8Bit);
            if (m_currentBytePosition + length > m_endOfByteStream)
            {
                m_currentBytePosition += length;
                m_currentValue = CtpObject.Null;
                return;
            }
            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            m_currentValue = Encoding.UTF8.GetString(rv);
            return;
        }

        private void ReadBuffer(CtpObjectSymbols symbol)
        {
            int length;
            if (symbol <= CtpObjectSymbols.CtpBuffer50)
                length = symbol - CtpObjectSymbols.CtpBuffer0;
            else
                length = ReadArrayLength(symbol - CtpObjectSymbols.CtpBuffer8Bit);
            if (m_currentBytePosition + length > m_endOfByteStream)
            {
                m_currentBytePosition += length;
                m_currentValue = CtpObject.Null;
                return;
            }

            m_currentValue = CtpBuffer.FromArray(m_buffer, m_currentBytePosition, length);
            m_currentBytePosition += length;
            return;
        }

        private void ReadCommand(CtpObjectSymbols symbol)
        {
            int length = ReadArrayLength(symbol - CtpObjectSymbols.CtpCommand8Bit);
            if (m_currentBytePosition + length > m_endOfByteStream)
            {
                m_currentBytePosition += length;
                m_currentValue = CtpObject.Null;
                return;
            }
            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            m_currentValue = new CtpCommand(rv);
            return;
        }

        private int ReadArrayLength(int bits)
        {
            switch (bits)
            {
                case 0:
                    return (int)ReadBits8();
                case 1:
                    return (int)ReadBits16();
                case 2:
                    return (int)ReadBits24();
                default:
                    return (int)ReadBits32();
            }
        }

        private uint ReadBits8()
        {
            if (m_currentBytePosition + 1 > m_endOfByteStream)
            {
                m_currentBytePosition++;
                return 0;
            }
            byte rv = m_buffer[m_currentBytePosition];
            m_currentBytePosition++;
            return rv;
        }

        private uint ReadBits16()
        {
            if (m_currentBytePosition + 2 > m_endOfByteStream)
            {
                m_currentBytePosition += 2;
                return 0;
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 8
                    | (uint)m_buffer[m_currentBytePosition + 1];
            m_currentBytePosition += 2;
            return rv;
        }

        private uint ReadBits24()
        {
            if (m_currentBytePosition + 3 > m_endOfByteStream)
            {
                m_currentBytePosition += 3;
                return 0;
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 16
                      | (uint)m_buffer[m_currentBytePosition + 1] << 8
                      | (uint)m_buffer[m_currentBytePosition + 2];
            m_currentBytePosition += 3;
            return rv;
        }

        private uint ReadBits32()
        {
            if (m_currentBytePosition + 4 > m_endOfByteStream)
            {
                m_currentBytePosition += 4;
                return 0;
            }
            uint rv = (uint)m_buffer[m_currentBytePosition] << 24
                      | (uint)m_buffer[m_currentBytePosition + 1] << 16
                      | (uint)m_buffer[m_currentBytePosition + 2] << 8
                      | (uint)m_buffer[m_currentBytePosition + 3];
            m_currentBytePosition += 4;
            return rv;
        }

        private ulong ReadBits40()
        {
            if (m_currentBytePosition + 5 > m_endOfByteStream)
            {
                m_currentBytePosition += 5;
                return 0;
            }
            ulong rv = (ulong)m_buffer[m_currentBytePosition + 0] << 32 |
                       (ulong)m_buffer[m_currentBytePosition + 1] << 24 |
                       (ulong)m_buffer[m_currentBytePosition + 2] << 16 |
                       (ulong)m_buffer[m_currentBytePosition + 3] << 8 |
                       (ulong)m_buffer[m_currentBytePosition + 4];
            m_currentBytePosition += 5;
            return rv;
        }

        private ulong ReadBits48()
        {
            if (m_currentBytePosition + 6 > m_endOfByteStream)
            {
                m_currentBytePosition += 6;
                return 0;
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

        private ulong ReadBits56()
        {
            if (m_currentBytePosition + 7 > m_endOfByteStream)
            {
                m_currentBytePosition += 7;
                return 0;
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

        private ulong ReadBits64()
        {
            if (m_currentBytePosition + 8 > m_endOfByteStream)
            {
                m_currentBytePosition += 8;
                return 0;
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

        #endregion

        internal static bool TryReadPacket(byte[] data, int position, int length, int maximumPacketSize, out PacketContents payloadType, out long payloadFlags, out byte[] payloadBuffer, out int consumedLength)
        {
            if (length > maximumPacketSize)
                throw new Exception("Command size is too large");

            var stream = new CtpObjectReader(data, position, length);

            CtpObject pType;
            CtpObject pFlags;
            CtpObject pData;
            stream.ReadInternal();
            pType = stream.m_currentValue;
            stream.ReadInternal();
            pFlags = stream.m_currentValue;
            stream.ReadInternal();
            pData = stream.m_currentValue;


            if (stream.m_currentBytePosition > stream.m_endOfByteStream)
            {
                payloadType = default(PacketContents);
                payloadFlags = 0;
                payloadBuffer = null;
                consumedLength = 0;
                return false;
            }
            else
            {
                payloadType = (PacketContents)(byte)pType;
                payloadFlags = (long)pFlags;
                payloadBuffer = (byte[])pData;
                consumedLength = stream.m_currentBytePosition - position;
                return true;
            }

        }

    }
}
