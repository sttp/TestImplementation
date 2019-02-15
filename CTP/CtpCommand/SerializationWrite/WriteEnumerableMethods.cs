using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CTP.SerializationWrite
{
    /// <summary>
    /// Creates methods for serializing one of these types: Array, List, HashSet, SortedSet, or ReadOnlyCollection
    /// </summary>
    internal static class WriteEnumerableMethods
    {
        private static readonly MethodInfo Method;

        static WriteEnumerableMethods()
        {
            Method = typeof(WriteEnumerableMethods).GetMethod("Generic", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static TypeWriteMethodBase<T> TryCreate<T>(CommandSchemaWriter schema, string recordName)
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var func = Method.MakeGenericMethod(type, type.GetElementType());
                return (TypeWriteMethodBase<T>)func.Invoke(null, new object[] { schema, recordName });
            }
            else if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                if (types.Length == 1)
                {
                    var func = Method.MakeGenericMethod(type, types[0]);
                    return (TypeWriteMethodBase<T>)func.Invoke(null, new object[] { schema, recordName });
                }
                else
                {
                    throw new Exception("Cannot serialize generic type");
                }
            }
            return null;
        }

        // ReSharper disable once UnusedMember.Local
        private static object Generic<TEnum, T>(CommandSchemaWriter schema, string recordName)
        {
            if (typeof(TEnum) == typeof(T[]))
            {
                return new TypeWriteArray<T>(schema, recordName);
            }
            if (typeof(TEnum) == typeof(List<T>))
            {
                return new TypeWriteList<T>(schema, recordName);
            }
            if (typeof(TEnum) == typeof(HashSet<T>))
            {
                return new TypeWriteEnumerable<HashSet<T>, T>(schema, recordName);
            }
            if (typeof(TEnum) == typeof(SortedSet<T>))
            {
                return new TypeWriteEnumerable<SortedSet<T>, T>(schema, recordName);
            }
            if (typeof(TEnum) == typeof(ReadOnlyCollection<T>))
            {
                return new TypeWriteEnumerable<ReadOnlyCollection<T>, T>(schema, recordName);
            }
            throw new Exception("Cannot serialize generic type");
        }

    }
}