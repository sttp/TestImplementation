using System;
using System.Collections.Generic;
using System.Reflection;

namespace CTP.Serialization
{
    internal static class GenericInitialization
    {
        private static readonly MethodInfo m_genericArray;
        private static readonly MethodInfo m_generic1;
        private static readonly MethodInfo m_generic1Struct;
        private static readonly MethodInfo m_generic2;

        static GenericInitialization()
        {
            m_genericArray = typeof(GenericInitialization).GetMethod("GenericArray", BindingFlags.Static | BindingFlags.NonPublic);
            m_generic1 = typeof(GenericInitialization).GetMethod("Generic1", BindingFlags.Static | BindingFlags.NonPublic);
            m_generic1Struct = typeof(GenericInitialization).GetMethod("Generic1Struct", BindingFlags.Static | BindingFlags.NonPublic);
            m_generic2 = typeof(GenericInitialization).GetMethod("Generic2", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static bool TryCreateMethod(Type type, out TypeSerializationMethodBase method)
        {
            method = null;

            if (type.IsArray)
            {
                var func = m_genericArray.MakeGenericMethod(type, type.GetElementType());
                method = (TypeSerializationMethodBase)func.Invoke(null, null);
            }
            else if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                if (types.Length == 1)
                {
                    if (types[0].IsValueType)
                    {
                        var func = m_generic1Struct.MakeGenericMethod(type, types[0]);
                        method = (TypeSerializationMethodBase)func.Invoke(null, null);
                    }
                    else
                    {
                        var func = m_generic1.MakeGenericMethod(type, types[0]);
                        method = (TypeSerializationMethodBase)func.Invoke(null, null);
                    }
                }
                else if (types.Length == 2)
                {
                    var func = m_generic2.MakeGenericMethod(type, types[0], types[1]);
                    method = (TypeSerializationMethodBase)func.Invoke(null, null);
                }
            }
            return method != null;
        }

        private static TypeSerializationMethodBase GenericArray<TType, TGeneric1>()
        {
            if (typeof(TType) == typeof(TGeneric1[]))
            {
                return new TypeSerializationArray<TGeneric1>();
            }
            else
            {
                throw new Exception("Multidimensional arrays not supported");
            }
        }

        private static TypeSerializationMethodBase Generic1Struct<TType, TGeneric1>()
            where TGeneric1 : struct
        {
            var item = Generic1<TType, TGeneric1>();
            if (item != null)
            {
                return item;
            }
            if (typeof(TType) == typeof(TGeneric1?))
            {
                return null;
            }
            return null;
        }

        private static TypeSerializationMethodBase Generic1<TType, TGeneric1>()
        {
            if (typeof(TType) == typeof(List<TGeneric1>))
            {
                return new TypeSerializationIList<List<TGeneric1>, TGeneric1>(x => new List<TGeneric1>(x));
            }
            return null;
        }

    }
}