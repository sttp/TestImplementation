using System;
using System.Linq;
using System.Reflection;

namespace CTP.Serialization
{

    /// <summary>
    /// This class assists in the automatic serialization of <see cref="DocumentObject"/>s to and from <see cref="CtpDocument"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class TypeSerialization<T>
    {
        private static TypeSerializationMethodBase<T> s_serialization;
        internal static readonly Exception LoadError;
        internal static readonly DocumentNameAttribute CommandAttribute;
        internal static TypeSerializationMethodBase<T> Serialization
        {
            get
            {
                if (LoadError != null)
                    throw LoadError;
                return s_serialization;
            }
            set
            {
                if (s_serialization == null)
                    return;
                s_serialization = value;
            }
        }

        static TypeSerialization()
        {
            try
            {
                s_serialization = BuiltinSerializationMethods.TryGetMethod<T>();
                if (s_serialization != null)
                    return;

                var type = typeof(T);
                CommandAttribute = type.GetCustomAttributes(false).OfType<DocumentNameAttribute>().FirstOrDefault();

                if (!type.IsClass)
                {
                    LoadError = new Exception("Specified type must be of type class");
                    return;
                }
                if (type.IsAbstract)
                {
                    LoadError = new Exception("Specified type cannot be an abstract or static type");
                    return;
                }
                if (type.IsInterface)
                {
                    LoadError = new Exception("Specified type cannot be an interface type");
                    return;
                }

                s_serialization = EnumerableSerializationMethods.TryCreate<T>();
                if (s_serialization != null)
                    return;

                var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if ((object)c == null)
                {
                    LoadError = new Exception("Specified type must have a parameterless constructor. This can be a private constructor.");
                    return;
                }
                s_serialization =  DocumentObjectSerializationMethod.Create<T>(c);
            }
            catch (Exception e)
            {
                s_serialization = null;
                LoadError = e;
            }

        }


    }
}
