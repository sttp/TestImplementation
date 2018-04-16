using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public abstract partial class CtpValue : IEquatable<CtpValue>
    {
        public static explicit operator CtpValue(sbyte value)
        {
            return (CtpValue)(long)value;
        }
        public static explicit operator CtpValue(short value)
        {
            return (CtpValue)(long)value;
        }
        public static explicit operator CtpValue(int value)
        {
            return (CtpValue)(long)value;
        }
        public static explicit operator CtpValue(long value)
        {
            return new CtpInt64(value);
        }
        public static explicit operator CtpValue(byte value)
        {
            return (CtpValue)(ulong)value;
        }
        public static explicit operator CtpValue(ushort value)
        {
            return (CtpValue)(ulong)value;
        }
        public static explicit operator CtpValue(uint value)
        {
            return (CtpValue)(ulong)value;
        }
        public static explicit operator CtpValue(ulong value)
        {
            if (value > long.MaxValue)
            {
                throw new OverflowException("uint64 values greater than int64.MaxValue are not supported.");
            }
            return (CtpValue)(long)value;
        }
        public static explicit operator CtpValue(float value)
        {
            return new CtpSingle(value);
        }
        public static explicit operator CtpValue(double value)
        {
            return new CtpDouble(value);
        }
        public static explicit operator CtpValue(DateTime value)
        {
            return (CtpValue)new CtpTime(value);
        }
        public static explicit operator CtpValue(CtpTime value)
        {
            return new CtpValueTime(value);
        }
        public static explicit operator CtpValue(bool value)
        {
            if (value)
                return CtpBoolean.ValueTrue;
            return CtpBoolean.ValueFalse;
        }
        public static explicit operator CtpValue(Guid value)
        {
            return new CtpGuid(value);
        }
        public static explicit operator CtpValue(string value)
        {
            if (value == null)
                return CtpValue.Null;
            if (value.Length == 0)
                return CtpString.EmptyString;
            return new CtpString(value);
        }
        public static explicit operator CtpValue(CtpBuffer value)
        {
            return new CtpValueBuffer(value);
        }
        public static explicit operator CtpValue(byte[] value)
        {
            return new CtpValueBuffer(value);
        }
        public static explicit operator CtpValue(CtpMarkup value)
        {
            return new CtpValueMarkup(value);
        }

        public static explicit operator sbyte(CtpValue value)
        {
            return value.AsSByte;
        }
        public static explicit operator short(CtpValue value)
        {
            return value.AsInt16;
        }
        public static explicit operator int(CtpValue value)
        {
            return value.AsInt32;
        }
        public static explicit operator long(CtpValue value)
        {
            return value.AsInt64;
        }
        public static explicit operator byte(CtpValue value)
        {
            return value.AsByte;
        }
        public static explicit operator ushort(CtpValue value)
        {
            return value.AsUInt16;
        }
        public static explicit operator uint(CtpValue value)
        {
            return value.AsUInt32;
        }
        public static explicit operator ulong(CtpValue value)
        {
            return value.AsUInt64;
        }
        public static explicit operator float(CtpValue value)
        {
            return value.AsSingle;
        }
        public static explicit operator double(CtpValue value)
        {
            return value.AsDouble;
        }
        public static explicit operator DateTime(CtpValue value)
        {
            return value.AsDateTime;
        }
        public static explicit operator CtpTime(CtpValue value)
        {
            return value.AsCtpTime;
        }
        public static explicit operator bool(CtpValue value)
        {
            return value.AsBoolean;
        }
        public static explicit operator Guid(CtpValue value)
        {
            return value.AsGuid;
        }
        public static explicit operator string(CtpValue value)
        {
            return value.AsString;
        }
        public static explicit operator CtpBuffer(CtpValue value)
        {
            return value.AsSttpBuffer;
        }
        public static explicit operator CtpMarkup(CtpValue value)
        {
            return value.AsSttpMarkup;
        }

        public static explicit operator byte[] (CtpValue value)
        {
            return value.AsSttpBuffer.ToBuffer();
        }

        public static CtpValue FromObject(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return CtpValue.Null;
            }

            var type = value.GetType();
            if (type == typeof(sbyte))
            {
                return (CtpValue)(sbyte)value;
            }
            else if (type == typeof(short))
            {
                return (CtpValue)(short)value;
            }
            else if (type == typeof(int))
            {
                return (CtpValue)(int)value;
            }
            else if (type == typeof(long))
            {
                return (CtpValue)(long)value;
            }
            else if (type == typeof(byte))
            {
                return (CtpValue)(byte)value;
            }
            else if (type == typeof(ushort))
            {
                return (CtpValue)(ushort)value;
            }
            else if (type == typeof(uint))
            {
                return (CtpValue)(uint)value;
            }
            else if (type == typeof(ulong))
            {
                return (CtpValue)(ulong)value;
            }
            else if (type == typeof(float))
            {
                return (CtpValue)(float)value;
            }
            else if (type == typeof(double))
            {
                return (CtpValue)(double)value;
            }
            else if (type == typeof(DateTime))
            {
                return (CtpValue)(DateTime)value;
            }
            else if (type == typeof(CtpTime))
            {
                return (CtpValue)(CtpTime)value;
            }
            else if (type == typeof(bool))
            {
                return (CtpValue)(bool)value;
            }
            else if (type == typeof(Guid))
            {
                return (CtpValue)(Guid)value;
            }
            else if (type == typeof(string))
            {
                return (CtpValue)(string)value;
            }
            else if (type == typeof(CtpBuffer))
            {
                return (CtpValue)(CtpBuffer)value;
            }
            else if (type == typeof(byte[]))
            {
                return (CtpValue)(byte[])value;
            }
            else if (value is CtpValue)
            {
                return (CtpValue)value;
            }
            else if (type == typeof(CtpMarkup))
            {
                return (CtpValue)(CtpMarkup)value;
            }
            else if (type == typeof(decimal))
            {
                return (CtpValue)(double)(decimal)value;
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }


        #region [ Nullable Types ]

        public static explicit operator CtpValue(sbyte? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }
        public static explicit operator CtpValue(short? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }
        public static explicit operator CtpValue(int? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }
        public static explicit operator CtpValue(long? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return new CtpInt64(value.Value);
        }
        public static explicit operator CtpValue(byte? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }
        public static explicit operator CtpValue(ushort? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }
        public static explicit operator CtpValue(uint? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }
        public static explicit operator CtpValue(ulong? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(long)(value.Value);
        }
        public static explicit operator CtpValue(float? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return new CtpSingle(value.Value);
        }
        public static explicit operator CtpValue(double? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return new CtpDouble(value.Value);
        }
        public static explicit operator CtpValue(DateTime? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)new CtpTime(value.Value);
        }
        public static explicit operator CtpValue(CtpTime? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return new CtpValueTime(value.Value);
        }
        public static explicit operator CtpValue(bool? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return (CtpValue)(value.Value);
        }

        public static explicit operator CtpValue(Guid? value)
        {
            if (!value.HasValue)
                return CtpValue.Null;
            return new CtpGuid(value.Value);
        }

        public static explicit operator sbyte? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSByte;
        }
        public static explicit operator short? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt16;
        }
        public static explicit operator int? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt32;
        }
        public static explicit operator long? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt64;
        }
        public static explicit operator byte? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsByte;
        }
        public static explicit operator ushort? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt16;
        }
        public static explicit operator uint? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt32;
        }
        public static explicit operator ulong? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt64;
        }
        public static explicit operator float? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsSingle;
        }
        public static explicit operator double? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDouble;
        }
        public static explicit operator DateTime? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsDateTime;
        }
        public static explicit operator CtpTime? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsCtpTime;
        }
        public static explicit operator bool? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsBoolean;
        }
        public static explicit operator Guid? (CtpValue value)
        {
            if (value.IsNull)
                return null;
            return value.AsGuid;
        }

        #endregion

    }
}
