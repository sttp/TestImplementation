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

        public CtpObjectReader(byte[] data)
        {
            SetBuffer(data);
        }

        public CtpObjectReader(byte[] data, int position, int length)
        {
            SetBuffer(data, position, length);
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

        public CtpObject Read()
        {
            CtpObjectSymbols symbol = (CtpObjectSymbols)ReadBits8();
            if (symbol <= CtpObjectSymbols.Null)
                return CtpObject.Null;
            if (symbol <= CtpObjectSymbols.IntElse)
                return ReadInt64(symbol);
            if (symbol <= CtpObjectSymbols.SingleElse)
                return ReadSingle(symbol);
            if (symbol <= CtpObjectSymbols.DoubleElse)
                return ReadDouble(symbol);
            if (symbol <= CtpObjectSymbols.NumericElse)
                return ReadNumeric(symbol);
            if (symbol <= CtpObjectSymbols.CtpTimeElse)
                return ReadTime(symbol);
            if (symbol <= CtpObjectSymbols.BoolElse)
                return symbol == CtpObjectSymbols.BoolElse;
            if (symbol <= CtpObjectSymbols.StringElse)
                return ReadString(symbol);
            if (symbol <= CtpObjectSymbols.CtpBufferElse)
                return ReadBuffer(symbol);
            if (symbol <= CtpObjectSymbols.CtpCommandElse)
                return ReadCommand();
            throw new ArgumentOutOfRangeException();
        }

        private CtpObject ReadInt64(CtpObjectSymbols symbol)
        {
            if (symbol <= CtpObjectSymbols.Int100)
            {
                return 100 - (int)symbol;
            }
            if (symbol <= CtpObjectSymbols.IntBits56)
            {
                switch (symbol)
                {
                    case CtpObjectSymbols.IntBits8:
                        return (int)ReadBits8();
                    case CtpObjectSymbols.IntBits16:
                        return (int)ReadBits16();
                    case CtpObjectSymbols.IntBits24:
                        return (int)ReadBits24();
                    case CtpObjectSymbols.IntBits32:
                        return (int)ReadBits32();
                    case CtpObjectSymbols.IntBits40:
                        return (long)ReadBits40();
                    case CtpObjectSymbols.IntBits48:
                        return (long)ReadBits48();
                    case CtpObjectSymbols.IntBits56:
                        return (long)ReadBits56();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
                }
            }
            else if (symbol <= CtpObjectSymbols.IntNegBits56)
            {
                switch (symbol)
                {
                    case CtpObjectSymbols.IntNegBits8:
                        return ~(int)ReadBits8();
                    case CtpObjectSymbols.IntNegBits16:
                        return ~(int)ReadBits16();
                    case CtpObjectSymbols.IntNegBits24:
                        return ~(int)ReadBits24();
                    case CtpObjectSymbols.IntNegBits32:
                        return ~(int)ReadBits32();
                    case CtpObjectSymbols.IntNegBits40:
                        return ~(long)ReadBits40();
                    case CtpObjectSymbols.IntNegBits48:
                        return ~(long)ReadBits48();
                    case CtpObjectSymbols.IntNegBits56:
                        return ~(long)ReadBits56();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
                }
            }
            else
            {
                return (long)ReadBits64();
            }
        }

        private CtpObject ReadSingle(CtpObjectSymbols symbol)
        {
            switch (symbol)
            {
                case CtpObjectSymbols.SingleNeg1:
                    return -1f;
                case CtpObjectSymbols.Single0:
                    return 0f;
                case CtpObjectSymbols.Single1:
                    return 1f;
                case CtpObjectSymbols.SingleElse:
                    uint value = ReadBits32();
                    return *(float*)&value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private CtpObject ReadDouble(CtpObjectSymbols symbol)
        {
            switch (symbol)
            {
                case CtpObjectSymbols.DoubleNeg1:
                    return -1.0;
                case CtpObjectSymbols.Double0:
                    return 0.0;
                case CtpObjectSymbols.Double1:
                    return 1.0;
                case CtpObjectSymbols.DoubleElse:
                    ulong value = ReadBits64();
                    return *(double*)&value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private CtpObject ReadNumeric(CtpObjectSymbols symbol)
        {
            switch (symbol)
            {
                case CtpObjectSymbols.NumericNeg1:
                    return new CtpNumeric(true, 0, 0, 0, 1);
                case CtpObjectSymbols.Numeric0:
                    return new CtpNumeric(false, 0, 0, 0, 0);
                case CtpObjectSymbols.Numeric1:
                    return new CtpNumeric(false, 0, 0, 0, 1);
                case CtpObjectSymbols.NumericBytes0:
                case CtpObjectSymbols.NumericBytes1:
                case CtpObjectSymbols.NumericBytes2:
                case CtpObjectSymbols.NumericBytes3:
                case CtpObjectSymbols.NumericBytes4:
                case CtpObjectSymbols.NumericBytes5:
                case CtpObjectSymbols.NumericBytes6:
                case CtpObjectSymbols.NumericBytes7:
                case CtpObjectSymbols.NumericBytes8:
                case CtpObjectSymbols.NumericBytes9:
                case CtpObjectSymbols.NumericBytes10:
                case CtpObjectSymbols.NumericBytes11:
                case CtpObjectSymbols.NumericElse:
                    {
                        byte code = (byte)ReadBits8();
                        int flags = (code & 31) << 16;
                        int low;
                        int mid;
                        if (code > 127)
                        {
                            flags |= unchecked((int)Bits.Bit31);
                        }
                        switch (symbol)
                        {
                            case CtpObjectSymbols.NumericBytes0:
                                return new CtpNumeric(flags, 0, 0, 0);
                            case CtpObjectSymbols.NumericBytes1:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits8());
                            case CtpObjectSymbols.NumericBytes2:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits16());
                            case CtpObjectSymbols.NumericBytes3:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits24());
                            case CtpObjectSymbols.NumericBytes4:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits32());
                            case CtpObjectSymbols.NumericBytes5:
                                low = (int)ReadBits32();
                                return new CtpNumeric(flags, 0, (int)ReadBits8(), low);
                            case CtpObjectSymbols.NumericBytes6:
                                low = (int)ReadBits32();
                                return new CtpNumeric(flags, 0, (int)ReadBits16(), low);
                            case CtpObjectSymbols.NumericBytes7:
                                low = (int)ReadBits32();
                                return new CtpNumeric(flags, 0, (int)ReadBits24(), low);
                            case CtpObjectSymbols.NumericBytes8:
                                low = (int)ReadBits32();
                                return new CtpNumeric(flags, 0, (int)ReadBits32(), low);
                            case CtpObjectSymbols.NumericBytes9:
                                low = (int)ReadBits32();
                                mid = (int)ReadBits32();
                                return new CtpNumeric(flags, (int)ReadBits8(), mid, low);
                            case CtpObjectSymbols.NumericBytes10:
                                low = (int)ReadBits32();
                                mid = (int)ReadBits32();
                                return new CtpNumeric(flags, (int)ReadBits16(), mid, low);
                            case CtpObjectSymbols.NumericBytes11:
                                low = (int)ReadBits32();
                                mid = (int)ReadBits32();
                                return new CtpNumeric(flags, (int)ReadBits24(), mid, low);
                            case CtpObjectSymbols.NumericElse:
                                low = (int)ReadBits32();
                                mid = (int)ReadBits32();
                                return new CtpNumeric(flags, (int)ReadBits32(), mid, low);
                            default:
                                throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private CtpObject ReadTime(CtpObjectSymbols symbol)
        {
            if (symbol == CtpObjectSymbols.CtpTimeZero)
                return new CtpTime(0);
            return new CtpTime((long)ReadBits64());
        }

        private byte[] ReadBuffer(CtpObjectSymbols symbol)
        {
            int length = symbol - CtpObjectSymbols.CtpBuffer0;
            if (symbol == CtpObjectSymbols.CtpBufferElse)
            {
                length = (int)Read();
            }
            EnsureCapacity(length);
            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            return rv;
        }

        private string ReadString(CtpObjectSymbols symbol)
        {
            int length = symbol - CtpObjectSymbols.String0;
            if (symbol == CtpObjectSymbols.StringElse)
            {
                length = (int)Read();
            }
            EnsureCapacity(length);
            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            return Encoding.UTF8.GetString(rv);
        }

        private CtpCommand ReadCommand()
        {
            int length = (int)Read();
            EnsureCapacity(length);
            byte[] rv = new byte[length];
            Array.Copy(m_buffer, m_currentBytePosition, rv, 0, length);
            m_currentBytePosition += length;
            return new CtpCommand(rv);
        }

        private uint ReadBits8()
        {
            if (m_currentBytePosition + 1 > m_endOfByteStream)
            {
                ThrowEndOfStreamException();
            }
            byte rv = m_buffer[m_currentBytePosition];
            m_currentBytePosition++;
            return rv;
        }

        private uint ReadBits16()
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

        private uint ReadBits24()
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

        private uint ReadBits32()
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

        private ulong ReadBits40()
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

        private ulong ReadBits48()
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

        private ulong ReadBits56()
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

        private ulong ReadBits64()
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

        internal static bool TryReadPacket(byte[] data, int position, int length, int maximumPacketSize, out long payloadType, out long payloadFlags, out byte[] payloadBuffer, out int consumedLength)
        {
            if (length > maximumPacketSize)
                throw new Exception("Command size is too large");
            throw new NotImplementedException();
        }

    }
}
