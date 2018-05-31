using System;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase
    {
        /// <summary>
        /// The type that does the serialization.
        /// </summary>
        public abstract Type ObjectType { get; }

        /// <summary>
        /// Due to circular dependencies, this method may need to be called before assigning nested dependencies.
        /// </summary>
        public virtual void InitializeSerializationMethod()
        {
        }
    }

    internal abstract class TypeSerializationMethodBase<T>
        : TypeSerializationMethodBase
    {
        public abstract bool IsArrayType { get; }
        public abstract bool IsValueType { get; }
        public override Type ObjectType => typeof(T);

        public abstract T Load(CtpDocumentElement reader);

        public abstract T Load(CtpObject reader);

        public abstract CtpObject Save(T obj);

        public abstract void Save(T obj, CtpDocumentWriter writer);
    }

    internal abstract class TypeSerializationMethodValueType<T>
        : TypeSerializationMethodBase<T>
    {
        public sealed override bool IsValueType => true;
        public sealed override Type ObjectType => typeof(T);
        public sealed override bool IsArrayType => false;

        public sealed override T Load(CtpDocumentElement reader)
        {
            throw new NotSupportedException();
        }

        public sealed override void Save(T obj, CtpDocumentWriter writer)
        {
            throw new NotSupportedException();
        }
    }


}
