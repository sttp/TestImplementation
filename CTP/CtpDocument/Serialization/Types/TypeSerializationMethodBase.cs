using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase<T>
    {
        /// <summary>
        /// Gets if null can be assigned to this field.
        /// </summary>
        public abstract bool CanAcceptNulls { get; }

        public abstract T Load(CtpDocumentReader reader);

        public abstract void Save(T obj, CtpDocumentWriter writer, CtpDocumentNames recordName);
    }
}
