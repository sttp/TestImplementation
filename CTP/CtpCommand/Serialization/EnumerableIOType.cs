using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CTP.Serialization
{
    /// <summary>
    /// Creates methods for serializing one of these types: Array, List, HashSet, SortedSet, or ReadOnlyCollection
    /// </summary>
    internal static class EnumerableIOType
    {
        private static readonly MethodInfo Method;

        static EnumerableIOType()
        {
            Method = typeof(EnumerableIOType).GetMethod("Generic", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static TypeIOMethodBase<T> TryCreate<T>(string recordName)
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var func = Method.MakeGenericMethod(type, type.GetElementType());
                return (TypeIOMethodBase<T>)func.Invoke(null, new object[] { recordName });
            }
            else if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                if (types.Length == 1)
                {
                    var func = Method.MakeGenericMethod(type, types[0]);
                    return (TypeIOMethodBase<T>)func.Invoke(null, new object[] { recordName });
                }
                else
                {
                    throw new Exception("Cannot serialize generic type");
                }
            }
            return null;
        }

        // ReSharper disable once UnusedMember.Local
        private static object Generic<TEnum, T>(string recordName)
        {
            if (typeof(TEnum) == typeof(T[]))
            {
                return new EnumerableIOType<T[], T>(recordName, x => x.ToArray());
            }
            if (typeof(TEnum) == typeof(List<T>))
            {
                return new EnumerableIOType<List<T>, T>(recordName, x => x);
            }
            if (typeof(TEnum) == typeof(HashSet<T>))
            {
                return new EnumerableIOType<HashSet<T>, T>(recordName, x => new HashSet<T>(x));
            }
            if (typeof(TEnum) == typeof(SortedSet<T>))
            {
                return new EnumerableIOType<SortedSet<T>, T>(recordName, x => new SortedSet<T>(x));
            }
            if (typeof(TEnum) == typeof(ReadOnlyCollection<T>))
            {
                return new EnumerableIOType<ReadOnlyCollection<T>, T>(recordName, x => new ReadOnlyCollection<T>(x));
            }
            throw new Exception("Cannot serialize generic type");
        }

    }
}