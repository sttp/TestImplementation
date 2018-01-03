using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public abstract partial class SttpValue : IEquatable<SttpValue>
    {
        public static explicit operator SttpValue(sbyte value)
        {
            return (SttpValue)(long)value;
        }
        public static explicit operator SttpValue(short value)
        {
            return (SttpValue)(long)value;
        }
        public static explicit operator SttpValue(int value)
        {
            return (SttpValue)(long)value;
        }
        public static explicit operator SttpValue(long value)
        {
            return new SttpValueInt64(value);
        }
        public static explicit operator SttpValue(byte value)
        {
            return (SttpValue)(ulong)value;
        }
        public static explicit operator SttpValue(ushort value)
        {
            return (SttpValue)(ulong)value;
        }
        public static explicit operator SttpValue(uint value)
        {
            return (SttpValue)(ulong)value;
        }
        public static explicit operator SttpValue(ulong value)
        {
            if (value > long.MaxValue)
            {
                throw new OverflowException("uint64 values greater than int64.MaxValue are not supported.");
            }
            return (SttpValue)(long)value;
        }
        public static explicit operator SttpValue(float value)
        {
            return new SttpValueSingle(value);
        }
        public static explicit operator SttpValue(double value)
        {
            return new SttpValueDouble(value);
        }
        public static explicit operator SttpValue(DateTime value)
        {
            return (SttpValue)new SttpTime(value);
        }
        public static explicit operator SttpValue(DateTimeOffset value)
        {
            return (SttpValue)new SttpTime(value);
        }
        public static explicit operator SttpValue(SttpTime value)
        {
            return new SttpValueSttpTime(value);
        }
        public static explicit operator SttpValue(bool value)
        {
            if (value)
                return SttpValueBoolean.ValueTrue;
            return SttpValueBoolean.ValueFalse;
        }
        public static explicit operator SttpValue(Guid value)
        {
            return new SttpValueGuid(value);
        }
        public static explicit operator SttpValue(string value)
        {
            if (value == null)
                return SttpValue.Null;
            if (value.Length == 0)
                return SttpValueString.EmptyString;
            return new SttpValueString(value);
        }
        public static explicit operator SttpValue(SttpBuffer value)
        {
            return new SttpValueSttpBuffer(value);
        }
        public static explicit operator SttpValue(byte[] value)
        {
            return new SttpValueSttpBuffer(value);
        }
        public static explicit operator SttpValue(SttpMarkup value)
        {
            return new SttpValueSttpMarkup(value);
        }
        public static explicit operator SttpValue(SttpBulkTransport value)
        {
            return new SttpValueSttpBulkTransport(value);
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
        public static explicit operator bool(SttpValue value)
        {
            return value.AsBoolean;
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
        public static explicit operator SttpMarkup(SttpValue value)
        {
            return value.AsSttpMarkup;
        }

        public static explicit operator byte[] (SttpValue value)
        {
            return value.AsSttpBuffer.ToBuffer();
        }

        public static explicit operator SttpBulkTransport(SttpValue value)
        {
            return value.AsSttpBulkTransport;
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
                return (SttpValue)(sbyte)value;
            }
            else if (type == typeof(short))
            {
                return (SttpValue)(short)value;
            }
            else if (type == typeof(int))
            {
                return (SttpValue)(int)value;
            }
            else if (type == typeof(long))
            {
                return (SttpValue)(long)value;
            }
            else if (type == typeof(byte))
            {
                return (SttpValue)(byte)value;
            }
            else if (type == typeof(ushort))
            {
                return (SttpValue)(ushort)value;
            }
            else if (type == typeof(uint))
            {
                return (SttpValue)(uint)value;
            }
            else if (type == typeof(ulong))
            {
                return (SttpValue)(ulong)value;
            }
            else if (type == typeof(float))
            {
                return (SttpValue)(float)value;
            }
            else if (type == typeof(double))
            {
                return (SttpValue)(double)value;
            }
            else if (type == typeof(DateTime))
            {
                return (SttpValue)(DateTime)value;
            }
            else if (type == typeof(DateTimeOffset))
            {
                return (SttpValue)(DateTimeOffset)value;
            }
            else if (type == typeof(SttpTime))
            {
                return (SttpValue)(SttpTime)value;
            }
            else if (type == typeof(bool))
            {
                return (SttpValue)(bool)value;
            }
            else if (type == typeof(Guid))
            {
                return (SttpValue)(Guid)value;
            }
            else if (type == typeof(string))
            {
                return (SttpValue)(string)value;
            }
            else if (type == typeof(SttpBuffer))
            {
                return (SttpValue)(SttpBuffer)value;
            }
            else if (type == typeof(byte[]))
            {
                return (SttpValue)(byte[])value;
            }
            else if (value is SttpValue)
            {
                return (SttpValue)value;
            }
            else if (type == typeof(SttpMarkup))
            {
                return (SttpValue)(SttpMarkup)value;
            }
            else if (type == typeof(SttpBulkTransport))
            {
                return (SttpValue)(SttpBulkTransport)value;
            }
            else if (type == typeof(decimal))
            {
                return (SttpValue)(double)(decimal)value;
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }


        #region [ Nullable Types ]

        public static explicit operator SttpValue(sbyte? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }
        public static explicit operator SttpValue(short? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }
        public static explicit operator SttpValue(int? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }
        public static explicit operator SttpValue(long? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueInt64(value.Value);
        }
        public static explicit operator SttpValue(byte? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }
        public static explicit operator SttpValue(ushort? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }
        public static explicit operator SttpValue(uint? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }
        public static explicit operator SttpValue(ulong? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(long)(value.Value);
        }
        public static explicit operator SttpValue(float? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueSingle(value.Value);
        }
        public static explicit operator SttpValue(double? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueDouble(value.Value);
        }
        public static explicit operator SttpValue(DateTime? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)new SttpTime(value.Value);
        }
        public static explicit operator SttpValue(DateTimeOffset? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)new SttpTime(value.Value);
        }
        public static explicit operator SttpValue(SttpTime? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueSttpTime(value.Value);
        }
        public static explicit operator SttpValue(bool? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return (SttpValue)(value.Value);
        }

        public static explicit operator SttpValue(Guid? value)
        {
            if (!value.HasValue)
                return SttpValue.Null;
            return new SttpValueGuid(value.Value);
        }

        public static explicit operator sbyte? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSByte;
        }
        public static explicit operator short? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt16;
        }
        public static explicit operator int? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt32;
        }
        public static explicit operator long? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt64;
        }
        public static explicit operator byte? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsByte;
        }
        public static explicit operator ushort? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt16;
        }
        public static explicit operator uint? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt32;
        }
        public static explicit operator ulong? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt64;
        }
        public static explicit operator float? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSingle;
        }
        public static explicit operator double? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDouble;
        }
        public static explicit operator DateTime? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDateTime;
        }
        public static explicit operator DateTimeOffset? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDateTimeOffset;
        }
        public static explicit operator SttpTime? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSttpTime;
        }
        public static explicit operator bool? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsBoolean;
        }
        public static explicit operator Guid? (SttpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsGuid;
        }

        #endregion

    }
}
