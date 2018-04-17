using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for CTP.
    /// </summary>
    public partial class CtpObject
    {
        public void SetNull()
        {
            m_valueTypeCode = CtpTypeCode.Null;
        }
        public void SetValue(DBNull value)
        {
            SetNull();
        }
        public void SetValue(sbyte value)
        {
            SetValue((long)value);
        }
        public void SetValue(short value)
        {
            SetValue((long)value);
        }
        public void SetValue(int value)
        {
            SetValue((long)value);
        }
        public void SetValue(long value)
        {
            m_valueTypeCode = CtpTypeCode.Int64;
            m_valueInt64 = value;
        }
        public void SetValue(byte value)
        {
            SetValue((ulong)value);
        }

        public void SetValue(ushort value)
        {
            SetValue((ulong)value);
        }
        public void SetValue(uint value)
        {
            SetValue((ulong)value);
        }
        public void SetValue(ulong value)
        {
            if (value > long.MaxValue)
            {
                SetValue((double)value);
            }
            else
            {
                SetValue((long)value);
            }
        }
        public void SetValue(float value)
        {
            m_valueTypeCode = CtpTypeCode.Single;
            m_valueSingle = value;
        }
        public void SetValue(double value)
        {
            m_valueTypeCode = CtpTypeCode.Double;
            m_valueDouble = value;
        }
        public void SetValue(decimal value)
        {
            SetValue((double)value);
        }
        public void SetValue(DateTime value)
        {
            SetValue(new CtpTime(value));
        }
        public void SetValue(CtpTime value)
        {
            m_valueTypeCode = CtpTypeCode.CtpTime;
            m_valueCtpTime = value;
        }
        public void SetValue(bool value)
        {
            m_valueTypeCode = CtpTypeCode.Boolean;
            m_valueBoolean = value;
        }
        public void SetValue(Guid value)
        {
            m_valueTypeCode = CtpTypeCode.Guid;
            m_valueGuid = value;
        }
        public void SetValue(char value)
        {
            SetValue(value.ToString());
        }
        public void SetValue(string value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            m_valueTypeCode = CtpTypeCode.String;
            m_valueObject = value;
        }
        public void SetValue(CtpBuffer value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            m_valueTypeCode = CtpTypeCode.CtpBuffer;
            m_valueObject = value;
        }
        public void SetValue(CtpDocument value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            m_valueTypeCode = CtpTypeCode.CtpDocument;
            m_valueObject = value;
        }

        public void SetValue(byte[] value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            SetValue(new CtpBuffer(value));
        }

        public void SetValue(sbyte? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(short? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(int? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(long? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(byte? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(ushort? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(uint? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(ulong? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(float? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(double? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(decimal? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(DateTime? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(CtpTime? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(bool? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(Guid? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        public void SetValue(char? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }

        public void SetValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                SetNull();
                return;
            }

            var type = value.GetType();
            if (type == typeof(sbyte))
            {
                SetValue((sbyte)value);
            }
            else if (type == typeof(short))
            {
                SetValue((short)value);
            }
            else if (type == typeof(int))
            {
                SetValue((int)value);
            }
            else if (type == typeof(long))
            {
                SetValue((long)value);
            }
            else if (type == typeof(byte))
            {
                SetValue((byte)value);
            }
            else if (type == typeof(ushort))
            {
                SetValue((ushort)value);
            }
            else if (type == typeof(uint))
            {
                SetValue((uint)value);
            }
            else if (type == typeof(ulong))
            {
                SetValue((ulong)value);
            }
            else if (type == typeof(float))
            {
                SetValue((float)value);
            }
            else if (type == typeof(double))
            {
                SetValue((double)value);
            }
            else if (type == typeof(decimal))
            {
                SetValue((decimal)value);
            }
            else if (type == typeof(DateTime))
            {
                SetValue((DateTime)value);
            }
            else if (type == typeof(CtpTime))
            {
                SetValue((CtpTime)value);
            }
            else if (type == typeof(bool))
            {
                SetValue((bool)value);
            }
            else if (type == typeof(Guid))
            {
                SetValue((Guid)value);
            }
            else if (type == typeof(char))
            {
                SetValue((char)value);
            }
            else if (type == typeof(string))
            {
                SetValue((string)value);
            }
            else if (type == typeof(CtpBuffer))
            {
                SetValue((CtpBuffer)value);
            }
            else if (type == typeof(CtpDocument))
            {
                SetValue((CtpDocument)value);
            }
            else if (type == typeof(byte[]))
            {
                SetValue((byte[])value);
            }
            else if (value is CtpObject)
            {
                SetValue((CtpObject)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported CtpValue type: " + type.ToString());
            }
        }

        public void SetValue(CtpObject value)
        {
            m_raw0 = value.m_raw0;
            m_raw1 = value.m_raw1;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.m_valueTypeCode;
        }

    }
}
