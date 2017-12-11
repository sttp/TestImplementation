using System;
using System.Text;

namespace Sttp
{
    public static class SttpValueTypeCodec
    {
        //public static SttpValue Encode(SttpValueTypeCode typeCode, object value)
        //{
        //    return (SttpValue)value;
        //}

        //public static object Decode(SttpValueTypeCode typeCode, SttpValue data)
        //{
        //    return data.ToNativeType(typeCode);
        //}

        public static SttpValueTypeCode FromType(Type columnDataType)
        {
            if (columnDataType == typeof(sbyte))
                return SttpValueTypeCode.Int64;
            if (columnDataType == typeof(Int16))
                return SttpValueTypeCode.Int64;
            if (columnDataType == typeof(Int32))
                return SttpValueTypeCode.Int64;
            if (columnDataType == typeof(Int64))
                return SttpValueTypeCode.Int64;
            if (columnDataType == typeof(Byte))
                return SttpValueTypeCode.UInt64;
            if (columnDataType == typeof(UInt16))
                return SttpValueTypeCode.UInt64;
            if (columnDataType == typeof(UInt32))
                return SttpValueTypeCode.UInt64;
            if (columnDataType == typeof(UInt64))
                return SttpValueTypeCode.UInt64;
            if (columnDataType == typeof(Decimal))
                return SttpValueTypeCode.Decimal;
            if (columnDataType == typeof(Double))
                return SttpValueTypeCode.Double;
            if (columnDataType == typeof(Single))
                return SttpValueTypeCode.Single;
            if (columnDataType == typeof(DateTime))
                return SttpValueTypeCode.SttpTime;
            if (columnDataType == typeof(bool))
                return SttpValueTypeCode.Boolean;
            if (columnDataType == typeof(Guid))
                return SttpValueTypeCode.Guid;
            if (columnDataType == typeof(String))
                return SttpValueTypeCode.String;
            if (columnDataType == typeof(byte[]))
                return SttpValueTypeCode.SttpBuffer;

            return SttpValueTypeCode.Null;
        }

        public static Type ToType(SttpValueTypeCode columnTypeCode)
        {
            switch (columnTypeCode)
            {
                case SttpValueTypeCode.Null:
                    return typeof(DBNull);
                case SttpValueTypeCode.Int64:
                    return typeof(Int64);
                case SttpValueTypeCode.UInt64:
                    return typeof(UInt64);
                case SttpValueTypeCode.Single:
                    return typeof(Single);
                case SttpValueTypeCode.Double:
                    return typeof(Double);
                case SttpValueTypeCode.Decimal:
                    return typeof(Decimal);
                case SttpValueTypeCode.SttpTime:
                    return typeof(SttpTime);
                case SttpValueTypeCode.Boolean:
                    return typeof(Boolean);
                case SttpValueTypeCode.Char:
                    return typeof(Char);
                case SttpValueTypeCode.Guid:
                    return typeof(Guid);
                case SttpValueTypeCode.String:
                    return typeof(String);
                case SttpValueTypeCode.SttpBuffer:
                    return typeof(byte[]);
                case SttpValueTypeCode.SttpValueSet:
                    return typeof(SttpValueSet);
                case SttpValueTypeCode.SttpNamedSet:
                    return typeof(SttpNamedSet);
                case SttpValueTypeCode.SttpMarkup:
                    return typeof(byte[]);
                case SttpValueTypeCode.BulkTransportGuid:
                    return typeof(Guid);
                default:
                    throw new ArgumentOutOfRangeException(nameof(columnTypeCode), columnTypeCode, null);
            }
        }
    }
}