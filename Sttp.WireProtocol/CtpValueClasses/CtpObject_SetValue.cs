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
    public partial class CtpObject
    {
        public void SetNull()
        {
            m_valueTypeCode = CtpTypeCode.Null;
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
                SetValue((decimal)value);
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
        public void SetValue(string value)
        {
            m_valueTypeCode = CtpTypeCode.String;
            m_valueObject = value;
        }
        public void SetValue(CtpBuffer value)
        {
            m_valueTypeCode = CtpTypeCode.CtpBuffer;
            m_valueObject = value;
        }
        public void SetValue(CtpDocument value)
        {
            m_valueTypeCode = CtpTypeCode.CtpDocument;
            m_valueObject = value;
        }

        public void SetValue(byte[] data)
        {
            SetValue(new CtpBuffer(data));
        }

        public void SetValue(CtpObject value)
        {
            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    SetNull();
                    break;
                case CtpTypeCode.Int64:
                    SetValue(value.AsInt64);
                    break;
                case CtpTypeCode.Single:
                    SetValue(value.AsSingle);
                    break;
                case CtpTypeCode.Double:
                    SetValue(value.AsDouble);
                    break;
                case CtpTypeCode.CtpTime:
                    SetValue(value.AsCtpTime);
                    break;
                case CtpTypeCode.Boolean:
                    SetValue(value.AsBoolean);
                    break;
                case CtpTypeCode.Guid:
                    SetValue(value.AsGuid);
                    break;
                case CtpTypeCode.String:
                    SetValue(value.AsString);
                    break;
                case CtpTypeCode.CtpBuffer:
                    SetValue(value.AsCtpBuffer);
                    break;
                case CtpTypeCode.CtpDocument:
                    SetValue(value.AsDocument);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                m_valueTypeCode = CtpTypeCode.Null;
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
                SetValue((double)(decimal)value);
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
            else if (type == typeof(CtpBuffer))
            {
                SetValue((CtpBuffer)value);
            }
            else if (type == typeof(byte[]))
            {
                SetValue((byte[])value);
            }
            else if (value is CtpObject)
            {
                SetValue((CtpObject)value);
            }
            else if (type == typeof(CtpDocument))
            {
                SetValue((CtpDocument)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }
    }
}
