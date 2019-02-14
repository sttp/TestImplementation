using System;
using System.Reflection;

namespace CTP.SerializationWrite
{
    internal abstract class TypeWriteMethodBase<T>
    {
        public abstract void Save(T obj, CtpCommandWriter writer);
    }

}
