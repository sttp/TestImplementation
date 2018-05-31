using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    internal static class BuiltinSerializationMethods
    {
        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static BuiltinSerializationMethods()
        {
            Add(new TypeSerializationDecimal());
            Add(new TypeSerializationGuid());
            Add(new TypeSerializationUInt16());
            Add(new TypeSerializationInt16());
            Add(new TypeSerializationChar());
            Add(new TypeSerializationUInt32());
            Add(new TypeSerializationInt32());
            Add(new TypeSerializationSingle());
            Add(new TypeSerializationUInt64());
            Add(new TypeSerializationInt64());
            Add(new TypeSerializationDouble());
            Add(new TypeSerializationDateTime());
            Add(new TypeSerializationUInt8());
            Add(new TypeSerializationInt8());
            Add(new TypeSerializationBool());

            Add(new TypeSerializationDecimalNull());
            Add(new TypeSerializationGuidNull());
            Add(new TypeSerializationUInt16Null());
            Add(new TypeSerializationInt16Null());
            Add(new TypeSerializationCharNull());
            Add(new TypeSerializationUInt32Null());
            Add(new TypeSerializationInt32Null());
            Add(new TypeSerializationSingleNull());
            Add(new TypeSerializationUInt64Null());
            Add(new TypeSerializationInt64Null());
            Add(new TypeSerializationDoubleNull());
            Add(new TypeSerializationDateTimeNull());
            Add(new TypeSerializationUInt8Null());
            Add(new TypeSerializationInt8Null());
            Add(new TypeSerializationBoolNull());

            Add(new TypeSerializationString());
            Add(new TypeSerializationByteArray());
            Add(new TypeSerializationCtpObject());
        }

        static void Add<T>(TypeSerializationMethodBase<T> method)
        {
            Methods.Add(typeof(T), method);
        }

        public static TypeSerializationMethodBase<T> TryGetMethod<T>()
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                return (TypeSerializationMethodBase<T>)value;
            }
            return null;
        }


    }
}