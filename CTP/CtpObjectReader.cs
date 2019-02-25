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

        public CtpObject Read()
        {
            int currentPosition = m_currentBytePosition;
            var rv = Read(m_buffer, ref currentPosition, m_endOfByteStream);
            if (currentPosition > m_endOfByteStream)
                throw new EndOfStreamException();
            m_currentBytePosition = currentPosition;
            return rv;
        }

        public static CtpObject Read(byte[] data, ref int currentPosition, int endPosition)
        {
            CtpObjectSymbols symbol = (CtpObjectSymbols)ReadBits8(data, ref currentPosition, endPosition);
            if (currentPosition > endPosition)
                return CtpObject.Null;

            if (symbol <= CtpObjectSymbols.Null)
                return CtpObject.Null;
            if (symbol <= CtpObjectSymbols.IntElse)
                return ReadInt64(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.SingleElse)
                return ReadSingle(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.DoubleElse)
                return ReadDouble(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.NumericElse)
                return ReadNumeric(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.CtpTimeElse)
                return ReadTime(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.BoolElse)
                return symbol == CtpObjectSymbols.BoolElse;
            if (symbol <= CtpObjectSymbols.GuidElse)
                return ReadGuid(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.StringElse)
                return ReadString(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.CtpBufferElse)
                return ReadBuffer(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.CtpCommandElse)
                return ReadCommand(data, ref currentPosition, endPosition);
            throw new ArgumentOutOfRangeException();
        }

        private static CtpObject ReadGuid(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            if (symbol == CtpObjectSymbols.GuidEmpty)
                return Guid.Empty;

            if (currentPosition + 16 > endPosition)
            {
                currentPosition += 16;
                return CtpObject.Null;
            }
            Guid rv = GuidExtensions.ToRfcGuid(data, currentPosition);
            currentPosition += 16;
            return rv;
        }

        private static CtpObject ReadInt64(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            if (symbol <= CtpObjectSymbols.Int100)
            {
                return (int)symbol - (int)CtpObjectSymbols.Int0;
            }
            if (symbol <= CtpObjectSymbols.IntBits56)
            {
                switch (symbol)
                {
                    case CtpObjectSymbols.IntBits8:
                        return (int)ReadBits8(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntBits16:
                        return (int)ReadBits16(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntBits24:
                        return (int)ReadBits24(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntBits32:
                        return (long)ReadBits32(data, ref currentPosition, endPosition); //Type long here since casting a uint->int->long can make the number negative
                    case CtpObjectSymbols.IntBits40:
                        return (long)ReadBits40(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntBits48:
                        return (long)ReadBits48(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntBits56:
                        return (long)ReadBits56(data, ref currentPosition, endPosition);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
                }
            }
            if (symbol <= CtpObjectSymbols.IntNegBits56)
            {
                switch (symbol)
                {
                    case CtpObjectSymbols.IntNegBits8:
                        return ~(long)ReadBits8(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntNegBits16:
                        return ~(long)ReadBits16(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntNegBits24:
                        return ~(long)ReadBits24(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntNegBits32:
                        return ~(long)ReadBits32(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntNegBits40:
                        return ~(long)ReadBits40(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntNegBits48:
                        return ~(long)ReadBits48(data, ref currentPosition, endPosition);
                    case CtpObjectSymbols.IntNegBits56:
                        return ~(long)ReadBits56(data, ref currentPosition, endPosition);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
                }
            }
            return (long)ReadBits64(data, ref currentPosition, endPosition);
        }

        private static CtpObject ReadSingle(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
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
                    uint value = ReadBits32(data, ref currentPosition, endPosition);
                    return *(float*)&value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private static CtpObject ReadDouble(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
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
                    ulong value = ReadBits64(data, ref currentPosition, endPosition);
                    return *(double*)&value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private static CtpObject ReadNumeric(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
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
                        byte flags = (byte)ReadBits8(data, ref currentPosition, endPosition);
                        int low;
                        int mid;
                        switch (symbol)
                        {
                            case CtpObjectSymbols.NumericBytes0:
                                return new CtpNumeric(flags, 0, 0, 0);
                            case CtpObjectSymbols.NumericBytes1:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits8(data, ref currentPosition, endPosition));
                            case CtpObjectSymbols.NumericBytes2:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits16(data, ref currentPosition, endPosition));
                            case CtpObjectSymbols.NumericBytes3:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits24(data, ref currentPosition, endPosition));
                            case CtpObjectSymbols.NumericBytes4:
                                return new CtpNumeric(flags, 0, 0, (int)ReadBits32(data, ref currentPosition, endPosition));
                            case CtpObjectSymbols.NumericBytes5:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, 0, (int)ReadBits8(data, ref currentPosition, endPosition), low);
                            case CtpObjectSymbols.NumericBytes6:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, 0, (int)ReadBits16(data, ref currentPosition, endPosition), low);
                            case CtpObjectSymbols.NumericBytes7:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, 0, (int)ReadBits24(data, ref currentPosition, endPosition), low);
                            case CtpObjectSymbols.NumericBytes8:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, 0, (int)ReadBits32(data, ref currentPosition, endPosition), low);
                            case CtpObjectSymbols.NumericBytes9:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                mid = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, (int)ReadBits8(data, ref currentPosition, endPosition), mid, low);
                            case CtpObjectSymbols.NumericBytes10:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                mid = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, (int)ReadBits16(data, ref currentPosition, endPosition), mid, low);
                            case CtpObjectSymbols.NumericBytes11:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                mid = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, (int)ReadBits24(data, ref currentPosition, endPosition), mid, low);
                            case CtpObjectSymbols.NumericElse:
                                low = (int)ReadBits32(data, ref currentPosition, endPosition);
                                mid = (int)ReadBits32(data, ref currentPosition, endPosition);
                                return new CtpNumeric(flags, (int)ReadBits32(data, ref currentPosition, endPosition), mid, low);
                            default:
                                throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private static CtpObject ReadTime(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            if (symbol == CtpObjectSymbols.CtpTimeZero)
                return new CtpTime(0);
            return new CtpTime((long)ReadBits64(data, ref currentPosition, endPosition));
        }

        private static CtpObject ReadBuffer(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            int length = symbol - CtpObjectSymbols.CtpBuffer0;
            if (symbol == CtpObjectSymbols.CtpBufferElse)
            {
                var lenObj = Read(data, ref currentPosition, endPosition);
                if (currentPosition > endPosition)
                {
                    return CtpObject.Null;
                }
                length = (int)lenObj;
            }
            if (currentPosition + length > endPosition)
            {
                currentPosition += length;
                return CtpObject.Null;
            }
            byte[] rv = new byte[length];
            Array.Copy(data, currentPosition, rv, 0, length);
            currentPosition += length;
            return rv;
        }

        private static CtpObject ReadString(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            int length = symbol - CtpObjectSymbols.String0;
            if (symbol == CtpObjectSymbols.StringElse)
            {
                var lenObj = Read(data, ref currentPosition, endPosition);
                if (currentPosition > endPosition)
                {
                    return CtpObject.Null;
                }
                length = (int)lenObj;
            }
            if (currentPosition + length > endPosition)
            {
                currentPosition += length;
                return CtpObject.Null;
            }
            byte[] rv = new byte[length];
            Array.Copy(data, currentPosition, rv, 0, length);
            currentPosition += length;
            return Encoding.UTF8.GetString(rv);
        }

        private static CtpObject ReadCommand(byte[] data, ref int currentPosition, int endPosition)
        {
            var lenObj = Read(data, ref currentPosition, endPosition);
            if (currentPosition > endPosition)
            {
                return CtpObject.Null;
            }
            int length = (int)lenObj;
            if (currentPosition + length > endPosition)
            {
                currentPosition += length;
                return CtpObject.Null;
            }
            byte[] rv = new byte[length];
            Array.Copy(data, currentPosition, rv, 0, length);
            currentPosition += length;
            return new CtpCommand(rv);
        }

        private static uint ReadBits8(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 1 > endPosition)
            {
                currentPosition++;
                return 0;
            }
            byte rv = data[currentPosition];
            currentPosition++;
            return rv;
        }

        private static uint ReadBits16(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 2 > endPosition)
            {
                currentPosition += 2;
                return 0;
            }
            uint rv = (uint)data[currentPosition] << 8
                    | (uint)data[currentPosition + 1];
            currentPosition += 2;
            return rv;
        }

        private static uint ReadBits24(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 3 > endPosition)
            {
                currentPosition += 3;
                return 0;
            }
            uint rv = (uint)data[currentPosition] << 16
                      | (uint)data[currentPosition + 1] << 8
                      | (uint)data[currentPosition + 2];
            currentPosition += 3;
            return rv;
        }

        private static uint ReadBits32(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 4 > endPosition)
            {
                currentPosition += 4;
                return 0;
            }
            uint rv = (uint)data[currentPosition] << 24
                      | (uint)data[currentPosition + 1] << 16
                      | (uint)data[currentPosition + 2] << 8
                      | (uint)data[currentPosition + 3];
            currentPosition += 4;
            return rv;
        }

        private static ulong ReadBits40(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 5 > endPosition)
            {
                currentPosition += 5;
                return 0;
            }
            ulong rv = (ulong)data[currentPosition + 0] << 32 |
                       (ulong)data[currentPosition + 1] << 24 |
                       (ulong)data[currentPosition + 2] << 16 |
                       (ulong)data[currentPosition + 3] << 8 |
                       (ulong)data[currentPosition + 4];
            currentPosition += 5;
            return rv;
        }

        private static ulong ReadBits48(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 6 > endPosition)
            {
                currentPosition += 6;
                return 0;
            }
            ulong rv = (ulong)data[currentPosition + 0] << 40 |
                       (ulong)data[currentPosition + 1] << 32 |
                       (ulong)data[currentPosition + 2] << 24 |
                       (ulong)data[currentPosition + 3] << 16 |
                       (ulong)data[currentPosition + 4] << 8 |
                       (ulong)data[currentPosition + 5];
            currentPosition += 6;
            return rv;
        }

        private static ulong ReadBits56(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 7 > endPosition)
            {
                currentPosition += 7;
                return 0;
            }
            ulong rv = (ulong)data[currentPosition + 0] << 48 |
                       (ulong)data[currentPosition + 1] << 40 |
                       (ulong)data[currentPosition + 2] << 32 |
                       (ulong)data[currentPosition + 3] << 24 |
                       (ulong)data[currentPosition + 4] << 16 |
                       (ulong)data[currentPosition + 5] << 8 |
                       (ulong)data[currentPosition + 6];
            currentPosition += 7;
            return rv;
        }

        private static ulong ReadBits64(byte[] data, ref int currentPosition, int endPosition)
        {
            if (currentPosition + 8 > endPosition)
            {
                currentPosition += 8;
                return 0;
            }
            ulong rv = (ulong)data[currentPosition + 0] << 56 |
                      (ulong)data[currentPosition + 1] << 48 |
                      (ulong)data[currentPosition + 2] << 40 |
                      (ulong)data[currentPosition + 3] << 32 |
                      (ulong)data[currentPosition + 4] << 24 |
                      (ulong)data[currentPosition + 5] << 16 |
                      (ulong)data[currentPosition + 6] << 8 |
                      (ulong)data[currentPosition + 7];
            currentPosition += 8;
            return rv;
        }

        internal static bool TryReadPacket(byte[] data, int position, int length, int maximumPacketSize, out PacketContents payloadType, out long payloadFlags, out byte[] payloadBuffer, out int consumedLength)
        {
            if (length > maximumPacketSize)
                throw new Exception("Command size is too large");

            int startPosition = position;
            var eos = position + length;

            var pType = Read(data, ref position, eos);
            var pFlags = Read(data, ref position, eos);
            var pData = Read(data, ref position, eos);

            if (position > eos)
            {
                payloadType = PacketContents.CommandSchema;
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
                consumedLength = position - startPosition;
                return true;
            }

        }

    }
}
