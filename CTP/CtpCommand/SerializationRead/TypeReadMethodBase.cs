using System;
using System.Reflection;

namespace CTP.SerializationRead
{
    internal abstract class TypeReadMethodBase<T>
    {
        public abstract T Load(CtpCommandReader reader);
    }

}
