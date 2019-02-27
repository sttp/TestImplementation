using System;
using System.Text;
using CTP;

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

        public static CtpTypeCode FromType(Type columnDataType)
        {
            if (columnDataType == typeof(sbyte))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(Int16))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(Int32))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(Int64))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(Byte))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(UInt16))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(UInt32))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(UInt64))
                return CtpTypeCode.Integer;
            if (columnDataType == typeof(Double))
                return CtpTypeCode.Double;
            if (columnDataType == typeof(Single))
                return CtpTypeCode.Single;
            if (columnDataType == typeof(CtpNumeric))
                return CtpTypeCode.Numeric;
            if (columnDataType == typeof(DateTime))
                return CtpTypeCode.CtpTime;
            if (columnDataType == typeof(bool))
                return CtpTypeCode.Boolean;
            if (columnDataType == typeof(Guid))
                return CtpTypeCode.Guid;
            if (columnDataType == typeof(String))
                return CtpTypeCode.String;
            if (columnDataType == typeof(byte[]))
                return CtpTypeCode.CtpBuffer;

            return CtpTypeCode.Null;
        }

        public static Type ToType(CtpTypeCode columnTypeCode)
        {
            switch (columnTypeCode)
            {
                case CtpTypeCode.Null:
                    return typeof(DBNull);
                case CtpTypeCode.Integer:
                    return typeof(Int64);
                case CtpTypeCode.Single:
                    return typeof(Single);
                case CtpTypeCode.Double:
                    return typeof(Double);
                case CtpTypeCode.Numeric:
                    return typeof(CtpNumeric);
                case CtpTypeCode.CtpTime:
                    return typeof(CtpTime);
                case CtpTypeCode.Boolean:
                    return typeof(Boolean);
                case CtpTypeCode.Guid:
                    return typeof(Guid);
                case CtpTypeCode.String:
                    return typeof(String);
                case CtpTypeCode.CtpBuffer:
                    return typeof(byte[]);
                case CtpTypeCode.CtpCommand:
                    return typeof(byte[]);
                default:
                    throw new ArgumentOutOfRangeException(nameof(columnTypeCode), columnTypeCode, null);
            }
        }
    }
}