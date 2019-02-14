using System;
using System.Collections.Generic;

namespace CTP.SerializationRead
{
    internal static class BuiltinSerializationMethods
    {
        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static BuiltinSerializationMethods()
        {
            Add(new TypeReadDecimal());
            Add(new TypeReadGuid());
            Add(new TypeReadUInt16());
            Add(new TypeReadInt16());
            Add(new TypeReadChar());
            Add(new TypeReadUInt32());
            Add(new TypeReadInt32());
            Add(new TypeReadSingle());
            Add(new TypeReadUInt64());
            Add(new TypeReadInt64());
            Add(new TypeReadDouble());
            Add(new TypeReadDateTime());
            Add(new TypeReadCtpTime());
            Add(new TypeReadUInt8());
            Add(new TypeReadInt8());
            Add(new TypeReadBool());

            Add(new TypeReadDecimalNull());
            Add(new TypeReadGuidNull());
            Add(new TypeReadUInt16Null());
            Add(new TypeReadInt16Null());
            Add(new TypeReadCharNull());
            Add(new TypeReadUInt32Null());
            Add(new TypeReadInt32Null());
            Add(new TypeReadSingleNull());
            Add(new TypeReadUInt64Null());
            Add(new TypeReadInt64Null());
            Add(new TypeReadDoubleNull());
            Add(new TypeReadDateTimeNull());
            Add(new TypeReadCtpTimeNull());
            Add(new TypeReadUInt8Null());
            Add(new TypeReadInt8Null());
            Add(new TypeReadBoolNull());

            Add(new TypeReadString());
            Add(new TypeReadByteArray());
            Add(new TypeReadCharArray());
            Add(new TypeReadCommand());
            Add(new TypeReadBuffer());
            Add(new TypeReadNumeric());
            Add(new TypeReadNumericNull());
            Add(new TypeReadCtpObject());
            Add(new TypeReadObject());
        }

        static void Add<T>(TypeReadMethodBase<T> method)
        {
            Methods.Add(typeof(T), method);
        }

        public static TypeReadMethodBase<T> TryGetMethod<T>()
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                return (TypeReadMethodBase<T>)value;
            }
            return null;
        }


    }
}