using System;
using System.Reflection;

namespace CTP.SerializationWrite
{
    internal abstract class TypeWriteMethodBase<T>
    {
        public abstract void Save(T obj, CtpObjectWriter writer);
        public abstract void WriteSchema(CommandSchemaWriter schema);
    }

}
