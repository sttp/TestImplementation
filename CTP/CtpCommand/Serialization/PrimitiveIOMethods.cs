using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    internal abstract class PrimitiveIOMethodBase<T>
    {
        public abstract void Save(T obj, CtpObjectWriter writer);
        public abstract T Load(CtpCommandReader reader);
    }

    internal static class PrimitiveIOMethods
    {
        private class NativeIO<T>
            : TypeIOMethodBase<T>
        {
            private PrimitiveIOMethodBase<T> m_io;
            private string m_recordName;

            public NativeIO(PrimitiveIOMethodBase<T> io, string recordName)
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
                schema.Value(m_recordName);
            }

            public override T Load(CtpCommandReader reader)
            {
                return m_io.Load(reader);
            }
        }

        private static readonly Dictionary<Type, object> Methods = new Dictionary<Type, object>();

        static PrimitiveIOMethods()
        {
            Add(new PrimitiveIoDecimal());
            Add(new PrimitiveIoGuid());
            Add(new PrimitiveIouInt16());
            Add(new PrimitiveIoInt16());
            Add(new PrimitiveIoChar());
            Add(new PrimitiveIouInt32());
            Add(new PrimitiveIoInt32());
            Add(new PrimitiveIoSingle());
            Add(new PrimitiveIouInt64());
            Add(new PrimitiveIoInt64());
            Add(new PrimitiveIoDouble());
            Add(new PrimitiveIoDateTime());
            Add(new PrimitiveIoCtpTime());
            Add(new PrimitiveIouInt8());
            Add(new PrimitiveIoInt8());
            Add(new PrimitiveIoBool());

            Add(new PrimitiveIoString());
            Add(new PrimitiveIoByteArray());
            Add(new PrimitiveIoCharArray());
            Add(new PrimitiveIoCommand());
            Add(new PrimitiveIoBuffer());
            Add(new PrimitiveIoNumeric());
            Add(new PrimitiveIoNumericNull());
            Add(new PrimitiveIoCtpObject());
            Add(new PrimitiveIoObject());
        }

        static void Add<T>(PrimitiveIOMethodBase<T> method)
        {
            Methods.Add(typeof(T), method);
        }

        public static TypeIOMethodBase<T> TryGetWriteMethod<T>(string record)
        {
            if (Methods.TryGetValue(typeof(T), out object value))
            {
                return new NativeIO<T>((PrimitiveIOMethodBase<T>)value, record);
            }
            return null;
        }

    }
}