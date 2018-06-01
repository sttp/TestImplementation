using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase<T>
    {
        public abstract bool IsArrayType { get; }
        public abstract bool IsValueType { get; }

        public virtual T Load(CtpDocumentElement reader)
        {
            throw new NotSupportedException();
        }

        public virtual T Load(CtpObject reader)
        {
            throw new NotSupportedException();
        }

        public virtual CtpObject Save(T obj)
        {
            throw new NotSupportedException();
        }

        public virtual void Save(T obj, CtpDocumentWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
