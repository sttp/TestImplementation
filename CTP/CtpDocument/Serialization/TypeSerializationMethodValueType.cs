using System;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodValueType<T>
        : TypeSerializationMethodBase<T>
    {
        public sealed override bool IsValueType => true;
        public sealed override bool IsArrayType => false;

        public sealed override T Load(CtpDocumentElement reader)
        {
            throw new NotSupportedException();
        }

        public sealed override void Save(T obj, CtpDocumentWriter writer)
        {
            throw new NotSupportedException();
        }

        public override void InitializeSerializationMethod()
        {

        }
    }
}