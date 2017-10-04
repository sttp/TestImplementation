using System;
using System.Text;

namespace Sttp.WireProtocol
{
    public static class ValueTypeCodec
    {
        public static byte[] Encode(ValueType type, object value)
        {
            //ToDo: Cast if it implements IConvertable.
            if (value == null)
                return null;
            switch (type)
            {
                case ValueType.Null:
                    return null;
                case ValueType.SByte:
                    return new byte[] { (byte)(sbyte)value };
                case ValueType.Int16:
                    return BigEndian.GetBytes((short)value);
                case ValueType.Int32:
                    return BigEndian.GetBytes((int)value);
                case ValueType.Int64:
                    return BigEndian.GetBytes((long)value);
                case ValueType.Byte:
                    return new byte[] { (byte)value };
                case ValueType.UInt16:
                    return BigEndian.GetBytes((ushort)value);
                case ValueType.UInt32:
                    return BigEndian.GetBytes((uint)value);
                case ValueType.UInt64:
                    return BigEndian.GetBytes((ulong)value);
                case ValueType.Decimal:
                    return BigEndian.GetBytes((decimal)value);
                case ValueType.Double:
                    return BigEndian.GetBytes((double)value);
                case ValueType.Single:
                    return BigEndian.GetBytes((float)value);
                case ValueType.Ticks:
                    return BigEndian.GetBytes(((DateTime)value).Ticks);
                case ValueType.Bool:
                    return BigEndian.GetBytes((bool)value);
                case ValueType.Guid:
                    return ((Guid)value).ToRfcBytes();
                case ValueType.String:
                    return Encoding.UTF8.GetBytes((string)value);
                case ValueType.Buffer:
                    return (byte[])value;
                default:
                    throw new NotSupportedException();
            }
        }

        public static object Decode(ValueType type, byte[] data)
        {
            if (data == null)
                return null;
            switch (type)
            {
                case ValueType.Null:
                    return null;
                case ValueType.SByte:
                    return (sbyte)data[0];
                case ValueType.Int16:
                    return BigEndian.ToInt16(data, 0);
                case ValueType.Int32:
                    return BigEndian.ToInt32(data, 0);
                case ValueType.Int64:
                    return BigEndian.ToInt64(data, 0);
                case ValueType.Byte:
                    return data[0];
                case ValueType.UInt16:
                    return BigEndian.ToUInt16(data, 0);
                case ValueType.UInt32:
                    return BigEndian.ToUInt32(data, 0);
                case ValueType.UInt64:
                    return BigEndian.ToUInt64(data, 0);
                case ValueType.Decimal:
                    return BigEndian.ToDecimal(data, 0);
                case ValueType.Double:
                    return BigEndian.ToDouble(data, 0);
                case ValueType.Single:
                    return BigEndian.ToSingle(data, 0);
                case ValueType.Ticks:
                    return new DateTime(BigEndian.ToInt64(data, 0));
                case ValueType.Bool:
                    return BigEndian.ToBoolean(data, 0);
                case ValueType.Guid:
                    return data.ToRfcGuid();
                case ValueType.String:
                    return Encoding.UTF8.GetString(data);
                case ValueType.Buffer:
                    return data;
                default:
                    throw new NotSupportedException();
            }

        }
    }
}