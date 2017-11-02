using System;
using System.Text;

namespace Sttp.WireProtocol
{
    public static class SttpValueTypeCodec
    {
        public static SttpValue Encode(SttpValueTypeCode typeCode, object value)
        {
            var rv = new SttpValue();
            rv.SetValue(value);
            return rv;
        }

        public static object Decode(SttpValueTypeCode typeCode, SttpValue data)
        {
            return data.ToNativeType(typeCode);
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