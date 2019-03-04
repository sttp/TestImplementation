using System;
using System.Collections.Generic;

namespace CTP.SerializationWrite
{
    internal abstract class NativeMethodsIOBase<T>
    {
        public abstract void Save(T obj, CtpObjectWriter writer);
        //public abstract T Load(CtpCommandReader reader);
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

        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static NativeIOMethods()
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


    }
}