using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CTP;
using GSF;

namespace CTP
{
    public class CtpObjectWriter
    {
        private byte[] m_byteBuffer;
        private int m_byteLength;

        public CtpObjectWriter()
        {
            m_byteBuffer = new byte[64];
            Clear();
        }

        public int Length => m_byteLength;

        /// <summary>
        /// Ensures that the byte stream has the room to store the specified number of bytes.
        /// </summary>
        /// <param name="neededBytes"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacityBytes(int neededBytes)
        {
            if (m_byteLength + neededBytes >= m_byteBuffer.Length)
                GrowBytes(neededBytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowBytes(int neededBytes)
        {
            while (m_byteLength + neededBytes >= m_byteBuffer.Length)
            {
                byte[] newBuffer = new byte[m_byteBuffer.Length * 2];
                m_byteBuffer.CopyTo(newBuffer, 0);
                m_byteBuffer = newBuffer;
            }
        }

        public byte[] ToArray()
        {
            byte[] data = new byte[Length];
            CopyTo(data, 0);
            return data;
        }

        public void CopyTo(byte[] data, int offset)
        {
            Array.Copy(m_byteBuffer, 0, data, offset, m_byteLength);
        }

        public void Clear()
        {
            m_byteLength = 0;
            //Note: Clearing the array isn't required since this class prohibits advancing the position.
            //Array.Clear(m_buffer, 0, m_buffer.Length);
        }

        public void Write(CtpObject value)
        {
            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    WriteSymbol(CtpObjectSymbols.Null);
                    break;
                case CtpTypeCode.Int64:
                    WriteInt64(value.IsInt64);
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
                        WriteSymbol(CtpObjectSymbols.BoolFalse);
                    else
                        WriteSymbol(CtpObjectSymbols.BoolElse);
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

        private void WriteCommand(CtpCommand value)
        {
            WriteBuffer(value.ToArray(), CtpObjectSymbols.CtpCommand0);
        }

        private void WriteBuffer(CtpBuffer value)
        {
            WriteBuffer(value.ToBuffer(), CtpObjectSymbols.CtpBuffer0);
        }

        private void WriteString(string value)
        {
            WriteBuffer(Encoding.UTF8.GetBytes(value), CtpObjectSymbols.String0);
        }

        private void WriteBuffer(byte[] buffer, CtpObjectSymbols itemZero)
        {
            if (buffer.Length <= 19)
            {
                WriteSymbol(itemZero + (byte)buffer.Length);
            }
            else
            {
                WriteSymbol(itemZero + 20);
                Write(buffer.Length);
            }

            Array.Copy(buffer, 0, m_byteBuffer, m_byteLength, buffer.Length);
            m_byteLength += buffer.Length;
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
                Array.Copy(value.ToRfcBytes(), 0, m_byteBuffer, m_byteLength, 16);
                m_byteLength += 16;
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
                WriteSymbol(CtpObjectSymbols.CtpTimeElse);
                WriteBits64((ulong)value.Ticks);
            }
        }

        private void WriteNumeric(CtpNumeric value)
        {
            if (value.Scale == 0 && value.High == 0 && value.Mid == 0)
            {
                if (value.IsNegative && value.Low == 1)
                {
                    WriteSymbol(CtpObjectSymbols.NumericNeg1);
                    return;
                }
                if (!value.IsNegative && value.Low == 0)
                {
                    WriteSymbol(CtpObjectSymbols.Numeric0);
                    return;
                }
                if (!value.IsNegative && value.Low == 1)
                {
                    WriteSymbol(CtpObjectSymbols.Numeric1);
                    return;
                }
            }
            var code = value.Scale;
            if (value.IsNegative)
                code |= 32;

            if ((uint)value.High >= (uint)Bits.Bit24)
            {
                WriteSymbol(CtpObjectSymbols.NumericElse);
                WriteBits8(code);
                WriteBits32((uint)value.High);
                WriteBits32((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.High >= (uint)Bits.Bit16)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes11);
                WriteBits8(code);
                WriteBits24((uint)value.High);
                WriteBits32((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.High >= (uint)Bits.Bit08)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes10);
                WriteBits8(code);
                WriteBits16((uint)value.High);
                WriteBits32((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.High >= (uint)Bits.Bit00)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes9);
                WriteBits8(code);
                WriteBits8((uint)value.High);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.Mid >= (uint)Bits.Bit24)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes8);
                WriteBits8(code);
                WriteBits32((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.Mid >= (uint)Bits.Bit16)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes7);
                WriteBits8(code);
                WriteBits24((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.Mid >= (uint)Bits.Bit08)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes6);
                WriteBits8(code);
                WriteBits16((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.Mid >= (uint)Bits.Bit00)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes5);
                WriteBits8(code);
                WriteBits8((uint)value.Mid);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.Low >= (uint)Bits.Bit24)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes4);
                WriteBits8(code);
                WriteBits32((uint)value.Low);
            }
            else if ((uint)value.Low >= (uint)Bits.Bit16)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes3);
                WriteBits8(code);
                WriteBits24((uint)value.Low);
            }
            else if ((uint)value.Low >= (uint)Bits.Bit08)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes2);
                WriteBits8(code);
                WriteBits16((uint)value.Low);
            }
            else if ((uint)value.Low >= (uint)Bits.Bit00)
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes1);
                WriteBits8(code);
                WriteBits8((uint)value.Low);
            }
            else
            {
                WriteSymbol(CtpObjectSymbols.NumericBytes0);
                WriteBits8(code);
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
                WriteSymbol(CtpObjectSymbols.DoubleElse);
                WriteBits64(value);
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
                WriteSymbol(CtpObjectSymbols.SingleElse);
                WriteBits32(value);
            }
        }

        private void WriteInt64(long value)
        {
            if (-9 <= value && value <= 137)
            {
                WriteSymbol(CtpObjectSymbols.IntNeg9 + (byte)(value + 9));
            }
            else
            {
                if (value < 0)
                {
                    value = ~value;
                    if (value < (long)Bits.Bit08)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits8);
                        WriteBits8((uint)value);
                    }
                    else if (value < (long)Bits.Bit16)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits16);
                        WriteBits16((uint)value);
                    }
                    else if (value < (long)Bits.Bit24)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits24);
                        WriteBits24((uint)value);
                    }
                    else if (value < (long)Bits.Bit32)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits32);
                        WriteBits32((uint)value);
                    }
                    else if (value < (long)Bits.Bit40)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits40);
                        WriteBits40((ulong)value);
                    }
                    else if (value < (long)Bits.Bit48)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits48);
                        WriteBits48((ulong)value);
                    }
                    else if (value < (long)Bits.Bit56)
                    {
                        WriteSymbol(CtpObjectSymbols.IntNegBits56);
                        WriteBits56((ulong)value);
                    }
                    else
                    {
                        WriteSymbol(CtpObjectSymbols.IntElse);
                        WriteBits64(~(ulong)value);
                    }
                }
                else
                {
                    if (value < (long)Bits.Bit08)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits8);
                        WriteBits8((uint)value);
                    }
                    else if (value < (long)Bits.Bit16)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits16);
                        WriteBits16((uint)value);
                    }
                    else if (value < (long)Bits.Bit24)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits24);
                        WriteBits24((uint)value);
                    }
                    else if (value < (long)Bits.Bit32)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits32);
                        WriteBits32((uint)value);
                    }
                    else if (value < (long)Bits.Bit40)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits40);
                        WriteBits40((ulong)value);
                    }
                    else if (value < (long)Bits.Bit48)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits48);
                        WriteBits48((ulong)value);
                    }
                    else if (value < (long)Bits.Bit56)
                    {
                        WriteSymbol(CtpObjectSymbols.IntBits56);
                        WriteBits56((ulong)value);
                    }
                    else
                    {
                        WriteSymbol(CtpObjectSymbols.IntElse);
                        WriteBits64((ulong)value);
                    }
                }
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
            m_byteBuffer[m_byteLength + 0] = (byte)value;
            m_byteLength += 1;
        }

        private void WriteBits16(uint value)
        {
            EnsureCapacityBytes(2);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 1] = (byte)value;
            m_byteLength += 2;
        }

        private void WriteBits24(uint value)
        {
            EnsureCapacityBytes(3);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 2] = (byte)value;
            m_byteLength += 3;
        }

        private void WriteBits32(uint value)
        {
            EnsureCapacityBytes(4);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 3] = (byte)value;
            m_byteLength += 4;
        }

        private void WriteBits40(ulong value)
        {
            EnsureCapacityBytes(5);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 4] = (byte)value;
            m_byteLength += 5;
        }

        private void WriteBits48(ulong value)
        {
            EnsureCapacityBytes(6);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 40);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 4] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 5] = (byte)value;
            m_byteLength += 6;
        }

        private void WriteBits56(ulong value)
        {
            EnsureCapacityBytes(7);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 48);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 40);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 4] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 5] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 6] = (byte)value;
            m_byteLength += 7;
        }

        private void WriteBits64(ulong value)
        {
            EnsureCapacityBytes(8);
            m_byteBuffer[m_byteLength + 0] = (byte)(value >> 56);
            m_byteBuffer[m_byteLength + 1] = (byte)(value >> 48);
            m_byteBuffer[m_byteLength + 2] = (byte)(value >> 40);
            m_byteBuffer[m_byteLength + 3] = (byte)(value >> 32);
            m_byteBuffer[m_byteLength + 4] = (byte)(value >> 24);
            m_byteBuffer[m_byteLength + 5] = (byte)(value >> 16);
            m_byteBuffer[m_byteLength + 6] = (byte)(value >> 8);
            m_byteBuffer[m_byteLength + 7] = (byte)value;
            m_byteLength += 8;
        }

    }
}

