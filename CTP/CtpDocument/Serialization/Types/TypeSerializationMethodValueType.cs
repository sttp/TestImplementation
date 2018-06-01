using System;

namespace CTP.Serialization
{
    internal abstract class TypeSerializationMethodValueType<T>
        : TypeSerializationMethodBase<T>
    {
        public sealed override bool IsValueType => true;
        public sealed override bool IsArrayType => false;
    }
}