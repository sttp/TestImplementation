using System;
using System.Collections.Generic;

namespace CTP.SerializationWrite
{
    internal static class NativeWriteMethods
    {
        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static NativeWriteMethods()
        {
            Add(new TypeWriteDecimal());
            Add(new TypeWriteGuid());
            Add(new TypeWriteUInt16());
            Add(new TypeWriteInt16());
            Add(new TypeWriteChar());
            Add(new TypeWriteUInt32());
            Add(new TypeWriteInt32());
            Add(new TypeWriteSingle());
            Add(new TypeWriteUInt64());
            Add(new TypeWriteInt64());
            Add(new TypeWriteDouble());
            Add(new TypeWriteDateTime());
            Add(new TypeWriteCtpTime());
            Add(new TypeWriteUInt8());
            Add(new TypeWriteInt8());
            Add(new TypeWriteBool());

            Add(new TypeWriteDecimalNull());
            Add(new TypeWriteGuidNull());
            Add(new TypeWriteUInt16Null());
            Add(new TypeWriteInt16Null());
            Add(new TypeWriteCharNull());
            Add(new TypeWriteUInt32Null());
            Add(new TypeWriteInt32Null());
            Add(new TypeWriteSingleNull());
            Add(new TypeWriteUInt64Null());
            Add(new TypeWriteInt64Null());
            Add(new TypeWriteDoubleNull());
            Add(new TypeWriteDateTimeNull());
            Add(new TypeWriteCtpTimeNull());
            Add(new TypeWriteUInt8Null());
            Add(new TypeWriteInt8Null());
            Add(new TypeWriteBoolNull());

            Add(new TypeWriteString());
            Add(new TypeWriteByteArray());
            Add(new TypeWriteCharArray());
            Add(new TypeWriteCommand());
            Add(new TypeWriteBuffer());
            Add(new TypeWriteNumeric());
            Add(new TypeWriteNumericNull());
            Add(new TypeWriteCtpObject());
            Add(new TypeWriteObject());
        }

        static void Add<T>(TypeWriteMethodBase<T> method)
        {
            Methods.Add(typeof(T), method);
        }

        public static TypeWriteMethodBase<T> TryGetMethod<T>(CommandSchemaWriter schema, string record)
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                schema.DefineValue(record);
                return (TypeWriteMethodBase<T>)value;
            }
            return null;
        }


    }
}