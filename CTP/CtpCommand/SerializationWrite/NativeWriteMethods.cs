using System;
using System.Collections.Generic;

namespace CTP.SerializationWrite
{
    internal static class NativeWriteMethods
    {
        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static NativeWriteMethods()
        {
            Add((x) => new TypeWriteDecimal(x));
            Add((x) => new TypeWriteGuid(x));
            Add((x) => new TypeWriteUInt16(x));
            Add((x) => new TypeWriteInt16(x));
            Add((x) => new TypeWriteChar(x));
            Add((x) => new TypeWriteUInt32(x));
            Add((x) => new TypeWriteInt32(x));
            Add((x) => new TypeWriteSingle(x));
            Add((x) => new TypeWriteUInt64(x));
            Add((x) => new TypeWriteInt64(x));
            Add((x) => new TypeWriteDouble(x));
            Add((x) => new TypeWriteDateTime(x));
            Add((x) => new TypeWriteCtpTime(x));
            Add((x) => new TypeWriteUInt8(x));
            Add((x) => new TypeWriteInt8(x));
            Add((x) => new TypeWriteBool(x));

            Add((x) => new TypeWriteDecimalNull(x));
            Add((x) => new TypeWriteGuidNull(x));
            Add((x) => new TypeWriteUInt16Null(x));
            Add((x) => new TypeWriteInt16Null(x));
            Add((x) => new TypeWriteCharNull(x));
            Add((x) => new TypeWriteUInt32Null(x));
            Add((x) => new TypeWriteInt32Null(x));
            Add((x) => new TypeWriteSingleNull(x));
            Add((x) => new TypeWriteUInt64Null(x));
            Add((x) => new TypeWriteInt64Null(x));
            Add((x) => new TypeWriteDoubleNull(x));
            Add((x) => new TypeWriteDateTimeNull(x));
            Add((x) => new TypeWriteCtpTimeNull(x));
            Add((x) => new TypeWriteUInt8Null(x));
            Add((x) => new TypeWriteInt8Null(x));
            Add((x) => new TypeWriteBoolNull(x));

            Add((x) => new TypeWriteString(x));
            Add((x) => new TypeWriteByteArray(x));
            Add((x) => new TypeWriteCharArray(x));
            Add((x) => new TypeWriteCommand(x));
            Add((x) => new TypeWriteBuffer(x));
            Add((x) => new TypeWriteNumeric(x));
            Add((x) => new TypeWriteNumericNull(x));
            Add((x) => new TypeWriteCtpObject(x));
            Add((x) => new TypeWriteObject(x));
        }

        static void Add<T>(Func<int, TypeWriteMethodBase<T>> method)
        {
            Methods.Add(typeof(T), method);
        }

        public static TypeWriteMethodBase<T> TryGetMethod<T>(int recordID)
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                return ((Func<int, TypeWriteMethodBase<T>>)value)(recordID);
            }
            return null;
        }


    }
}