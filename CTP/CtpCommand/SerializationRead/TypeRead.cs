using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using CTP.Serialization;
using CTP.SerializationWrite;

namespace CTP.SerializationRead
{
    /// <summary>
    /// This class assists in the automatic serialization of <see cref="CommandObject"/>s to and from <see cref="CtpCommand"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class TypeRead<T>
    {
        private static TypeReadMethodBase<T> s_read;
        private static readonly Exception s_loadError;
        private static readonly CommandNameAttribute s_commandAttribute;

        /// <summary>
        /// Sets the TypeSerializationMethod variable in the instance of a circular reference.
        /// </summary>
        /// <param name="method"></param>
        public static void Set(TypeReadMethodBase<T> method)
        {
            Interlocked.CompareExchange(ref s_read, method, null);
        }

        /// <summary>
        /// Used by other serialization methods to acquire the serialization method
        /// </summary>
        /// <returns></returns>
        public static TypeReadMethodBase<T> Get()
        {
            if (s_loadError != null)
                throw s_loadError;
            if (s_read == null)
                throw new Exception("Serialization method is missing");
            return s_read;
        }

        public static void Get(out Exception loadError, out string commandName, out TypeReadMethodBase<T> read)
        {
            loadError = s_loadError;
            if (loadError == null)
            {
                commandName = s_commandAttribute?.CommandName ?? nameof(T);
                read = s_read;
            }
            else
            {
                commandName = null;
                read = null;
            }
        }

        static TypeRead()
        {
            try
            {
                s_read = NativeIOMethods.TryGetMethod<T>();
                if (s_read != null)
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

                s_read = ReadEnumerableMethods.TryCreate<T>();
                if (s_read != null)
                    return;

                var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if ((object)c == null)
                {
                    s_loadError = new Exception("Specified type must have a parameterless constructor. This can be a private constructor.");
                    return;
                }
                s_read = new CommandObjectReadMethod<T>(c);
            }
            catch (Exception e)
            {
                s_read = null;
                s_loadError = e;
            }

        }
    }
}
