using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase<T>
    {
        public abstract T Load(CtpCommandReader reader);

        public abstract void Save(T obj, CtpCommandWriter writer, int recordName);
    }
}
