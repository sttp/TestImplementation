using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CTP.SerializationRead
{
    /// <summary>
    /// Creates methods for serializing one of these types: Array, List, HashSet, SortedSet, or ReadOnlyCollection
    /// </summary>
    internal static class EnumerableSerializationMethods
    {
        private static readonly MethodInfo Method;

        static EnumerableSerializationMethods()
        {
            Method = typeof(EnumerableSerializationMethods).GetMethod("Generic", BindingFlags.Static | BindingFlags.NonPublic);
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
                else
                {
                    throw new Exception("Cannot serialize generic type");
                }
            }
            return null;
        }

        // ReSharper disable once UnusedMember.Local
        private static object Generic<TEnum, T>()
        {
            if (typeof(TEnum) == typeof(T[]))
            {
                return new TypeSerializationArray<T>();
            }
            if (typeof(TEnum) == typeof(List<T>))
            {
                return new TypeSerializationList<T>();
            }
            if (typeof(TEnum) == typeof(HashSet<T>))
            {
                return new TypeSerializationEnumerable<HashSet<T>, T>(x => new HashSet<T>(x));
            }
            if (typeof(TEnum) == typeof(SortedSet<T>))
            {
                return new TypeSerializationEnumerable<SortedSet<T>, T>(x => new SortedSet<T>(x));
            }
            if (typeof(TEnum) == typeof(ReadOnlyCollection<T>))
            {
                return new TypeSerializationEnumerable<ReadOnlyCollection<T>, T>(x => new ReadOnlyCollection<T>(x));
            }
            throw new Exception("Cannot serialize generic type");
        }

    }
}