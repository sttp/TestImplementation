using System;

namespace CTP
{
    /// <summary>
    /// This class contains the fundamental value for CTP.
    /// </summary>
    public partial class CtpObject
    {
        private void SetNull()
        {
            m_valueTypeCode = CtpTypeCode.Null;
        }
        private void SetValue(DBNull value)
        {
            SetNull();
        }
        private void SetValue(sbyte value)
        {
            SetValue((long)value);
        }
        private void SetValue(short value)
        {
            SetValue((long)value);
        }
        private void SetValue(int value)
        {
            SetValue((long)value);
        }
        private void SetValue(long value)
        {
            m_valueTypeCode = CtpTypeCode.Int64;
            m_valueInt64 = value;
        }
        private void SetValue(byte value)
        {
            SetValue((ulong)value);
        }

        private void SetValue(ushort value)
        {
            SetValue((ulong)value);
        }
        private void SetValue(uint value)
        {
            SetValue((ulong)value);
        }
        private void SetValue(ulong value)
        {
            if (value > long.MaxValue)
            {
                SetValue(value.ToString());
            }
            else
            {
                SetValue((long)value);
            }
        }
        private void SetValue(float value)
        {
            m_valueTypeCode = CtpTypeCode.Single;
            m_valueSingle = value;
        }
        private void SetValue(double value)
        {
            m_valueTypeCode = CtpTypeCode.Double;
            m_valueDouble = value;
        }
        private void SetValue(decimal value)
        {
            var d = Decimal.Truncate(value);
            if (d == value)
            {
                //Value is an integer;
                if (long.MinValue <= d && d <= long.MaxValue)
                {
                    SetValue((long)d);
                }
                //ToDo: Consider if very large integers should be string represented.
            }
            SetValue((double)value);
        }
        private void SetValue(DateTime value)
        {
            SetValue(new CtpTime(value));
        }
        private void SetValue(CtpTime value)
        {
            m_valueTypeCode = CtpTypeCode.CtpTime;
            m_valueCtpTime = value;
        }
        private void SetValue(bool value)
        {
            m_valueTypeCode = CtpTypeCode.Boolean;
            m_valueBoolean = value;
        }
        private void SetValue(Guid value)
        {
            m_valueTypeCode = CtpTypeCode.Guid;
            m_valueGuid = value;
        }
        private void SetValue(char value)
        {
            SetValue(value.ToString());
        }
        private void SetValue(char[] value)
        {
            SetValue(value.ToString());
        }
        private void SetValue(string value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            m_valueTypeCode = CtpTypeCode.String;
            m_valueObject = value;
        }
        private void SetValue(CtpBuffer value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            m_valueTypeCode = CtpTypeCode.CtpBuffer;
            m_valueObject = value;
        }
        private void SetValue(CtpCommand value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            m_valueTypeCode = CtpTypeCode.CtpCommand;
            m_valueObject = value;
        }

        private void SetValue(byte[] value)
        {
            if (value == null)
            {
                SetNull();
                return;
            }
            SetValue(new CtpBuffer(value));
        }

        private void SetValue(sbyte? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(short? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(int? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(long? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(byte? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(ushort? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(uint? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(ulong? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(float? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(double? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(decimal? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(DateTime? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(CtpTime? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(bool? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(Guid? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }
        private void SetValue(char? value)
        {
            if (!value.HasValue)
                SetNull();
            else
                SetValue(value.Value);
        }

        private void SetValue(object value)
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
            else if (type == typeof(char[]))
            {
                SetValue((char[])value);
            }
            else if (type == typeof(string))
            {
                SetValue((string)value);
            }
            else if (type == typeof(CtpBuffer))
            {
                SetValue((CtpBuffer)value);
            }
            else if (type == typeof(CtpCommand))
            {
                SetValue((CtpCommand)value);
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

        private void SetValue(CtpObject value)
        {
            m_raw0 = value.m_raw0;
            m_raw1 = value.m_raw1;
            m_valueObject = value.m_valueObject;
            m_valueTypeCode = value.m_valueTypeCode;
        }

    }
}
