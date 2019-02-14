using System;
using System.Reflection;

namespace CTP.SerializationRead
{
    internal abstract class TypeSerializationMethodBase<T>
    {
        public abstract T Load(CtpCommandReader reader);
    }

}
