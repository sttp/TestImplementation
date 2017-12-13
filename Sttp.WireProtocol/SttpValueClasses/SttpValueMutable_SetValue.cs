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
            m_valueTypeCode = SttpValueTypeCode.Int64;
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
        public void SetValue(DateTime value)
        {
            SetValue(new SttpTime(value));
        }
        public void SetValue(DateTimeOffset value)
        {
            SetValue(new SttpTime(value));
        }
        public void SetValue(SttpTime value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpTime;
            m_valueSttpTime = value;
        }
        public void SetValue(bool value)
        {
            m_valueTypeCode = SttpValueTypeCode.Boolean;
            m_valueBoolean = value;
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
        public void SetValue(SttpBuffer value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpBuffer;
            m_valueObject = value;
        }
        public void SetValue(SttpMarkup value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpMarkup;
            m_valueObject = value;
        }
        public void SetValueBulkTransportGuid(Guid value)
        {
            m_valueTypeCode = SttpValueTypeCode.SttpBulkTransport;
            m_valueGuid = value;
        }

        public void SetValue(byte[] data)
        {
            SetValue(new SttpBuffer(data));
        }

        public void SetValue(SttpValue value)
        {
            switch (value.ValueTypeCode)
            {
                case SttpValueTypeCode.Null:
                    SetNull();
                    break;
                case SttpValueTypeCode.Int64:
                    SetValue(value.AsInt64);
                    break;
                case SttpValueTypeCode.Single:
                    SetValue(value.AsSingle);
                    break;
                case SttpValueTypeCode.Double:
                    SetValue(value.AsDouble);
                    break;
                case SttpValueTypeCode.Decimal:
                    SetValue(value.AsDecimal);
                    break;
                case SttpValueTypeCode.SttpTime:
                    SetValue(value.AsSttpTime);
                    break;
                case SttpValueTypeCode.Boolean:
                    SetValue(value.AsBoolean);
                    break;
                case SttpValueTypeCode.Guid:
                    SetValue(value.AsGuid);
                    break;
                case SttpValueTypeCode.String:
                    SetValue(value.AsString);
                    break;
                case SttpValueTypeCode.SttpBuffer:
                    SetValue(value.AsSttpBuffer);
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    SetValue(value.AsSttpMarkup);
                    break;
                case SttpValueTypeCode.SttpBulkTransport:
                    SetValue(value.AsSttpBulkTransport);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            else if (type == typeof(DateTime))
            {
                SetValue((DateTime)value);
            }
            else if (type == typeof(DateTimeOffset))
            {
                SetValue((DateTimeOffset)value);
            }
            else if (type == typeof(SttpTime))
            {
                SetValue((SttpTime)value);
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
            else if (type == typeof(SttpBuffer))
            {
                SetValue((SttpBuffer)value);
            }
            else if (type == typeof(byte[]))
            {
                SetValue((byte[])value);
            }
            else if (value is SttpValue)
            {
                SetValue((SttpValue)value);
            }
            else if (type == typeof(SttpMarkup))
            {
                SetValue((SttpMarkup)value);
            }
            else
            {
                throw new NotSupportedException("Type is not a supported SttpValue type: " + type.ToString());
            }
        }

        public new void Load(ByteReader rd)
        {
            SttpValueTypeCode value = (SttpValueTypeCode)rd.ReadByte();
            switch (value)
            {
                case SttpValueTypeCode.Null:
                    SetNull();
                    break;
                case SttpValueTypeCode.Int64:
                    SetValue(rd.ReadInt64());
                    break;
                case SttpValueTypeCode.Single:
                    SetValue(rd.ReadSingle());
                    break;
                case SttpValueTypeCode.Double:
                    SetValue(rd.ReadDouble());
                    break;
                case SttpValueTypeCode.Decimal:
                    SetValue(rd.ReadDecimal());
                    break;
                case SttpValueTypeCode.SttpTime:
                    SetValue(rd.ReadSttpTime());
                    break;
                case SttpValueTypeCode.Boolean:
                    SetValue(rd.ReadBoolean());
                    break;
                case SttpValueTypeCode.Guid:
                    SetValue(rd.ReadGuid());
                    break;
                case SttpValueTypeCode.String:
                    SetValue(rd.ReadString());
                    break;
                case SttpValueTypeCode.SttpBuffer:
                    SetValue(rd.ReadSttpBuffer());
                    break;
                case SttpValueTypeCode.SttpMarkup:
                    SetValue(rd.ReadSttpMarkup());
                    break;
                case SttpValueTypeCode.SttpBulkTransport:
                    SetValueBulkTransportGuid(rd.ReadGuid());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        public unsafe void LoadDelta(ByteReader rd, SttpValue other)
        {
            if (rd.ReadBits1() == 0)
            {
                var value = other.ValueTypeCode;
                switch (value)
                {
                    case SttpValueTypeCode.Null:
                        break;
                    case SttpValueTypeCode.Int64:
                        //wrBits.Write8BitSegments((ulong)AsInt64);
                        SetValue((long)(rd.Read8BitSegments() ^ (ulong)other.AsInt64));
                        break;
                    case SttpValueTypeCode.Single:
                        {
                            float value2 = other.AsSingle;
                            uint result = rd.ReadUInt32() ^ *(uint*)&value2;
                            SetValue(*(float*)&result);
                            break;
                        }
                    case SttpValueTypeCode.Double:
                        {
                            double value2 = other.AsDouble;
                            ulong result = rd.ReadUInt64() ^ *(uint*)&value2;
                            SetValue(*(double*)&result);
                            break;
                        }
                    case SttpValueTypeCode.Decimal:
                        SetValue(rd.ReadDecimal());
                        break;
                    case SttpValueTypeCode.SttpTime:
                        SetValue(rd.ReadSttpTime());
                        break;
                    case SttpValueTypeCode.Boolean:
                        SetValue(rd.ReadBits1() == 1);
                        break;
                    case SttpValueTypeCode.Guid:
                        SetValue(rd.ReadGuid());
                        break;
                    case SttpValueTypeCode.String:
                        SetValue(rd.ReadString());
                        break;
                    case SttpValueTypeCode.SttpBuffer:
                        SetValue(rd.ReadSttpBuffer());
                        break;
                    case SttpValueTypeCode.SttpMarkup:
                        SetValue(rd.ReadSttpMarkup());
                        break;
                    case SttpValueTypeCode.SttpBulkTransport:
                        SetValue(rd.ReadSttpBulkTransport());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                var value = (SttpValueTypeCode)rd.ReadBits4();
                switch (value)
                {
                    case SttpValueTypeCode.Null:
                        break;
                    case SttpValueTypeCode.Int64:
                        SetValue(rd.ReadInt64());
                        break;
                    case SttpValueTypeCode.Single:
                        SetValue(rd.ReadSingle());
                        break;
                    case SttpValueTypeCode.Double:
                        SetValue(rd.ReadDouble());
                        break;
                    case SttpValueTypeCode.Decimal:
                        SetValue(rd.ReadDecimal());
                        break;
                    case SttpValueTypeCode.SttpTime:
                        SetValue(rd.ReadSttpTime());
                        break;
                    case SttpValueTypeCode.Boolean:
                        SetValue(rd.ReadBits1() == 1);
                        break;
                    case SttpValueTypeCode.Guid:
                        SetValue(rd.ReadGuid());
                        break;
                    case SttpValueTypeCode.String:
                        SetValue(rd.ReadString());
                        break;
                    case SttpValueTypeCode.SttpBuffer:
                        SetValue(rd.ReadSttpBuffer());
                        break;
                    case SttpValueTypeCode.SttpMarkup:
                        SetValue(rd.ReadSttpMarkup());
                        break;
                    case SttpValueTypeCode.SttpBulkTransport:
                        SetValue(rd.ReadSttpBulkTransport());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


    }
}
