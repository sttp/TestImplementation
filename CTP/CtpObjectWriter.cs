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

        public void CopyTo(byte[] data, int offset)
        {
            Array.Copy(m_buffer, 0, data, offset, m_length);
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
                    WriteSymbol(CtpObjectSymbols.Null);
                    break;
                case CtpTypeCode.Integer:
                    WriteInt(value.IsInteger);
                    break;
                case CtpTypeCode.Single:
                    WriteSingle(value.UnsafeRawInt32);
                    break;
                case CtpTypeCode.Double:
                    WriteDouble(value.UnsafeRawInt64);
                    break;
                case CtpTypeCode.Numeric:
                    WriteNumeric(value.IsNumeric);
                    break;
                case CtpTypeCode.CtpTime:
                    WriteTime(value.IsCtpTime);
                    break;
                case CtpTypeCode.Boolean:
                    if (value.IsBoolean)
                        WriteSymbol(CtpObjectSymbols.BoolTrue);
                    else
                        WriteSymbol(CtpObjectSymbols.BoolFalse);
                    break;
                case CtpTypeCode.Guid:
                    WriteGuid(value.IsGuid);
                    break;
                case CtpTypeCode.String:
                    WriteString(value.IsString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    WriteBuffer(value.IsCtpBuffer);
                    break;
                case CtpTypeCode.CtpCommand:
                    WriteCommand(value.IsCtpCommand);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WriteInt(long value)
        {
            if (-2 <= value && value <= 64)
            {
                WriteSymbol(CtpObjectSymbols.IntNeg2 + (byte)(value + 2));
            }
            else if ((ulong)value > Bits56)
            {
                WriteSymbol(CtpObjectSymbols.IntBits64);
                WriteBits64((ulong)value);
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
                    WriteSymbol(CtpObjectSymbols.IntBits8Pos + adder);
                    WriteBits8((uint)value);
                }
                else if (value <= Bits16)
                {
                    WriteSymbol(CtpObjectSymbols.IntBits16Pos + adder);
                    WriteBits16((uint)value);
                }
                else if (value <= Bits24)
                {
                    WriteSymbol(CtpObjectSymbols.IntBits24Pos + adder);
                    WriteBits24((uint)value);
                }
                else if (value <= Bits32)
                {
                    WriteSymbol(CtpObjectSymbols.IntBits32Pos + adder);
                    WriteBits32((uint)value);
                }
                else if (value <= (long)Bits40)
                {
                    WriteSymbol(CtpObjectSymbols.IntBits40Pos + adder);
                    WriteBits40((ulong)value);
                }
                else if (value <= (long)Bits48)
                {
                    WriteSymbol(CtpObjectSymbols.IntBits48Pos + adder);
                    WriteBits48((ulong)value);
                }
                else
                {
                    WriteSymbol(CtpObjectSymbols.IntBits56Pos + adder);
                    WriteBits56((ulong)value);
                }
            }
        }

        private void WriteSingle(uint value)
        {
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
                    WriteSymbol(CtpObjectSymbols.Single56 + (byte)(firstByte - 56));
                    WriteBits24(value);
                }
                else if (184 <= firstByte && firstByte <= 207)
                {
                    WriteSymbol(CtpObjectSymbols.Single184 + (byte)(firstByte - 184));
                    WriteBits24(value);
                }
                else
                {
                    WriteSymbol(CtpObjectSymbols.SingleElse);
                    WriteBits32(value);
                }
            }
        }

        private void WriteDouble(ulong value)
        {
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
                    WriteSymbol(CtpObjectSymbols.Double63 + (byte)(firstByte - 63));
                    WriteBits56(value);
                }
                else if (191 <= firstByte && firstByte <= 193)
                {
                    WriteSymbol(CtpObjectSymbols.Double191 + (byte)(firstByte - 191));
                    WriteBits56(value);
                }
                else
                {
                    WriteSymbol(CtpObjectSymbols.DoubleElse);
                    WriteBits64(value);
                }
            }
        }

        private void WriteNumeric(CtpNumeric value)
        {
            if (value.High != 0)
            {
                WriteSymbol(CtpObjectSymbols.NumericHigh);
                WriteNumericHelper(value.Flags, value.High);
                WriteBits32(value.Mid);
                WriteBits32(value.Low);
            }
            else if (value.Mid != 0)
            {
                WriteSymbol(CtpObjectSymbols.NumericMid);
                WriteNumericHelper(value.Flags, value.Mid);
                WriteBits32(value.Low);
            }
            else if (value.Low != 0)
            {
                WriteSymbol(CtpObjectSymbols.NumericLow);
                WriteNumericHelper(value.Flags, value.Low);
            }
            else
            {
                WriteSymbol(CtpObjectSymbols.NumericNone);
                WriteBits8(value.Flags);
            }
        }

        private void WriteNumericHelper(byte flags, uint value)
        {
            if (value > Bits24)
            {
                WriteBits8(flags + 192u);
                WriteBits32(value);
            }
            else if (value > Bits16)
            {
                WriteBits8(flags + 128u);
                WriteBits24(value);
            }
            else if (value > Bits8)
            {
                WriteBits8(flags + 64u);
                WriteBits16(value);
            }
            else
            {
                WriteBits8(flags);
                WriteBits8(value);
            }
        }

        private void WriteTime(CtpTime value)
        {
            if (value.Ticks == 0)
            {
                WriteSymbol(CtpObjectSymbols.CtpTimeZero);
            }
            else
            {
                byte firstByte = (byte)(value.Ticks >> 56);
                if (14 <= firstByte && firstByte <= 17)
                {
                    WriteSymbol(CtpObjectSymbols.CtpTime14 + (byte)(firstByte - 14));
                    WriteBits56((ulong)value.Ticks);
                }
                else
                {
                    WriteSymbol(CtpObjectSymbols.CtpTimeElse);
                    WriteBits64((ulong)value.Ticks);
                }
            }
        }

        private void WriteGuid(Guid value)
        {
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
                WriteBits8((uint)baseSymbol + 3u);
                WriteBits32((uint)length);
            }
            else if (length > Bits16)
            {
                WriteBits8((uint)baseSymbol + 2u);
                WriteBits24((uint)length);
            }
            else if (length > Bits8)
            {
                WriteBits8((uint)baseSymbol + 1u);
                WriteBits16((uint)length);
            }
            else
            {
                WriteBits8((uint)baseSymbol + 0u);
                WriteBits8((uint)length);
            }
        }

        private void WriteSymbol(CtpObjectSymbols symbol)
        {
            EnsureCapacityBytes(1);
            WriteBits8((uint)symbol);
        }

        private void WriteBits8(uint value)
        {
            EnsureCapacityBytes(1);
            m_buffer[m_length + 0] = (byte)value;
            m_length += 1;
        }

        private void WriteBits16(uint value)
        {
            EnsureCapacityBytes(2);
            m_buffer[m_length + 0] = (byte)(value >> 8);
            m_buffer[m_length + 1] = (byte)value;
            m_length += 2;
        }

        private void WriteBits24(uint value)
        {
            EnsureCapacityBytes(3);
            m_buffer[m_length + 0] = (byte)(value >> 16);
            m_buffer[m_length + 1] = (byte)(value >> 8);
            m_buffer[m_length + 2] = (byte)value;
            m_length += 3;
        }

        private void WriteBits32(uint value)
        {
            EnsureCapacityBytes(4);
            m_buffer[m_length + 0] = (byte)(value >> 24);
            m_buffer[m_length + 1] = (byte)(value >> 16);
            m_buffer[m_length + 2] = (byte)(value >> 8);
            m_buffer[m_length + 3] = (byte)value;
            m_length += 4;
        }

        private void WriteBits40(ulong value)
        {
            EnsureCapacityBytes(5);
            m_buffer[m_length + 0] = (byte)(value >> 32);
            m_buffer[m_length + 1] = (byte)(value >> 24);
            m_buffer[m_length + 2] = (byte)(value >> 16);
            m_buffer[m_length + 3] = (byte)(value >> 8);
            m_buffer[m_length + 4] = (byte)value;
            m_length += 5;
        }

        private void WriteBits48(ulong value)
        {
            EnsureCapacityBytes(6);
            m_buffer[m_length + 0] = (byte)(value >> 40);
            m_buffer[m_length + 1] = (byte)(value >> 32);
            m_buffer[m_length + 2] = (byte)(value >> 24);
            m_buffer[m_length + 3] = (byte)(value >> 16);
            m_buffer[m_length + 4] = (byte)(value >> 8);
            m_buffer[m_length + 5] = (byte)value;
            m_length += 6;
        }

        private void WriteBits56(ulong value)
        {
            EnsureCapacityBytes(7);
            m_buffer[m_length + 0] = (byte)(value >> 48);
            m_buffer[m_length + 1] = (byte)(value >> 40);
            m_buffer[m_length + 2] = (byte)(value >> 32);
            m_buffer[m_length + 3] = (byte)(value >> 24);
            m_buffer[m_length + 4] = (byte)(value >> 16);
            m_buffer[m_length + 5] = (byte)(value >> 8);
            m_buffer[m_length + 6] = (byte)value;
            m_length += 7;
        }

        private void WriteBits64(ulong value)
        {
            EnsureCapacityBytes(8);
            m_buffer[m_length + 0] = (byte)(value >> 56);
            m_buffer[m_length + 1] = (byte)(value >> 48);
            m_buffer[m_length + 2] = (byte)(value >> 40);
            m_buffer[m_length + 3] = (byte)(value >> 32);
            m_buffer[m_length + 4] = (byte)(value >> 24);
            m_buffer[m_length + 5] = (byte)(value >> 16);
            m_buffer[m_length + 6] = (byte)(value >> 8);
            m_buffer[m_length + 7] = (byte)value;
            m_length += 8;
        }

        public void CopyTo(CtpObjectWriter wr)
        {
            wr.Write(m_buffer, 0, m_length);
        }


    }
}

