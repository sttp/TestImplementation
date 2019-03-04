using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeIOMethodBase<T>
    {
        public abstract void Save(T obj, CtpObjectWriter writer);
        public abstract void WriteSchema(CommandSchemaWriter schema);
        public abstract T Load(CtpCommandReader reader);
    }

}
