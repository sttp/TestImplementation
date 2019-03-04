using System;
using System.Collections.Generic;
using CTP.SerializationRead;
using CTP.SerializationWrite;

namespace CTP.Serialization
{
    internal abstract class NativeMethodsIOBase<T>
    {
        public abstract void Save(T obj, CtpObjectWriter writer);
        public abstract T Load(CtpCommandReader reader);
    }

    internal static class NativeIOMethods
    {
        private class Write<T>
            : TypeWriteMethodBase<T>
        {
            private NativeMethodsIOBase<T> m_io;
            private string m_recordName;

            public Write(NativeMethodsIOBase<T> io, string recordName)
            {
                m_io = io;
                m_recordName = recordName;
            }

            public override void Save(T obj, CtpObjectWriter writer)
            {
                m_io.Save(obj, writer);
            }

            public override void WriteSchema(CommandSchemaWriter schema)
            {
                schema.DefineValue(m_recordName);
            }
        }

        private class Read<T>
            : TypeReadMethodBase<T>
        {
            private NativeMethodsIOBase<T> m_io;

            public Read(NativeMethodsIOBase<T> io)
            {
                m_io = io;
            }

            public override T Load(CtpCommandReader reader)
            {
                return m_io.Load(reader);
            }
        }

        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static NativeIOMethods()
        {
            Add(new TypeIODecimal());
            Add(new TypeIOGuid());
            Add(new TypeIOUInt16());
            Add(new TypeIOInt16());
            Add(new TypeIOChar());
            Add(new TypeIOUInt32());
            Add(new TypeIOInt32());
            Add(new TypeIOSingle());
            Add(new TypeIOUInt64());
            Add(new TypeIOInt64());
            Add(new TypeIODouble());
            Add(new TypeIODateTime());
            Add(new TypeIOCtpTime());
            Add(new TypeIOUInt8());
            Add(new TypeIOInt8());
            Add(new TypeIOBool());

            Add(new TypeIODecimalNull());
            Add(new TypeIOGuidNull());
            Add(new TypeIOUInt16Null());
            Add(new TypeIOInt16Null());
            Add(new TypeIOCharNull());
            Add(new TypeIOUInt32Null());
            Add(new TypeIOInt32Null());
            Add(new TypeIOSingleNull());
            Add(new TypeIOUInt64Null());
            Add(new TypeIOInt64Null());
            Add(new TypeIODoubleNull());
            Add(new TypeIODateTimeNull());
            Add(new TypeIOCtpTimeNull());
            Add(new TypeIOUInt8Null());
            Add(new TypeIOInt8Null());
            Add(new TypeIOBoolNull());

            Add(new TypeIOString());
            Add(new TypeIOByteArray());
            Add(new TypeIOCharArray());
            Add(new TypeIOCommand());
            Add(new TypeIOBuffer());
            Add(new TypeIONumeric());
            Add(new TypeIONumericNull());
            Add(new TypeIOCtpObject());
            Add(new TypeIOObject());
        }

        static void Add<T>(NativeMethodsIOBase<T> method)
        {
            Methods.Add(typeof(T), method);
        }

        public static TypeWriteMethodBase<T> TryGetWriteMethod<T>(string record)
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                return new Write<T>((NativeMethodsIOBase<T>)value, record);
            }
            return null;
        }

        public static TypeReadMethodBase<T> TryGetMethod<T>()
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                return new Read<T>((NativeMethodsIOBase<T>)value);
            }
            return null;
        }


    }
}