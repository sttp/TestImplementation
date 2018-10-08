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

        /// <summary>
        /// Gets if this object is serialized as a Value, or as an Element.
        /// 
        /// True: indicates that the load and save methods using CtpObject will function properly.
        /// False: Means that the loading and saving is done with the document reader/writer itself.
        /// </summary>
        public abstract bool IsValueRecord { get; }

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
