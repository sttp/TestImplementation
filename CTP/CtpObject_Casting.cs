using System;

namespace CTP
{
    public partial struct CtpObject
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
                        return $"(CtpTime){m_valueCtpTime.ToString()}";
                    case CtpTypeCode.Boolean:
                        return $"(bool){m_valueBoolean.ToString()}";
                    case CtpTypeCode.Guid:
                        return $"(Guid){m_valueGuid.ToString()}";
                    case CtpTypeCode.String:
                        return $"(String){(m_valueObject as string).ToString()}";
                    case CtpTypeCode.CtpBuffer:
                        return $"(CtpBuffer){(m_valueObject as CtpBuffer).ToString()}";
                    case CtpTypeCode.CtpCommand:
                        return $"(CtpDocument){(m_valueObject as CtpCommand).ToString()}";
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
                    case CtpTypeCode.CtpCommand:
                        return m_valueObject as CtpCommand;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static CtpObject FromObject(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return CtpObject.Null;
            }

            var type = value.GetType();
            if (type == typeof(sbyte))
            {
                return (CtpObject)((sbyte)value);
            }
            else if (type == typeof(short))
            {
                return (CtpObject)((short)value);
            }
            else if (type == typeof(int))
            {
                return (CtpObject)((int)value);
            }
            else if (type == typeof(long))
            {
                return (CtpObject)((long)value);
            }
            else if (type == typeof(byte))
            {
                return (CtpObject)((byte)value);
            }
            else if (type == typeof(ushort))
            {
                return (CtpObject)((ushort)value);
            }
            else if (type == typeof(uint))
            {
                return (CtpObject)((uint)value);
            }
            else if (type == typeof(ulong))
            {
                return (CtpObject)((ulong)value);
            }
            else if (type == typeof(float))
            {
                return (CtpObject)((float)value);
            }
            else if (type == typeof(double))
            {
                return (CtpObject)((double)value);
            }
            else if (type == typeof(decimal))
            {
                return (CtpObject)((decimal)value);
            }
            else if (type == typeof(DateTime))
            {
                return (CtpObject)((DateTime)value);
            }
            else if (type == typeof(CtpTime))
            {
                return (CtpObject)((CtpTime)value);
            }
            else if (type == typeof(bool))
            {
                return (CtpObject)((bool)value);
            }
            else if (type == typeof(Guid))
            {
                return (CtpObject)((Guid)value);
            }
            else if (type == typeof(char))
            {
                return (CtpObject)((char)value);
            }
            else if (type == typeof(char[]))
            {
                return (CtpObject)((char[])value);
            }
            else if (type == typeof(string))
            {
                return (CtpObject)((string)value);
            }
            else if (type == typeof(CtpBuffer))
            {
                return (CtpObject)((CtpBuffer)value);
            }
            else if (type == typeof(CtpCommand))
            {
                return (CtpObject)((CtpCommand)value);
            }
            else if (type == typeof(byte[]))
            {
                return (CtpObject)((byte[])value);
            }
            else if (value is CtpObject)
            {
                return (CtpObject)((CtpObject)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported CtpValue type: " + type.ToString());
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

        public sbyte AsSByte
        {
            get
            {
                checked
                {
                    return (sbyte)AsInt64;
                }
            }
        }

        public short AsInt16
        {
            get
            {
                checked
                {
                    return (short)AsInt64;
                }
            }
        }

        public int AsInt32
        {
            get
            {
                checked
                {
                    return (int)AsInt64;
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
                    case CtpTypeCode.String:
                        {
                            checked
                            {
                                return (long)AsDecimal;
                            }
                        }
                    default:
                        throw new InvalidOperationException($"Cannot cast from {ToTypeString} to Int64");
                }
            }
        }

        public byte AsByte
        {
            get
            {
                checked
                {
                    return (byte)AsInt64;
                }
            }
        }

        public ushort AsUInt16
        {
            get
            {
                checked
                {
                    return (ushort)AsInt64;
                }
            }
        }

        public uint AsUInt32
        {
            get
            {
                checked
                {
                    return (uint)AsInt64;
                }
            }
        }

        public ulong AsUInt64
        {
            get
            {
                checked
                {
                    return (ulong)AsInt64;
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
                    case CtpTypeCode.String:
                    {
                        checked
                        {
                            return (float)AsDecimal;
                        }
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
                    case CtpTypeCode.String:
                    {
                        checked
                        {
                            return (double)AsDecimal;
                        }
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
                    case CtpTypeCode.String:
                        {
                            if (decimal.TryParse((string)m_valueObject, out var result))
                                return result;
                            throw new InvalidCastException($"Cannot cast from {ToTypeString} to Decimal");
                        }
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to Decimal");
                }
            }
        }

        public DateTime AsDateTime => AsCtpTime.AsDateTime;

        public CtpTime AsCtpTime
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.CtpTime:
                        return m_valueCtpTime;
                    case CtpTypeCode.String:
                    {
                        if (DateTime.TryParse((string)m_valueObject, out var result))
                            return new CtpTime(result);
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to CtpTime");
                    }
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

        public string AsString
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return null;
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
                    case CtpTypeCode.CtpCommand:
                        return (m_valueObject as CtpCommand).ToString();
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
                    case CtpTypeCode.Null:
                        return null;
                    case CtpTypeCode.CtpBuffer:
                        return m_valueObject as CtpBuffer;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to CtpBuffer");
                }
            }
        }

        public CtpCommand AsCtpCommand
        {
            get
            {
                switch (m_valueTypeCode)
                {
                    case CtpTypeCode.Null:
                        return null;
                    case CtpTypeCode.CtpCommand:
                        return m_valueObject as CtpCommand;
                    default:
                        throw new InvalidCastException($"Cannot cast from {ToTypeString} to CtpDocument");
                }
            }
        }




    }
}
