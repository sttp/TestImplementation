using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial struct CtpObject
    {
        public static implicit operator CtpObject(DBNull value)
        {
            return new CtpObject();
        }
        public static implicit operator CtpObject(sbyte value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(short value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(int value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(long value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(byte value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(ushort value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(uint value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(ulong value)
        {
            if (value > long.MaxValue)
                return new CtpObject(new CtpNumeric(value));
            return new CtpObject((long)value);
        }
        public static implicit operator CtpObject(float value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(double value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(decimal value)
        {
            return (CtpObject)(CtpNumeric)value;
        }

        public static implicit operator CtpObject(CtpNumeric value)
        {
            if (value.Scale == 0 && value.High == 0)
            {
                decimal v = (decimal)value;
                if (long.MinValue <= v && v <= long.MaxValue)
                    return new CtpObject((long)v);
            }
            return new CtpObject(value);
        }

        public static implicit operator CtpObject(DateTime value)
        {
            return new CtpObject((CtpTime)value);
        }
        public static implicit operator CtpObject(CtpTime value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(bool value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(Guid value)
        {
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(char value)
        {
            return new CtpObject(value.ToString());
        }
        public static implicit operator CtpObject(string value)
        {
            if (value == null)
                return CtpObject.Null;
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(char[] value)
        {
            if (value == null)
                return CtpObject.Null;
            return new CtpObject(value.ToString());
        }
        public static implicit operator CtpObject(CtpBuffer value)
        {
            if (value == null)
                return CtpObject.Null;
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(CtpCommand value)
        {
            if (value == null)
                return CtpObject.Null;
            return new CtpObject(value);
        }
        public static implicit operator CtpObject(byte[] value)
        {
            if (value == null)
                return CtpObject.Null;
            return new CtpObject((CtpBuffer)value);
        }
        public static implicit operator CtpObject(sbyte? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(short? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(int? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(long? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(byte? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(ushort? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(uint? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(ulong? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(float? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(double? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(CtpNumeric? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(decimal? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(DateTime? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(CtpTime? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(bool? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(Guid? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }
        public static implicit operator CtpObject(char? value)
        {
            if (!value.HasValue)
                return new CtpObject();
            return (CtpObject)value.Value;
        }

        public static implicit operator DBNull(CtpObject value)
        {
            if (value.IsNull)
                return DBNull.Value;
            return value.AsDBNull;
        }
        public static explicit operator sbyte(CtpObject value)
        {
            return value.AsSByte;
        }
        public static explicit operator short(CtpObject value)
        {
            return value.AsInt16;
        }
        public static explicit operator int(CtpObject value)
        {
            return value.AsInt32;
        }
        public static explicit operator long(CtpObject value)
        {
            return value.AsInt64;
        }
        public static explicit operator byte(CtpObject value)
        {
            return value.AsByte;
        }
        public static explicit operator ushort(CtpObject value)
        {
            return value.AsUInt16;
        }
        public static explicit operator uint(CtpObject value)
        {
            return value.AsUInt32;
        }
        public static explicit operator ulong(CtpObject value)
        {
            return value.AsUInt64;
        }
        public static explicit operator float(CtpObject value)
        {
            return value.AsSingle;
        }
        public static explicit operator double(CtpObject value)
        {
            return value.AsDouble;
        }
        public static explicit operator decimal(CtpObject value)
        {
            return value.AsDecimal;
        }
        public static explicit operator CtpNumeric(CtpObject value)
        {
            return value.AsNumeric;
        }
        public static explicit operator DateTime(CtpObject value)
        {
            return value.AsDateTime;
        }
        public static explicit operator CtpTime(CtpObject value)
        {
            return value.AsCtpTime;
        }
        public static explicit operator bool(CtpObject value)
        {
            return value.AsBoolean;
        }
        public static explicit operator Guid(CtpObject value)
        {
            return value.AsGuid;
        }
        public static explicit operator char(CtpObject value)
        {
            return value.AsChar;
        }
        public static explicit operator char[] (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsString.ToCharArray();
        }
        public static explicit operator string(CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsString;
        }
        public static explicit operator CtpBuffer(CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsCtpBuffer;
        }
        public static explicit operator CtpCommand(CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsCtpCommand;
        }
        public static explicit operator byte[] (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsCtpBuffer.ToBuffer();
        }

        public static explicit operator sbyte? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsSByte;
        }
        public static explicit operator short? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt16;
        }
        public static explicit operator int? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt32;
        }
        public static explicit operator long? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsInt64;
        }
        public static explicit operator byte? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsByte;
        }
        public static explicit operator ushort? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt16;
        }
        public static explicit operator uint? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt32;
        }
        public static explicit operator ulong? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsUInt64;
        }
        public static explicit operator float? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsSingle;
        }
        public static explicit operator double? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsDouble;
        }
        public static explicit operator CtpNumeric? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsNumeric;
        }
        public static explicit operator decimal? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsDecimal;
        }
        public static explicit operator DateTime? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsDateTime;
        }
        public static explicit operator CtpTime? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsCtpTime;
        }
        public static explicit operator bool? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsBoolean;
        }
        public static explicit operator Guid? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsGuid;
        }
        public static explicit operator char? (CtpObject value)
        {
            if (value.IsNull)
                return null;
            return value.AsChar;
        }

    }
}
