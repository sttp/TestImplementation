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
                        return "";
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

        public static explicit operator CtpObject(sbyte value)
        {
            return (CtpObject)(long)value;
        }
        public static explicit operator CtpObject(short value)
        {
            return (CtpObject)(long)value;
        }
        public static explicit operator CtpObject(int value)
        {
            return (CtpObject)(long)value;
        }
        public static explicit operator CtpObject(long value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(byte value)
        {
            return (CtpObject)(ulong)value;
        }
        public static explicit operator CtpObject(ushort value)
        {
            return (CtpObject)(ulong)value;
        }
        public static explicit operator CtpObject(uint value)
        {
            return (CtpObject)(ulong)value;
        }
        public static explicit operator CtpObject(ulong value)
        {
            if (value > long.MaxValue)
            {
                throw new OverflowException("uint64 values greater than int64.MaxValue are not supported.");
            }
            return (CtpObject)(long)value;
        }
        public static explicit operator CtpObject(float value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(double value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(DateTime value)
        {
            return (CtpObject)new CtpTime(value);
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
        public static explicit operator CtpObject(string value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(CtpBuffer value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(byte[] value)
        {
            return new CtpObject(value);
        }
        public static explicit operator CtpObject(CtpDocument value)
        {
            return new CtpObject(value);
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

        public static explicit operator byte[] (CtpObject value)
        {
            return value.AsCtpBuffer.ToBuffer();
        }

        public static CtpObject FromObject(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return Null;
            }

            var type = value.GetType();
            if (type == typeof(sbyte))
            {
                return (CtpObject)(sbyte)value;
            }
            else if (type == typeof(short))
            {
                return (CtpObject)(short)value;
            }
            else if (type == typeof(int))
            {
                return (CtpObject)(int)value;
            }
            else if (type == typeof(long))
            {
                return (CtpObject)(long)value;
            }
            else if (type == typeof(byte))
            {
                return (CtpObject)(byte)value;
            }
            else if (type == typeof(ushort))
            {
                return (CtpObject)(ushort)value;
            }
            else if (type == typeof(uint))
            {
                return (CtpObject)(uint)value;
            }
            else if (type == typeof(ulong))
            {
                return (CtpObject)(ulong)value;
            }
            else if (type == typeof(float))
            {
                return (CtpObject)(float)value;
            }
            else if (type == typeof(double))
            {
                return (CtpObject)(double)value;
            }
            else if (type == typeof(DateTime))
            {
                return (CtpObject)(DateTime)value;
            }
            else if (type == typeof(CtpTime))
            {
                return (CtpObject)(CtpTime)value;
            }
            else if (type == typeof(bool))
            {
                return (CtpObject)(bool)value;
            }
            else if (type == typeof(Guid))
            {
                return (CtpObject)(Guid)value;
            }
            else if (type == typeof(string))
            {
                return (CtpObject)(string)value;
            }
            else if (type == typeof(CtpBuffer))
            {
                return (CtpObject)(CtpBuffer)value;
            }
            else if (type == typeof(byte[]))
            {
                return (CtpObject)(byte[])value;
            }
            else if (value is CtpObject)
            {
                return (CtpObject)value;
            }
            else if (type == typeof(CtpDocument))
            {
                return (CtpObject)(CtpDocument)value;
            }
            else if (type == typeof(decimal))
            {
                return (CtpObject)(double)(decimal)value;
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

        #region [ Nullable Types ]

        public static explicit operator CtpObject(sbyte? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }
        public static explicit operator CtpObject(short? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }
        public static explicit operator CtpObject(int? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }
        public static explicit operator CtpObject(long? value)
        {
            if (!value.HasValue)
                return Null;
            return new CtpObject(value.Value);
        }
        public static explicit operator CtpObject(byte? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }
        public static explicit operator CtpObject(ushort? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }
        public static explicit operator CtpObject(uint? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }
        public static explicit operator CtpObject(ulong? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(long)(value.Value);
        }
        public static explicit operator CtpObject(float? value)
        {
            if (!value.HasValue)
                return Null;
            return new CtpObject(value.Value);
        }
        public static explicit operator CtpObject(double? value)
        {
            if (!value.HasValue)
                return Null;
            return new CtpObject(value.Value);
        }
        public static explicit operator CtpObject(DateTime? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)new CtpTime(value.Value);
        }
        public static explicit operator CtpObject(CtpTime? value)
        {
            if (!value.HasValue)
                return Null;
            return new CtpObject(value.Value);
        }
        public static explicit operator CtpObject(bool? value)
        {
            if (!value.HasValue)
                return Null;
            return (CtpObject)(value.Value);
        }

        public static explicit operator CtpObject(Guid? value)
        {
            if (!value.HasValue)
                return Null;
            return new CtpObject(value.Value);
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

        #endregion

    }
}
