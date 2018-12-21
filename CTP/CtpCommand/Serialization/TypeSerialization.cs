using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace CTP.Serialization
{
    /// <summary>
    /// This class assists in the automatic serialization of <see cref="CommandObject"/>s to and from <see cref="CtpCommand"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class TypeSerialization<T>
    {
        private static TypeSerializationMethodBase<T> s_serialization;
        private static readonly Exception s_loadError;
        private static readonly CommandNameAttribute s_commandAttribute;

        /// <summary>
        /// Sets the TypeSerializationMethod variable in the instance of a circular reference.
        /// </summary>
        /// <param name="method"></param>
        public static void Set(TypeSerializationMethodBase<T> method)
        {
            Interlocked.CompareExchange(ref s_serialization, method, null);
        }

        /// <summary>
        /// Used by other serialization methods to acquire the serialization method
        /// </summary>
        /// <returns></returns>
        public static TypeSerializationMethodBase<T> Get()
        {
            if (s_loadError != null)
                throw s_loadError;
            if (s_serialization == null)
                throw new Exception("Serialization method is missing");
            return s_serialization;
        }

        public static void Get(out Exception loadError, out CtpCommandKeyword commandName, out TypeSerializationMethodBase<T> serialization)
        {
            loadError = s_loadError;
            if (loadError == null)
            {
                commandName = CtpCommandKeyword.Create(s_commandAttribute?.CommandName ?? nameof(T));
                serialization = s_serialization;
            }
            else
            {
                commandName = null;
                serialization = null;
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
                s_commandAttribute = type.GetCustomAttributes(false).OfType<CommandNameAttribute>().FirstOrDefault();

                if (!type.IsClass)
                {
                    s_loadError = new Exception("Specified type must be of type class");
                    return;
                }
                if (type.IsAbstract)
                {
                    s_loadError = new Exception("Specified type cannot be an abstract or static type");
                    return;
                }
                if (type.IsInterface)
                {
                    s_loadError = new Exception("Specified type cannot be an interface type");
                    return;
                }

                s_serialization = EnumerableSerializationMethods.TryCreate<T>();
                if (s_serialization != null)
                    return;

                var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if ((object)c == null)
                {
                    s_loadError = new Exception("Specified type must have a parameterless constructor. This can be a private constructor.");
                    return;
                }
                s_serialization = CommandObjectSerializationMethod.Create<T>(c);
            }
            catch (Exception e)
            {
                s_serialization = null;
                s_loadError = e;
            }

        }


    }
}
