using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial class CtpObject
    {
        public static explicit operator CtpObject(DBNull value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(sbyte value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(short value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(int value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(long value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(byte value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(ushort value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(uint value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(ulong value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(float value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(double value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(decimal value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(DateTime value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(CtpTime value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(bool value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(Guid value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(char value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(string value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(CtpBuffer value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(CtpDocument value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(byte[] value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(sbyte? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(short? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(int? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(long? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(byte? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(ushort? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(uint? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(ulong? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(float? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(double? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(decimal? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(DateTime? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(CtpTime? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(bool? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(Guid? value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(char? value)
        {
            return new CtpObject(value);
        }

        public static explicit operator DBNull(CtpObject value)
        {
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
        public static explicit operator Char(CtpObject value)
        {
            return value.AsChar;
        }
        public static explicit operator string(CtpObject value)
        {
            return value.AsString;
        }
        public static explicit operator CtpBuffer(CtpObject value)
        {
            return value.AsCtpBuffer;
        }
        public static explicit operator CtpDocument(CtpObject value)
        {
            return value.AsCtpDocument;
        }
        public static explicit operator byte[](CtpObject value)
        {
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
