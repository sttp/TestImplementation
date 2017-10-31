using System;
using System.Text;

namespace Sttp.WireProtocol
{
    public static class SttpValueTypeCodec
    {
        public static byte[] Encode(SttpValueTypeCode typeCode, object value)
        {
            //ToDo: Cast if it implements IConvertable.
            if (value == null)
                return null;
            switch (typeCode)
            {
                case SttpValueTypeCode.Null:
                    return null;
                case SttpValueTypeCode.SByte:
                    return new byte[] { (byte)(sbyte)value };
                case SttpValueTypeCode.Int16:
                    return BigEndian.GetBytes((short)value);
                case SttpValueTypeCode.Int32:
                    return BigEndian.GetBytes((int)value);
                case SttpValueTypeCode.Int64:
                    return BigEndian.GetBytes((long)value);
                case SttpValueTypeCode.Byte:
                    return new byte[] { (byte)value };
                case SttpValueTypeCode.UInt16:
                    return BigEndian.GetBytes((ushort)value);
                case SttpValueTypeCode.UInt32:
                    return BigEndian.GetBytes((uint)value);
                case SttpValueTypeCode.UInt64:
                    return BigEndian.GetBytes((ulong)value);
                case SttpValueTypeCode.Decimal:
                    return BigEndian.GetBytes((decimal)value);
                case SttpValueTypeCode.Double:
                    return BigEndian.GetBytes((double)value);
                case SttpValueTypeCode.Single:
                    return BigEndian.GetBytes((float)value);
                case SttpValueTypeCode.DateTime:
                    return BigEndian.GetBytes(((DateTime)value).Ticks);
                case SttpValueTypeCode.Bool:
                    return BigEndian.GetBytes((bool)value);
                case SttpValueTypeCode.Guid:
                    return ((Guid)value).ToRfcBytes();
                case SttpValueTypeCode.String:
                    return Encoding.UTF8.GetBytes((string)value);
                case SttpValueTypeCode.Buffer:
                    return (byte[])value;
                default:
                    throw new NotSupportedException();
            }
        }

        public static object Decode(SttpValueTypeCode typeCode, byte[] data)
        {
            if (data == null)
                return null;
            switch (typeCode)
            {
                case SttpValueTypeCode.Null:
                    return null;
                case SttpValueTypeCode.SByte:
                    return (sbyte)data[0];
                case SttpValueTypeCode.Int16:
                    return BigEndian.ToInt16(data, 0);
                case SttpValueTypeCode.Int32:
                    return BigEndian.ToInt32(data, 0);
                case SttpValueTypeCode.Int64:
                    return BigEndian.ToInt64(data, 0);
                case SttpValueTypeCode.Byte:
                    return data[0];
                case SttpValueTypeCode.UInt16:
                    return BigEndian.ToUInt16(data, 0);
                case SttpValueTypeCode.UInt32:
                    return BigEndian.ToUInt32(data, 0);
                case SttpValueTypeCode.UInt64:
                    return BigEndian.ToUInt64(data, 0);
                case SttpValueTypeCode.Decimal:
                    return BigEndian.ToDecimal(data, 0);
                case SttpValueTypeCode.Double:
                    return BigEndian.ToDouble(data, 0);
                case SttpValueTypeCode.Single:
                    return BigEndian.ToSingle(data, 0);
                case SttpValueTypeCode.DateTime:
                    return new DateTime(BigEndian.ToInt64(data, 0));
                case SttpValueTypeCode.Bool:
                    return BigEndian.ToBoolean(data, 0);
                case SttpValueTypeCode.Guid:
                    return data.ToRfcGuid();
                case SttpValueTypeCode.String:
                    return Encoding.UTF8.GetString(data);
                case SttpValueTypeCode.Buffer:
                    return data;
                default:
                    throw new NotSupportedException();
            }

        }

        public static SttpValueTypeCode FromType(Type columnDataType)
        {
            if (columnDataType == typeof(sbyte))
                return SttpValueTypeCode.SByte;
            if (columnDataType == typeof(Int16))
                return SttpValueTypeCode.Int16;
            if (columnDataType == typeof(Int32))
                return SttpValueTypeCode.Int32;
            if (columnDataType == typeof(Int64))
                return SttpValueTypeCode.Int64;
            if (columnDataType == typeof(Byte))
                return SttpValueTypeCode.Byte;
            if (columnDataType == typeof(UInt16))
                return SttpValueTypeCode.UInt16;
            if (columnDataType == typeof(UInt32))
                return SttpValueTypeCode.UInt32;
            if (columnDataType == typeof(UInt64))
                return SttpValueTypeCode.UInt64;
            if (columnDataType == typeof(Decimal))
                return SttpValueTypeCode.Decimal;
            if (columnDataType == typeof(Double))
                return SttpValueTypeCode.Double;
            if (columnDataType == typeof(Single))
                return SttpValueTypeCode.Single;
            if (columnDataType == typeof(DateTime))
                return SttpValueTypeCode.DateTime;
            if (columnDataType == typeof(bool))
                return SttpValueTypeCode.Bool;
            if (columnDataType == typeof(Guid))
                return SttpValueTypeCode.Guid;
            if (columnDataType == typeof(String))
                return SttpValueTypeCode.String;
            if (columnDataType == typeof(byte[]))
                return SttpValueTypeCode.Buffer;

            return SttpValueTypeCode.Null;
        }
    }
}