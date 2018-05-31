using System;
using System.Collections.Generic;
using System.Reflection;

namespace CTP.Serialization
{
    internal static class ListSerializationMethods
    {
        private static readonly MethodInfo Method;

        static ListSerializationMethods()
        {
            Method = typeof(ListSerializationMethods).GetMethod("GenericList", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static TypeSerializationMethodBase<T> TryCreate<T>()
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var func = Method.MakeGenericMethod(type, type.GetElementType());
                return (TypeSerializationMethodBase<T>)func.Invoke(null, null);
            }
            else if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                if (types.Length == 1)
                {
                    var func = Method.MakeGenericMethod(type, types[0]);
                    return (TypeSerializationMethodBase<T>)func.Invoke(null, null);
                }
            }
            return null;
        }

        // ReSharper disable once UnusedMember.Local
        private static object GenericList<TType, TGeneric1>()
        {
            if (typeof(TType) == typeof(TGeneric1[]))
            {
                return new TypeSerializationArray<TGeneric1>();
            }
            if (typeof(TType) == typeof(List<TGeneric1>))
            {
                return new TypeSerializationIList<List<TGeneric1>, TGeneric1>(() => new List<TGeneric1>());
            }
            return null;
        }

    }
}