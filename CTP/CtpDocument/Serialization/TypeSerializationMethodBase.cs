using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodBase<T>
    {
        public abstract bool IsArrayType { get; }
        public abstract bool IsValueType { get; }

        public abstract T Load(CtpDocumentElement reader);

        public abstract T Load(CtpObject reader);

        public abstract CtpObject Save(T obj);

        public abstract void Save(T obj, CtpDocumentWriter writer);

        /// <summary>
        /// Due to circular dependencies, this method may need to be called before assigning nested dependencies.
        /// </summary>
        public abstract void InitializeSerializationMethod();
    }
}
