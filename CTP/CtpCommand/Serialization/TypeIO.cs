using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using CTP.Serialization;

namespace CTP.Serialization
{
    /// <summary>
    /// This class assists in the automatic serialization of <see cref="CommandObject"/>s to and from <see cref="CtpCommand"/>s.
    /// </summary>
    internal static class TypeIO
    {
        private static readonly MethodInfo GenericCollectionMethod;
        private static readonly MethodInfo GenericEnumMethod;
        private static readonly MethodInfo GenericNullableMethod;
        private static readonly MethodInfo Generic2Method;

        static TypeIO()
        {
            GenericCollectionMethod = typeof(TypeIO).GetMethod("GenericCollection", BindingFlags.Static | BindingFlags.NonPublic);
            GenericEnumMethod = typeof(TypeIO).GetMethod("GenericEnum", BindingFlags.Static | BindingFlags.NonPublic);
            GenericNullableMethod = typeof(TypeIO).GetMethod("GenericNullable", BindingFlags.Static | BindingFlags.NonPublic);
            Generic2Method = typeof(TypeIO).GetMethod("Generic2", BindingFlags.Static | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Used by other serialization methods to acquire child serialization methods
        /// </summary>
        /// <returns></returns>
        public static TypeIOMethodBase<T> Create<T>(string recordName)
        {
            var serialization = PrimitiveIOMethods.TryGetWriteMethod<T>(recordName);
            if (serialization != null)
                return serialization;

            serialization = TryCreate<T>(recordName);
            if (serialization != null)
                return serialization;

            var type = typeof(T);

            if (!type.IsClass)
                throw new Exception("Specified type must be of type class");
            if (type.IsAbstract)
                throw new Exception("Specified type cannot be an abstract or static type");
            if (type.IsInterface)
                throw new Exception("Specified type cannot be an interface type");

            var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if ((object)c == null)
            {
                throw new Exception("Specified type must have a parameterless constructor. This can be a private constructor. " + typeof(T).ToString());
            }

            return new CommandObjectIOMethod<T>(c, recordName);
        }

        private static TypeIOMethodBase<T> TryCreate<T>(string recordName)
        {
            MethodInfo func;
            var type = typeof(T);
            if (type.IsArray)
            {
                func = GenericCollectionMethod.MakeGenericMethod(type, type.GetElementType());
            }
            else if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                if (types.Length == 1)
                {
                    if (type.IsValueType)
                    {
                        func = GenericNullableMethod.MakeGenericMethod(type, types[0]);
                    }
                    else
                    {
                        func = GenericCollectionMethod.MakeGenericMethod(type, types[0]);
                    }
                }
                else if (types.Length == 2)
                {
                    func = Generic2Method.MakeGenericMethod(type, types[0], types[1]);
                }
                else
                    return null;
            }
            else if (type.IsEnum)
            {
                func = GenericEnumMethod.MakeGenericMethod(type);
            }
            else
            {
                return null;
            }
            return (TypeIOMethodBase<T>)func.Invoke(null, new object[] { recordName });

        }

        // ReSharper disable once UnusedMember.Local
        private static object GenericCollection<TEnum, T>(string recordName)
        {
            if (typeof(TEnum) == typeof(T[]))
            {
                return new EnumerableIOType<T[], T>(recordName, x => x);
            }
            if (typeof(TEnum) == typeof(List<T>))
            {
                return new EnumerableIOType<List<T>, T>(recordName, x => x?.ToList());
            }
            if (typeof(TEnum) == typeof(HashSet<T>))
            {
                return new EnumerableIOType<HashSet<T>, T>(recordName, x => x == null ? null : new HashSet<T>(x));
            }
            if (typeof(TEnum) == typeof(SortedSet<T>))
            {
                return new EnumerableIOType<SortedSet<T>, T>(recordName, x => x == null ? null : new SortedSet<T>(x));
            }
            if (typeof(TEnum) == typeof(ReadOnlyCollection<T>))
            {
                return new EnumerableIOType<ReadOnlyCollection<T>, T>(recordName, x => x == null ? null : new ReadOnlyCollection<T>(x));
            }
            throw new Exception("Cannot serialize generic type");
        }

        // ReSharper disable once UnusedMember.Local
        private static object GenericEnum<T>(string recordName)
            where T : struct
        {
            return new EnumSerializationMethod<T>(recordName);
        }

        // ReSharper disable once UnusedMember.Local
        private static object GenericNullable<TType, TGeneric1>(string recordName)
            where TGeneric1 : struct
        {
            if (typeof(TType) == typeof(TGeneric1?))
            {
                return new NullableSerializationMethod<TGeneric1>(recordName);
            }

            return null;
        }


        // ReSharper disable once UnusedMember.Local
        private static object Generic2<TType, TGeneric1, TGeneric2>(string recordName)
        {
            if (typeof(TType) == typeof(KeyValuePair<TGeneric1, TGeneric2>))
            {

            }
            if (typeof(TType) == typeof(Dictionary<TGeneric1, TGeneric2>))
            {
                return new IDictionarySerialization<Dictionary<TGeneric1, TGeneric2>, TGeneric1, TGeneric2>(recordName, x => new Dictionary<TGeneric1, TGeneric2>(x));
            }
            if (typeof(TType) == typeof(SortedList<TGeneric1, TGeneric2>))
            {
                return new IDictionarySerialization<SortedList<TGeneric1, TGeneric2>, TGeneric1, TGeneric2>(recordName, x => new SortedList<TGeneric1, TGeneric2>(x));
            }
            if (typeof(TType) == typeof(SortedDictionary<TGeneric1, TGeneric2>))
            {
                return new IDictionarySerialization<SortedDictionary<TGeneric1, TGeneric2>, TGeneric1, TGeneric2>(recordName, x => new SortedDictionary<TGeneric1, TGeneric2>(x));
            }
            return null;
        }

    }
}
