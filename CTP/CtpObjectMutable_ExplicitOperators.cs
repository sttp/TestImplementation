using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial class CtpObjectMutable
    {
        public static explicit operator CtpObject(CtpObjectMutable value)
        {
            if ((object)value == null)
                return CtpObject.Null;

            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    return CtpObject.Null;
                case CtpTypeCode.Int64:
                    return new CtpObject(value.IsInt64);
                case CtpTypeCode.Single:
                    return new CtpObject(value.IsSingle);
                case CtpTypeCode.Double:
                    return new CtpObject(value.IsDouble);
                case CtpTypeCode.CtpTime:
                    return new CtpObject(value.IsCtpTime);
                case CtpTypeCode.Boolean:
                    return new CtpObject(value.IsBoolean);
                case CtpTypeCode.Guid:
                    return new CtpObject(value.IsGuid);
                case CtpTypeCode.String:
                    return new CtpObject(value.IsString);
                case CtpTypeCode.CtpBuffer:
                    return new CtpObject(value.IsCtpBuffer);
                case CtpTypeCode.CtpCommand:
                    return new CtpObject(value.IsCtpCommand);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static explicit operator CtpObjectMutable(DBNull value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(sbyte value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(short value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(int value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(long value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(byte value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(ushort value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(uint value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(ulong value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(float value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(double value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(decimal value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(DateTime value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(CtpTime value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(bool value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(Guid value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(char value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(string value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(char[] value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(CtpBuffer value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(CtpCommand value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(byte[] value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(sbyte? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(short? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(int? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(long? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(byte? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(ushort? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(uint? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(ulong? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(float? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(double? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(decimal? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(DateTime? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(CtpTime? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(bool? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(Guid? value)
        {
            return new CtpObjectMutable(value);
        }
        public static explicit operator CtpObjectMutable(char? value)
        {
            return new CtpObjectMutable(value);
        }

        public static explicit operator DBNull(CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return DBNull.Value;
            return value.AsDBNull;
        }
        public static explicit operator sbyte(CtpObjectMutable value)
        {
            return value.AsSByte;
        }
        public static explicit operator short(CtpObjectMutable value)
        {
            return value.AsInt16;
        }
        public static explicit operator int(CtpObjectMutable value)
        {
            return value.AsInt32;
        }
        public static explicit operator long(CtpObjectMutable value)
        {
            return value.AsInt64;
        }
        public static explicit operator byte(CtpObjectMutable value)
        {
            return value.AsByte;
        }
        public static explicit operator ushort(CtpObjectMutable value)
        {
            return value.AsUInt16;
        }
        public static explicit operator uint(CtpObjectMutable value)
        {
            return value.AsUInt32;
        }
        public static explicit operator ulong(CtpObjectMutable value)
        {
            return value.AsUInt64;
        }
        public static explicit operator float(CtpObjectMutable value)
        {
            return value.AsSingle;
        }
        public static explicit operator double(CtpObjectMutable value)
        {
            return value.AsDouble;
        }
        public static explicit operator decimal(CtpObjectMutable value)
        {
            return value.AsDecimal;
        }
        public static explicit operator DateTime(CtpObjectMutable value)
        {
            return value.AsDateTime;
        }
        public static explicit operator CtpTime(CtpObjectMutable value)
        {
            return value.AsCtpTime;
        }
        public static explicit operator bool(CtpObjectMutable value)
        {
            return value.AsBoolean;
        }
        public static explicit operator Guid(CtpObjectMutable value)
        {
            return value.AsGuid;
        }
        public static explicit operator char(CtpObjectMutable value)
        {
            return value.AsChar;
        }
        public static explicit operator char[] (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsString.ToCharArray();
        }
        public static explicit operator string(CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsString;
        }
        public static explicit operator CtpBuffer(CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsCtpBuffer;
        }
        public static explicit operator CtpCommand(CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsCtpCommand;
        }
        public static explicit operator byte[] (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsCtpBuffer.ToBuffer();
        }

        public static explicit operator sbyte? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsSByte;
        }
        public static explicit operator short? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsInt16;
        }
        public static explicit operator int? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsInt32;
        }
        public static explicit operator long? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsInt64;
        }
        public static explicit operator byte? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsByte;
        }
        public static explicit operator ushort? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsUInt16;
        }
        public static explicit operator uint? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsUInt32;
        }
        public static explicit operator ulong? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsUInt64;
        }
        public static explicit operator float? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsSingle;
        }
        public static explicit operator double? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsDouble;
        }
        public static explicit operator decimal? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsDecimal;
        }
        public static explicit operator DateTime? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsDateTime;
        }
        public static explicit operator CtpTime? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsCtpTime;
        }
        public static explicit operator bool? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsBoolean;
        }
        public static explicit operator Guid? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsGuid;
        }
        public static explicit operator char? (CtpObjectMutable value)
        {
            if ((object)value == null || value.IsNull)
                return null;
            return value.AsChar;
        }

    }
}
