using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial class CtpObject
    {
        public string ToTypeString
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return $"(Null)";
                    case CtpTypeCode.Int64:
                        return $"(long){m_valueInt64.ToString()}";
                    case CtpTypeCode.Single:
                        return $"(float){m_valueSingle.ToString()}";
                    case CtpTypeCode.Double:
                        return $"(double){m_valueDouble.ToString()}";
                    case CtpTypeCode.CtpTime:
                        return $"(SttpTime){m_valueCtpTime.ToString()}";
                    case CtpTypeCode.Boolean:
                        return $"(bool){m_valueBoolean.ToString()}";
                    case CtpTypeCode.Guid:
                        return $"(Guid){m_valueGuid.ToString()}";
                    case CtpTypeCode.String:
                        return $"(String){(m_valueObject as string).ToString()}";
                    case CtpTypeCode.CtpBuffer:
                        return $"(SttpBuffer){(m_valueObject as CtpBuffer).ToString()}";
                    case CtpTypeCode.CtpDocument:
                        return $"(CtpDocument){(m_valueObject as CtpDocument).ToString()}";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public object ToNativeType
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return null;
                    case CtpTypeCode.Int64:
                        return m_valueInt64;
                    case CtpTypeCode.Single:
                        return m_valueSingle;
                    case CtpTypeCode.Double:
                        return m_valueDouble;
                    case CtpTypeCode.CtpTime:
                        return m_valueCtpTime;
                    case CtpTypeCode.Boolean:
                        return m_valueBoolean;
                    case CtpTypeCode.Guid:
                        return m_valueGuid;
                    case CtpTypeCode.String:
                        return m_valueObject as string;
                    case CtpTypeCode.CtpBuffer:
                        return m_valueObject as CtpBuffer;
                    case CtpTypeCode.CtpDocument:
                        return m_valueObject as CtpDocument;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public long AsInt64
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        {
                            checked
                            {
                                return (long)m_valueInt64;
                            }
                        }
                    case CtpTypeCode.Single:
                        {
                            checked
                            {
                                return (long)m_valueSingle;
                            }
                        }
                    case CtpTypeCode.Double:
                        {
                            checked
                            {
                                return (long)m_valueDouble;
                            }
                        }
                    default:
                        throw new InvalidOperationException($"Cannot cast from {ToTypeString} to Int64");
                }
            }

        }

        public float AsSingle
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        checked
                        {
                            return (float)m_valueInt64;
                        }
                    case CtpTypeCode.Single:
                        checked
                        {
                            return (float)m_valueSingle;
                        }
                    case CtpTypeCode.Double:
                        checked
                        {
                            return (float)m_valueDouble;
                        }
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to Single");
                }
            }

        }

        public double AsDouble
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        checked
                        {
                            return (double)m_valueInt64;
                        }
                    case CtpTypeCode.Single:
                        checked
                        {
                            return (double)m_valueSingle;
                        }
                    case CtpTypeCode.Double:
                        checked
                        {
                            return (double)m_valueDouble;
                        }
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to Double");
                }
            }

        }

        public decimal AsDecimal
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        checked
                        {
                            return (decimal)m_valueInt64;
                        }
                    case CtpTypeCode.Single:
                        checked
                        {
                            return (decimal)m_valueSingle;
                        }
                    case CtpTypeCode.Double:
                        checked
                        {
                            return (decimal)m_valueDouble;
                        }
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to Decimal");
                }
            }
        }

        public DBNull AsDBNull
        {
            get
            {
                if (IsNull)
                    return DBNull.Value;
                throw new InvalidCastException($"Cannot cast from {ToTypeString} to DBNull");
            }
        }

        public CtpTime AsCtpTime
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.CtpTime:
                        return m_valueCtpTime;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to CtpTime");
                }
            }

        }

        public bool AsBoolean
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Boolean:
                        return m_valueBoolean;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to Boolean");
                }
            }
        }

        public char AsChar
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Boolean:
                        if (m_valueBoolean)
                        {
                            return 'T';
                        }
                        return 'F';
                    case CtpTypeCode.String:
                        var s = (string)m_valueObject;
                        if (s.Length != 1)
                        {
                            throw new InvalidCastException($"Cannot cast from {ToTypeString} to char");
                        }
                        return s[0];
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to char");
                }
            }
        }

        public Guid AsGuid
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Guid:
                        return m_valueGuid;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to Guid");
                }
            }

        }

        public string AsString
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return "{null}";
                    case CtpTypeCode.Int64:
                        return m_valueInt64.ToString();
                    case CtpTypeCode.Single:
                        return m_valueSingle.ToString();
                    case CtpTypeCode.Double:
                        return m_valueDouble.ToString();
                    case CtpTypeCode.CtpTime:
                        return m_valueCtpTime.ToString();
                    case CtpTypeCode.Boolean:
                        return m_valueBoolean.ToString();
                    case CtpTypeCode.Guid:
                        return m_valueGuid.ToString();
                    case CtpTypeCode.String:
                        return (m_valueObject as string).ToString();
                    case CtpTypeCode.CtpBuffer:
                        return (m_valueObject as CtpBuffer).ToString();
                    case CtpTypeCode.CtpDocument:
                        return (m_valueObject as CtpDocument).ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        public CtpBuffer AsCtpBuffer
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.CtpBuffer:
                        return m_valueObject as CtpBuffer;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to CtpBuffer");
                }
            }
        }

        public CtpDocument AsDocument
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.CtpDocument:
                        return m_valueObject as CtpDocument;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to CtpDocument");
                }
            }
        }

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
            return value.AsDocument;
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

        public static CtpObject FromObject(object value)
        {
            return new CtpObject(value);
        }

        #region [ Nullable Types ]

       

       

        #endregion

    }
}
