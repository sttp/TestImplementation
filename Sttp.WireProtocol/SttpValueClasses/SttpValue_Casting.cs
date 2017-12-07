using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;
using Sttp.SttpValueClasses;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public abstract partial class SttpValue : IEquatable<SttpValue>
    {
        protected static SttpValue CreateBulkTransportGuid(Guid guid)
        {
            return new SttpValueBulkTransportGuid(guid);
        }

        public static implicit operator SttpValue(sbyte value)
        {
            return new SttpValueSByte(value);
        }
        public static implicit operator SttpValue(short value)
        {
            return new SttpValueInt16(value);
        }
        public static implicit operator SttpValue(int value)
        {
            return new SttpValueInt32(value);
        }
        public static implicit operator SttpValue(long value)
        {
            return new SttpValueInt64(value);
        }
        public static implicit operator SttpValue(byte value)
        {
            return new SttpValueByte(value);
        }
        public static implicit operator SttpValue(ushort value)
        {
            return new SttpValueUInt16(value);
        }
        public static implicit operator SttpValue(uint value)
        {
            return new SttpValueUInt32(value);
        }
        public static implicit operator SttpValue(ulong value)
        {
            return new SttpValueUInt64(value);
        }
        public static implicit operator SttpValue(float value)
        {
            return new SttpValueSingle(value);
        }
        public static implicit operator SttpValue(double value)
        {
            return new SttpValueDouble(value);
        }
        public static implicit operator SttpValue(decimal value)
        {
            return new SttpValueDecimal(value);
        }
        public static implicit operator SttpValue(DateTime value)
        {
            return new SttpValueDateTime(value);
        }
        public static implicit operator SttpValue(DateTimeOffset value)
        {
            return new SttpValueDateTimeOffset(value);
        }
        public static implicit operator SttpValue(SttpTime value)
        {
            return new SttpValueSttpTime(value);
        }
        public static implicit operator SttpValue(SttpTimeOffset value)
        {
            return new SttpValueSttpTimeOffset(value);
        }
        public static implicit operator SttpValue(TimeSpan value)
        {
            return new SttpValueTimeSpan(value);
        }
        public static implicit operator SttpValue(bool value)
        {
            return new SttpValueBoolean(value);
        }
        public static implicit operator SttpValue(char value)
        {
            return new SttpValueChar(value);
        }
        public static implicit operator SttpValue(Guid value)
        {
            return new SttpValueGuid(value);
        }
        public static implicit operator SttpValue(string value)
        {
            return new SttpValueString(value);
        }
        public static implicit operator SttpValue(SttpBuffer value)
        {
            return new SttpValueSttpBuffer(value);
        }
        public static implicit operator SttpValue(SttpValueSet value)
        {
            return new SttpValueSttpValueSet(value);
        }
        public static implicit operator SttpValue(SttpNamedSet value)
        {
            return new SttpValueSttpNamedSet(value);
        }
        public static implicit operator SttpValue(SttpMarkup value)
        {
            return new SttpValueSttpMarkup(value);
        }

        public static explicit operator sbyte(SttpValue value)
        {
            return value.AsSByte;
        }
        public static explicit operator short(SttpValue value)
        {
            return value.AsInt16;
        }
        public static explicit operator int(SttpValue value)
        {
            return value.AsInt32;
        }
        public static explicit operator long(SttpValue value)
        {
            return value.AsInt64;
        }
        public static explicit operator byte(SttpValue value)
        {
            return value.AsByte;
        }
        public static explicit operator ushort(SttpValue value)
        {
            return value.AsUInt16;
        }
        public static explicit operator uint(SttpValue value)
        {
            return value.AsUInt32;
        }
        public static explicit operator ulong(SttpValue value)
        {
            return value.AsUInt64;
        }
        public static explicit operator float(SttpValue value)
        {
            return value.AsSingle;
        }
        public static explicit operator double(SttpValue value)
        {
            return value.AsDouble;
        }
        public static explicit operator decimal(SttpValue value)
        {
            return value.AsDecimal;
        }
        public static explicit operator DateTime(SttpValue value)
        {
            return value.AsDateTime;
        }
        public static explicit operator DateTimeOffset(SttpValue value)
        {
            return value.AsDateTimeOffset;
        }
        public static explicit operator SttpTime(SttpValue value)
        {
            return value.AsSttpTime;
        }
        public static explicit operator SttpTimeOffset(SttpValue value)
        {
            return value.AsSttpTimeOffset;
        }
        public static explicit operator TimeSpan(SttpValue value)
        {
            return value.AsTimeSpan;
        }
        public static explicit operator bool(SttpValue value)
        {
            return value.AsBoolean;
        }
        public static explicit operator char(SttpValue value)
        {
            return value.AsChar;
        }
        public static explicit operator Guid(SttpValue value)
        {
            return value.AsGuid;
        }
        public static explicit operator string(SttpValue value)
        {
            return value.AsString;
        }
        public static explicit operator SttpBuffer(SttpValue value)
        {
            return value.AsSttpBuffer;
        }
        public static explicit operator SttpValueSet(SttpValue value)
        {
            return value.AsSttpValueSet;
        }
        public static explicit operator SttpNamedSet(SttpValue value)
        {
            return value.AsSttpNamedSet;
        }
        public static explicit operator SttpMarkup(SttpValue value)
        {
            return value.AsSttpMarkup;
        }

    }
}
