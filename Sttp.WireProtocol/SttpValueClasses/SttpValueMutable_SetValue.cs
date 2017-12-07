using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sttp.Codec;
using Sttp.SttpValueClasses;

namespace Sttp
{
    /// <summary>
    /// This class contains the fundamental value for STTP.
    /// </summary>
    public partial class SttpValueMutable : SttpValue
    {
        public void SetNull()
        {
            m_valueTypeCode = SttpValueTypeCode.Null;
        }

        public void SetValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                m_valueTypeCode = SttpValueTypeCode.Null;
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
            else if (type == typeof(SttpTime))
            {
                SetValue((SttpTime)value);
            }
            else if (type == typeof(SttpTimeOffset))
            {
                SetValue((SttpTimeOffset)value);
            }
            else if (type == typeof(DateTime))
            {
                SetValue((DateTime)value);
            }
            else if (type == typeof(DateTimeOffset))
            {
                SetValue((DateTimeOffset)value);
            }
            else if (type == typeof(TimeSpan))
            {
                SetValue((TimeSpan)value);
            }
            else if (type == typeof(bool))
            {
                SetValue((bool)value);
            }
            else if (type == typeof(char))
            {
                SetValue((char)value);
            }
            else if (type == typeof(Guid))
            {
                SetValue((Guid)value);
            }
            else if (type == typeof(string))
            {
                SetValue((string)value);
            }
            else if (type == typeof(byte[]))
            {
                SetValue((byte[])value);
            }
            else if (type == typeof(SttpValueSet))
            {
                SetValue((SttpValueSet)value);
            }
            else if (type == typeof(SttpNamedSet))
            {
                SetValue((SttpNamedSet)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

        public void SetValue(sbyte value)
        {
            m_valueTypeCode = SttpValueTypeCode.SByte;
            m_valueSByte = value;
        }
        public void SetValue(short value)
        {
            m_valueTypeCode = SttpValueTypeCode.Int16;
            m_valueInt16 = value;
        }
        public void SetValue(int value)
        {
            m_valueTypeCode = SttpValueTypeCode.Int32;
            m_valueInt32 = value;
        }
        public void SetValue(long value)
        {
            m_valueTypeCode = SttpValueTypeCode.Int64;
            m_valueInt64 = value;
        }
        public void SetValue(byte value)
        {
            m_valueTypeCode = SttpValueTypeCode.Byte;
            m_valueByte = value;
        }
        public void SetValue(ushort value)
        {
            m_valueTypeCode = SttpValueTypeCode.UInt16;
            m_valueUInt16 = value;
        }
        public void SetValue(uint value)
        {
            m_valueTypeCode = SttpValueTypeCode.UInt32;
            m_valueUInt32 = value;
        }
        public void SetValue(ulong value)
        {
            m_valueTypeCode = SttpValueTypeCode.UInt64;
            m_valueUInt64 = value;
        }
        public void SetValue(float value)
        {
            m_valueTypeCode = SttpValueTypeCode.Single;
            m_valueSingle = value;
        }
        public void SetValue(double value)
        {
            m_valueTypeCode = SttpValueTypeCode.Double;
            m_valueDouble = value;
        }
        public void SetValue(decimal value)
        {
            m_valueTypeCode = SttpValueTypeCode.Decimal;
            m_valueDecimal = value;
        }
        public void SetValue(SttpTime value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpTime;
            m_valueSttpTime = value;
        }
        public void SetValue(SttpTimeOffset value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpTimeOffset;
            m_valueSttpTimeOffset = value;
        }
        public void SetValue(DateTime value)
        {
            m_valueTypeCode = SttpValueTypeCode.DateTime;
            m_valueDateTime = value;
        }
        public void SetValue(DateTimeOffset value)
        {
            m_valueTypeCode = SttpValueTypeCode.DateTimeOffset;
            m_valueDateTimeOffset = value;
        }
        public void SetValue(TimeSpan value)
        {
            m_valueTypeCode = SttpValueTypeCode.TimeSpan;
            m_valueTimeSpan = value;
        }
        public void SetValue(bool value)
        {
            m_valueTypeCode = SttpValueTypeCode.Boolean;
            m_valueBoolean = value;
        }
        public void SetValue(char value)
        {
            m_valueTypeCode = SttpValueTypeCode.Char;
            m_valueChar = value;
        }
        public void SetValue(Guid value)
        {
            m_valueTypeCode = SttpValueTypeCode.Guid;
            m_valueGuid = value;
        }
        public void SetValue(string value)
        {
            m_valueTypeCode = SttpValueTypeCode.String;
            m_valueObject = value;
        }
        public void SetValue(byte[] value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpBuffer;
            m_valueObject = value;
        }
        public void SetValue(SttpValueSet value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpValueSet;
            m_valueObject = value;
        }
        public void SetValue(SttpNamedSet value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpNamedSet;
            m_valueObject = value;
        }

        
    }
}
