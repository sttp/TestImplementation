using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CTP;
using CTP.Collection;
using GSF;

namespace CTP
{
    public class CtpObjectWriter
    {
        const ulong Bits56 = 0xFFFFFFFFFFFFFFu;
        const ulong Bits48 = 0xFFFFFFFFFFFFu;
        const ulong Bits40 = 0xFFFFFFFFFFu;
        const uint Bits32 = 0xFFFFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits8 = 0xFFu;

        private byte[] m_buffer;
        private int m_length;

        public CtpObjectWriter()
        {
            m_buffer = new byte[64];
            Clear();
        }

        public int Length => m_length;

        /// <summary>
        /// Ensures that the byte stream has the room to store the specified number of bytes.
        /// </summary>
        /// <param name="neededBytes"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacityBytes(int neededBytes)
        {
            if (m_length + neededBytes >= m_buffer.Length)
                GrowBytes(neededBytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowBytes(int neededBytes)
        {
            while (m_length + neededBytes >= m_buffer.Length)
            {
                byte[] newBuffer = new byte[m_buffer.Length * 2];
                m_buffer.CopyTo(newBuffer, 0);
                m_buffer = newBuffer;
            }
        }

        public PooledBuffer TakeBuffer()
        {
            return PooledBuffer.Create(m_buffer, 0, m_length);
        }

        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            CopyTo(data, 0);
            return data;
        }

        private void CopyTo(byte[] data, int offset)
        {
            Array.Copy(m_buffer, 0, data, offset, m_length);
        }

        public void CopyToAsCtpBuffer(CtpObjectWriter wr)
        {
            wr.Write(m_buffer, 0, m_length);
        }

        public void Clear()
        {
            m_length = 0;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }

        public void Write(byte[] value)
        {
            if (value == null)
                Write(CtpObject.Null);
            else
                WriteBuffer(value, 0, value.Length);
        }

        public void Write(byte[] value, int offset, int length)
        {
            if (value == null)
            {
                Write(CtpObject.Null);
            }
            else
            {
                value.ValidateParameters(offset, length);
                WriteBuffer(value, offset, length);
            }
        }

        public void Write(CtpObject value)
        {
            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    WriteNull();
                    break;
                case CtpTypeCode.Integer:
                    WriteInt(value.UnsafeInteger);
                    break;
                case CtpTypeCode.Single:
                    WriteSingle(value.UnsafeRawUInt32);
                    break;
                case CtpTypeCode.Double:
                    WriteDouble(value.UnsafeRawUInt64);
                    break;
                case CtpTypeCode.Numeric:
                    WriteNumeric(value.UnsafeNumeric);
                    break;
                case CtpTypeCode.CtpTime:
                    WriteTime(value.UnsafeCtpTime);
                    break;
                case CtpTypeCode.Boolean:
                    WriteBool(value.UnsafeBoolean);
                    break;
                case CtpTypeCode.Guid:
                    WriteGuid(value.UnsafeGuid);
                    break;
                case CtpTypeCode.String:
                    WriteString(value.UnsafeString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    WriteBuffer(value.UnsafeCtpBuffer);
                    break;
                case CtpTypeCode.CtpCommand:
                    WriteCommand(value.UnsafeCtpCommand);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WriteNull()
        {
            EnsureCapacityBytes(1);
            WriteSymbol(CtpObjectSymbols.Null);
        }

        private void WriteBool(bool value)
        {
            EnsureCapacityBytes(1);
            if (value)
                WriteSymbol(CtpObjectSymbols.BoolTrue);
            else
                WriteSymbol(CtpObjectSymbols.BoolFalse);
        }


        public void WriteExact(long value)
        {
            WriteInt(value);
        }

        public unsafe void WriteExact(float value)
        {
            WriteSingle(*(uint*)&value);
        }

        public unsafe void WriteExact(double value)
        {
            WriteDouble(*(ulong*)&value);
        }

        private void WriteInt(long value)
        {
            EnsureCapacityBytes(9);
            if (-2 <= value && value <= 64)
            {
                WriteSymbol(CtpObjectSymbols.IntNeg2 + (byte)(value + 2));
            }
            else if ((ulong)value > Bits56)
            {
                WriteBits64(CtpObjectSymbols.IntBits64, (ulong)value);
            }
            else
            {
                byte adder = 0;
                if (value < 0)
                {
                    value = ~value; //1's complement not 2. This ensures that the new value cannot be negative.
                    adder = CtpObjectSymbols.IntBits8Neg - CtpObjectSymbols.IntBits8Pos;
                }
                if (value <= Bits8)
                {
                    WriteBits8(CtpObjectSymbols.IntBits8Pos + adder, (uint)value);
                }
                else if (value <= Bits16)
                {
                    WriteBits16(CtpObjectSymbols.IntBits16Pos + adder, (uint)value);
                }
                else if (value <= Bits24)
                {
                    WriteBits24(CtpObjectSymbols.IntBits24Pos + adder, (uint)value);
                }
                else if (value <= Bits32)
                {
                    WriteBits32(CtpObjectSymbols.IntBits32Pos + adder, (uint)value);
                }
                else if (value <= (long)Bits40)
                {
                    WriteBits40(CtpObjectSymbols.IntBits40Pos + adder, (ulong)value);
                }
                else if (value <= (long)Bits48)
                {
                    WriteBits48(CtpObjectSymbols.IntBits48Pos + adder, (ulong)value);
                }
                else
                {
                    WriteBits56(CtpObjectSymbols.IntBits56Pos + adder, (ulong)value);
                }
            }
        }

        private void WriteSingle(uint value)
        {
            EnsureCapacityBytes(5);

            if (value == NumericConstants.SingleNeg1)
                WriteSymbol(CtpObjectSymbols.SingleNeg1);
            else if (value == NumericConstants.Single0)
                WriteSymbol(CtpObjectSymbols.Single0);
            else if (value == NumericConstants.Single1)
                WriteSymbol(CtpObjectSymbols.Single1);
            else
            {
                byte firstByte = (byte)(value >> 24);
                if (56 <= firstByte && firstByte <= 79)
                {
                    WriteBits24(CtpObjectSymbols.Single56 + (byte)(firstByte - 56), value);
                }
                else if (184 <= firstByte && firstByte <= 207)
                {
                    WriteBits24(CtpObjectSymbols.Single184 + (byte)(firstByte - 184), value);
                }
                else
                {
                    WriteBits32(CtpObjectSymbols.SingleElse, value);
                }
            }
        }

        private void WriteDouble(ulong value)
        {
            EnsureCapacityBytes(9);

            if (value == NumericConstants.DoubleNeg1)
                WriteSymbol(CtpObjectSymbols.DoubleNeg1);
            else if (value == NumericConstants.Double0)
                WriteSymbol(CtpObjectSymbols.Double0);
            else if (value == NumericConstants.Double1)
                WriteSymbol(CtpObjectSymbols.Double1);
            else
            {
                byte firstByte = (byte)(value >> 56);
                if (63 <= firstByte && firstByte <= 65)
                {
                    WriteBits56(CtpObjectSymbols.Double63 + (byte)(firstByte - 63), value);
                }
                else if (191 <= firstByte && firstByte <= 193)
                {
                    WriteBits56(CtpObjectSymbols.Double191 + (byte)(firstByte - 191), value);
                }
                else
                {
                    WriteBits64(CtpObjectSymbols.DoubleElse, value);
                }
            }
        }

        private void WriteNumeric(CtpNumeric value)
        {
            EnsureCapacityBytes(4 + 4 + 4 + 1 + 1);

            if (value.High != 0)
            {
                WriteNumericHelper(CtpObjectSymbols.NumericHigh, value.Flags, value.High);
                WriteBits32(value.Mid);
                WriteBits32(value.Low);
            }
            else if (value.Mid != 0)
            {
                WriteNumericHelper(CtpObjectSymbols.NumericMid, value.Flags, value.Mid);
                WriteBits32(value.Low);
            }
            else if (value.Low != 0)
            {
                WriteNumericHelper(CtpObjectSymbols.NumericLow, value.Flags, value.Low);
            }
            else
            {
                WriteBits8(CtpObjectSymbols.NumericNone, value.Flags);
            }
        }

        private void WriteNumericHelper(CtpObjectSymbols symbol, byte flags, uint value)
        {
            if (value > Bits24)
            {
                m_buffer[m_length + 0] = (byte)symbol;
                m_buffer[m_length + 1] = (byte)(flags + 192u);
                m_buffer[m_length + 2] = (byte)(value >> 24);
                m_buffer[m_length + 3] = (byte)(value >> 16);
                m_buffer[m_length + 4] = (byte)(value >> 8);
                m_buffer[m_length + 5] = (byte)value;
                m_length += 6;
            }
            else if (value > Bits16)
            {
                m_buffer[m_length + 0] = (byte)symbol;
                m_buffer[m_length + 1] = (byte)(flags + 128u);
                m_buffer[m_length + 2] = (byte)(value >> 16);
                m_buffer[m_length + 3] = (byte)(value >> 8);
                m_buffer[m_length + 4] = (byte)value;
                m_length += 5;
            }
            else if (value > Bits8)
            {
                m_buffer[m_length + 0] = (byte)symbol;
                m_buffer[m_length + 1] = (byte)(flags + 64u);
                m_buffer[m_length + 2] = (byte)(value >> 8);
                m_buffer[m_length + 3] = (byte)value;
                m_length += 4;
            }
            else
            {
                m_buffer[m_length + 0] = (byte)symbol;
                m_buffer[m_length + 1] = (byte)flags;
                m_buffer[m_length + 2] = (byte)value;
                m_length += 3;
            }
        }

        private void WriteTime(CtpTime value)
        {
            EnsureCapacityBytes(9);
            if (value.Ticks == 0)
            {
                WriteSymbol(CtpObjectSymbols.CtpTimeZero);
            }
            else
            {
                byte firstByte = (byte)(value.Ticks >> 56);
                if (14 <= firstByte && firstByte <= 17)
                {
                    WriteBits56(CtpObjectSymbols.CtpTime14 + (byte)(firstByte - 14), (ulong)value.Ticks);
                }
                else
                {
                    WriteBits64(CtpObjectSymbols.CtpTimeElse, (ulong)value.Ticks);
                }
            }
        }

        private void WriteGuid(Guid value)
        {
            EnsureCapacityBytes(17);
            if (value == Guid.Empty)
            {
                WriteSymbol(CtpObjectSymbols.GuidEmpty);
            }
            else
            {
                WriteSymbol(CtpObjectSymbols.GuidElse);
                EnsureCapacityBytes(16);
                Array.Copy(value.ToRfcBytes(), 0, m_buffer, m_length, 16);
                m_length += 16;
            }
        }

        private void WriteString(string value)
        {
            EnsureCapacityBytes(5);
            int length = Encoding.UTF8.GetByteCount(value);
            if (length <= 30)
                WriteSymbol(CtpObjectSymbols.String0 + (byte)length);
            else
                WriteArrayLength(CtpObjectSymbols.String8Bit, length);
            EnsureCapacityBytes(length);
            if (length != Encoding.UTF8.GetBytes(value, 0, value.Length, m_buffer, m_length))
                throw new Exception("Encoding Error");
            m_length += length;
        }

        private void WriteBuffer(CtpBuffer value)
        {
            EnsureCapacityBytes(5);
            if (value.Length <= 50)
                WriteSymbol(CtpObjectSymbols.CtpBuffer0 + (byte)value.Length);
            else
                WriteArrayLength(CtpObjectSymbols.CtpBuffer8Bit, value.Length);
            EnsureCapacityBytes(value.Length);
            value.CopyTo(m_buffer, m_length);
            m_length += value.Length;
        }

        private void WriteBuffer(byte[] value, int offset, int length)
        {
            EnsureCapacityBytes(5);
            if (length <= 50)
                WriteSymbol(CtpObjectSymbols.CtpBuffer0 + (byte)length);
            else
                WriteArrayLength(CtpObjectSymbols.CtpBuffer8Bit, length);
            EnsureCapacityBytes(length);
            Array.Copy(value, offset, m_buffer, m_length, length);
            m_length += length;
        }

        private void WriteCommand(CtpCommand value)
        {
            EnsureCapacityBytes(5);
            int length = value.LengthWithSchema;
            WriteArrayLength(CtpObjectSymbols.CtpCommand8Bit, length);
            EnsureCapacityBytes(length);
            value.CopyTo(m_buffer, m_length);
            m_length += length;
        }

        private void WriteArrayLength(CtpObjectSymbols baseSymbol, int length)
        {
            if (length > Bits24)
            {
                WriteBits32(baseSymbol + 3, (uint)length);
            }
            else if (length > Bits16)
            {
                WriteBits24(baseSymbol + 2, (uint)length);
            }
            else if (length > Bits8)
            {
                WriteBits16(baseSymbol + 1, (uint)length);
            }
            else
            {
                WriteBits8(baseSymbol + 0, (uint)length);
            }
        }

        private void WriteSymbol(CtpObjectSymbols symbol)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_length += 1;
        }

        private void WriteBits32(uint value)
        {
            m_buffer[m_length + 0] = (byte)(value >> 24);
            m_buffer[m_length + 1] = (byte)(value >> 16);
            m_buffer[m_length + 2] = (byte)(value >> 8);
            m_buffer[m_length + 3] = (byte)value;
            m_length += 4;
        }

        private void WriteBits8(CtpObjectSymbols symbol, uint value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)value;
            m_length += 2;
        }

        private void WriteBits16(CtpObjectSymbols symbol, uint value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 8);
            m_buffer[m_length + 2] = (byte)value;
            m_length += 3;
        }

        private void WriteBits24(CtpObjectSymbols symbol, uint value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 16);
            m_buffer[m_length + 2] = (byte)(value >> 8);
            m_buffer[m_length + 3] = (byte)value;
            m_length += 4;
        }

        private void WriteBits32(CtpObjectSymbols symbol, uint value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 24);
            m_buffer[m_length + 2] = (byte)(value >> 16);
            m_buffer[m_length + 3] = (byte)(value >> 8);
            m_buffer[m_length + 4] = (byte)value;
            m_length += 5;
        }

        private void WriteBits40(CtpObjectSymbols symbol, ulong value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 32);
            m_buffer[m_length + 2] = (byte)(value >> 24);
            m_buffer[m_length + 3] = (byte)(value >> 16);
            m_buffer[m_length + 4] = (byte)(value >> 8);
            m_buffer[m_length + 5] = (byte)value;
            m_length += 6;
        }

        private void WriteBits48(CtpObjectSymbols symbol, ulong value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 40);
            m_buffer[m_length + 2] = (byte)(value >> 32);
            m_buffer[m_length + 3] = (byte)(value >> 24);
            m_buffer[m_length + 4] = (byte)(value >> 16);
            m_buffer[m_length + 5] = (byte)(value >> 8);
            m_buffer[m_length + 6] = (byte)value;
            m_length += 7;
        }

        private void WriteBits56(CtpObjectSymbols symbol, ulong value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 48);
            m_buffer[m_length + 2] = (byte)(value >> 40);
            m_buffer[m_length + 3] = (byte)(value >> 32);
            m_buffer[m_length + 4] = (byte)(value >> 24);
            m_buffer[m_length + 5] = (byte)(value >> 16);
            m_buffer[m_length + 6] = (byte)(value >> 8);
            m_buffer[m_length + 7] = (byte)value;
            m_length += 8;
        }

        private void WriteBits64(CtpObjectSymbols symbol, ulong value)
        {
            m_buffer[m_length + 0] = (byte)symbol;
            m_buffer[m_length + 1] = (byte)(value >> 56);
            m_buffer[m_length + 2] = (byte)(value >> 48);
            m_buffer[m_length + 3] = (byte)(value >> 40);
            m_buffer[m_length + 4] = (byte)(value >> 32);
            m_buffer[m_length + 5] = (byte)(value >> 24);
            m_buffer[m_length + 6] = (byte)(value >> 16);
            m_buffer[m_length + 7] = (byte)(value >> 8);
            m_buffer[m_length + 8] = (byte)value;
            m_length += 9;
        }



    }
}

