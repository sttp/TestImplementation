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
            int currentPosition = m_currentBytePosition;
            var rv = Read(m_buffer, ref currentPosition, m_endOfByteStream);
            if (currentPosition > m_endOfByteStream)
                throw new EndOfStreamException();
            m_currentBytePosition = currentPosition;
            return rv;
        }

        #region [ Static Methods ]

        /// <summary>
        /// Attempts to read the next object stored on the buffer.
        /// Note: if reading past the end of the stream, the return value will be <see cref="CtpObject.Null"/>
        /// and <see cref="currentPosition"/> will be greater than <see cref="endPosition"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentPosition"></param>
        /// <param name="endPosition"></param>
        /// <returns></returns>
        public static CtpObject Read(byte[] data, ref int currentPosition, int endPosition)
        {
            CtpObjectSymbols symbol = (CtpObjectSymbols)ReadBits8(data, ref currentPosition, endPosition);
            if (currentPosition > endPosition)
                return CtpObject.Null;

            if (symbol <= CtpObjectSymbols.Null)
                return CtpObject.Null;
            if (symbol <= CtpObjectSymbols.IntElse)
                return ReadInteger(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.SingleElse)
                return ReadSingle(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.DoubleElse)
                return ReadDouble(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.NumericElse)
                return ReadNumeric(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.CtpTimeElse)
                return ReadTime(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.BoolElse)
                return symbol == CtpObjectSymbols.BoolTrue;
            if (symbol <= CtpObjectSymbols.GuidElse)
                return ReadGuid(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.StringElse)
                return ReadString(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.CtpBufferElse)
                return ReadBuffer(symbol, data, ref currentPosition, endPosition);
            if (symbol <= CtpObjectSymbols.CtpCommandElse)
                return ReadCommand(symbol, data, ref currentPosition, endPosition);
            throw new ArgumentOutOfRangeException();
        }

        private static CtpObject ReadInteger(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            if (symbol <= CtpObjectSymbols.IntMaxRunLen)
                return (int)symbol - (int)CtpObjectSymbols.Int0;

            if (symbol == CtpObjectSymbols.IntElse)
                return (long)ReadBits64(data, ref currentPosition, endPosition);

            ulong negate = 0;
            if (symbol >= CtpObjectSymbols.IntBits8Neg)
            {
                negate = ulong.MaxValue;
                symbol -= (CtpObjectSymbols.IntBits8Neg - CtpObjectSymbols.IntBits8Pos);
            }

            switch (symbol)
            {
                case CtpObjectSymbols.IntBits8Pos:
                    return (long)(ReadBits8(data, ref currentPosition, endPosition) ^ negate);
                case CtpObjectSymbols.IntBits16Pos:
                    return (long)(ReadBits16(data, ref currentPosition, endPosition) ^ negate);
                case CtpObjectSymbols.IntBits24Pos:
                    return (long)(ReadBits24(data, ref currentPosition, endPosition) ^ negate);
                case CtpObjectSymbols.IntBits32Pos:
                    return (long)(ReadBits32(data, ref currentPosition, endPosition) ^ negate);
                case CtpObjectSymbols.IntBits40Pos:
                    return (long)(ReadBits40(data, ref currentPosition, endPosition) ^ negate);
                case CtpObjectSymbols.IntBits48Pos:
                    return (long)(ReadBits48(data, ref currentPosition, endPosition) ^ negate);
                case CtpObjectSymbols.IntBits56Pos:
                    return (long)(ReadBits56(data, ref currentPosition, endPosition) ^ negate);
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private static CtpObject ReadSingle(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            uint value;
            if (symbol == CtpObjectSymbols.SingleNeg1)
                return -1f;
            if (symbol == CtpObjectSymbols.Single0)
                return 0f;
            if (symbol == CtpObjectSymbols.Single1)
                return 1f;
            if (symbol <= CtpObjectSymbols.Single79)
            {
                value = ReadBits24(data, ref currentPosition, endPosition) | ((56u + (symbol - CtpObjectSymbols.Single56)) << 24);
                return *(float*)&value;
            }
            if (symbol <= CtpObjectSymbols.Single207)
            {
                value = ReadBits24(data, ref currentPosition, endPosition) | ((184u + (symbol - CtpObjectSymbols.Single184)) << 24);
                return *(float*)&value;
            }

            value = ReadBits32(data, ref currentPosition, endPosition);
            return *(float*)&value;
        }

        private static CtpObject ReadDouble(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            ulong value;
            if (symbol == CtpObjectSymbols.DoubleNeg1)
                return -1.0;
            if (symbol == CtpObjectSymbols.Double0)
                return 0.0;
            if (symbol == CtpObjectSymbols.Double1)
                return 1.0;
            if (symbol <= CtpObjectSymbols.Double65)
            {
                value = ReadBits56(data, ref currentPosition, endPosition) | ((63ul + (symbol - CtpObjectSymbols.Double63)) << 56);
                return *(double*)&value;
            }
            if (symbol <= CtpObjectSymbols.Double193)
            {
                value = ReadBits56(data, ref currentPosition, endPosition) | ((191ul + (symbol - CtpObjectSymbols.Double191)) << 56);
                return *(double*)&value;
            }

            value = ReadBits64(data, ref currentPosition, endPosition);
            return *(double*)&value;
        }

        private static CtpObject ReadNumeric(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            byte flags;
            uint low;
            uint mid;
            uint high;
            switch (symbol)
            {
                case CtpObjectSymbols.NumericHigh:
                    flags = (byte)ReadBits8(data, ref currentPosition, endPosition);
                    high = ReadNumericHelper(data, ref currentPosition, endPosition, flags);
                    mid = ReadBits32(data, ref currentPosition, endPosition);
                    low = ReadBits32(data, ref currentPosition, endPosition);
                    return new CtpNumeric((byte)(flags & 63), high, mid, low);
                case CtpObjectSymbols.NumericMid:
                    flags = (byte)ReadBits8(data, ref currentPosition, endPosition);
                    mid = ReadNumericHelper(data, ref currentPosition, endPosition, flags);
                    low = ReadBits32(data, ref currentPosition, endPosition);
                    return new CtpNumeric((byte)(flags & 63), 0, mid, low);
                case CtpObjectSymbols.NumericLow:
                    flags = (byte)ReadBits8(data, ref currentPosition, endPosition);
                    low = ReadNumericHelper(data, ref currentPosition, endPosition, flags);
                    return new CtpNumeric((byte)(flags & 63), 0, 0, low);
                case CtpObjectSymbols.NumericNone:
                    flags = (byte)ReadBits8(data, ref currentPosition, endPosition);
                    return new CtpNumeric((byte)(flags & 63), 0, 0, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        private static uint ReadNumericHelper(byte[] data, ref int currentPosition, int endPosition, byte flags)
        {
            switch (flags >> 6)
            {
                case 0:
                    return ReadBits8(data, ref currentPosition, endPosition);
                case 1:
                    return ReadBits16(data, ref currentPosition, endPosition);
                case 2:
                    return ReadBits24(data, ref currentPosition, endPosition);
                default:
                    return ReadBits32(data, ref currentPosition, endPosition);
            }
        }

        private static CtpObject ReadTime(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            if (symbol == CtpObjectSymbols.CtpTimeZero)
                return new CtpTime(0);

            ulong value;
            if (symbol <= CtpObjectSymbols.CtpTime17)
            {
                value = ReadBits56(data, ref currentPosition, endPosition) | ((14ul + (symbol - CtpObjectSymbols.CtpTime14)) << 56);
                return new CtpTime((long)value);
            }

            value = ReadBits64(data, ref currentPosition, endPosition);
            return new CtpTime((long)value);
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

        private static CtpObject ReadString(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            int length;
            if (symbol <= CtpObjectSymbols.String30)
                length = symbol - CtpObjectSymbols.String0;
            else
                length = ReadArrayLength(symbol - CtpObjectSymbols.String8Bit, data, ref currentPosition, endPosition);
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

        private static CtpObject ReadBuffer(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            int length;
            if (symbol <= CtpObjectSymbols.CtpBuffer50)
                length = symbol - CtpObjectSymbols.CtpBuffer0;
            else
                length = ReadArrayLength(symbol - CtpObjectSymbols.CtpBuffer8Bit, data, ref currentPosition, endPosition);
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

        private static CtpObject ReadCommand(CtpObjectSymbols symbol, byte[] data, ref int currentPosition, int endPosition)
        {
            int length = ReadArrayLength(symbol - CtpObjectSymbols.CtpCommand8Bit, data, ref currentPosition, endPosition);
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

        private static int ReadArrayLength(int bits, byte[] data, ref int currentPosition, int endPosition)
        {
            switch (bits)
            {
                case 0:
                    return (int)ReadBits8(data, ref currentPosition, endPosition);
                case 1:
                    return (int)ReadBits16(data, ref currentPosition, endPosition);
                case 2:
                    return (int)ReadBits24(data, ref currentPosition, endPosition);
                default:
                    return (int)ReadBits32(data, ref currentPosition, endPosition);
            }
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

        #endregion



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
