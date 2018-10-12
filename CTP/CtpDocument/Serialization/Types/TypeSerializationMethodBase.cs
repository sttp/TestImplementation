using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase<T>
    {
        public abstract T Load(CtpDocumentReader reader);

        public abstract void Save(T obj, CtpDocumentWriter writer, CtpDocumentName recordName);
    }
}
