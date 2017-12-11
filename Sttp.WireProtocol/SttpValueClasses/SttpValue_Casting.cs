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
        public static implicit operator SttpValue(sbyte value)
        {
            return (SttpValue)(int)value;
        }
        public static implicit operator SttpValue(short value)
        {
            return (SttpValue)(int)value;
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
            return (SttpValue)(uint)value;
        }
        public static implicit operator SttpValue(ushort value)
        {
            return (SttpValue)(uint)value;
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
            return new SttpTime(value);
        }
        public static implicit operator SttpValue(DateTimeOffset value)
        {
            return new SttpTime(value);
        }
        public static implicit operator SttpValue(SttpTime value)
        {
            return new SttpValueSttpTime(value);
        }
        public static implicit operator SttpValue(TimeSpan value)
        {
            return new SttpTime(value);
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
        public static implicit operator SttpValue(byte[] value)
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

        public static explicit operator byte[] (SttpValue value)
        {
            return value.AsSttpBuffer.ToBuffer();
        }

        public static SttpValue FromObject(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return SttpValue.Null;
            }

            var type = value.GetType();
            if (type == typeof(sbyte))
            {
                return (sbyte)value;
            }
            else if (type == typeof(short))
            {
                return (short)value;
            }
            else if (type == typeof(int))
            {
                return (int)value;
            }
            else if (type == typeof(long))
            {
                return (long)value;
            }
            else if (type == typeof(byte))
            {
                return (byte)value;
            }
            else if (type == typeof(ushort))
            {
                return (ushort)value;
            }
            else if (type == typeof(uint))
            {
                return (uint)value;
            }
            else if (type == typeof(ulong))
            {
                return (ulong)value;
            }
            else if (type == typeof(float))
            {
                return (float)value;
            }
            else if (type == typeof(double))
            {
                return (double)value;
            }
            else if (type == typeof(decimal))
            {
                return (decimal)value;
            }
            else if (type == typeof(DateTime))
            {
                return (DateTime)value;
            }
            else if (type == typeof(DateTimeOffset))
            {
                return (DateTimeOffset)value;
            }
            else if (type == typeof(SttpTime))
            {
                return (SttpTime)value;
            }
            else if (type == typeof(TimeSpan))
            {
                return (TimeSpan)value;
            }
            else if (type == typeof(bool))
            {
                return (bool)value;
            }
            else if (type == typeof(char))
            {
                return (char)value;
            }
            else if (type == typeof(Guid))
            {
                return (Guid)value;
            }
            else if (type == typeof(string))
            {
                return (string)value;
            }
            else if (type == typeof(SttpBuffer))
            {
                return (SttpBuffer)value;
            }
            else if (type == typeof(byte[]))
            {
                return (byte[])value;
            }
            else if (value is SttpValue)
            {
                return (SttpValue)value;
            }
            else if (type == typeof(SttpValueSet))
            {
                return (SttpValueSet)value;
            }
            else if (type == typeof(SttpNamedSet))
            {
                return (SttpNamedSet)value;
            }
            else if (type == typeof(SttpMarkup))
            {
                return (SttpMarkup)value;
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }





        #region [ Nullable Types ]

        public static implicit operator SttpValue(sbyte? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (int)(value.Value);
        }
        public static implicit operator SttpValue(short? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (int)(value.Value);
        }
        public static implicit operator SttpValue(int? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueInt32(value.Value);
        }
        public static implicit operator SttpValue(long? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueInt64(value.Value);
        }
        public static implicit operator SttpValue(byte? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (uint)(value.Value);
        }
        public static implicit operator SttpValue(ushort? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (uint)(value.Value);
        }
        public static implicit operator SttpValue(uint? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueUInt32(value.Value);
        }
        public static implicit operator SttpValue(ulong? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueUInt64(value.Value);
        }
        public static implicit operator SttpValue(float? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueSingle(value.Value);
        }
        public static implicit operator SttpValue(double? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueDouble(value.Value);
        }
        public static implicit operator SttpValue(decimal? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueDecimal(value.Value);
        }
        public static implicit operator SttpValue(DateTime? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpTime(value.Value);
        }
        public static implicit operator SttpValue(DateTimeOffset? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpTime(value.Value);
        }
        public static implicit operator SttpValue(SttpTime? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueSttpTime(value.Value);
        }
        public static implicit operator SttpValue(TimeSpan? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpTime(value.Value);
        }
        public static implicit operator SttpValue(bool? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueBoolean(value.Value);
        }
        public static implicit operator SttpValue(char? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueChar(value.Value);
        }
        public static implicit operator SttpValue(Guid? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueGuid(value.Value);
        }
       
        public static explicit operator sbyte?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSByte;
        }
        public static explicit operator short?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt16;
        }
        public static explicit operator int?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt32;
        }
        public static explicit operator long?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt64;
        }
        public static explicit operator byte?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsByte;
        }
        public static explicit operator ushort?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt16;
        }
        public static explicit operator uint?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt32;
        }
        public static explicit operator ulong?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt64;
        }
        public static explicit operator float?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSingle;
        }
        public static explicit operator double?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDouble;
        }
        public static explicit operator decimal?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDecimal;
        }
        public static explicit operator DateTime?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDateTime;
        }
        public static explicit operator DateTimeOffset?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDateTimeOffset;
        }
        public static explicit operator SttpTime?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSttpTime;
        }
        public static explicit operator TimeSpan?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsTimeSpan;
        }
        public static explicit operator bool?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsBoolean;
        }
        public static explicit operator char?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsChar;
        }
        public static explicit operator Guid?(SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsGuid;
        }
       
        #endregion
        
    }
}
