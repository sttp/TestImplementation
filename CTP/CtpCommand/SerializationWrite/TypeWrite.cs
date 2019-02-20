using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace CTP.SerializationWrite
{
    /// <summary>
    /// This class assists in the automatic serialization of <see cref="CommandObject"/>s to and from <see cref="CtpCommand"/>s.
    /// </summary>
    internal static class TypeWrite
    {
        public static void Get<T>(out TypeWriteMethodBase<T> method, out CommandSchemaWriter schema, out string keyword)
           where T : CommandObject
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(false).OfType<CommandNameAttribute>().FirstOrDefault();
            keyword = attribute?.CommandName ?? type.Name;
            schema = new CommandSchemaWriter();
            method = GetStrongTyped<T>(schema, keyword);
        }

        /// <summary>
        /// Used by other serialization methods to acquire child serialization methods
        /// </summary>
        /// <returns></returns>
        public static TypeWriteMethodBase<T> GetUnknownType<T>(CommandSchemaWriter schema, string recordName)
        {
            var serialization = NativeWriteMethods.TryGetMethod<T>(schema, recordName);
            if (serialization != null)
                return serialization;

            serialization = WriteEnumerableMethods.TryCreate<T>(schema, recordName);
            if (serialization != null)
                return serialization;

            if (typeof(T).IsSubclassOf(typeof(CommandObject)))
            {
                var strongTyped = Method.MakeGenericMethod(typeof(T));
                return (TypeWriteMethodBase<T>)strongTyped.Invoke(null, new object[] { schema, recordName });
            }

            throw new NotSupportedException(typeof(T).FullName + " Is not a supported type for serialization.");
        }

        /// <summary>
        /// Used by other serialization methods to acquire child serialization methods
        /// </summary>
        /// <returns></returns>
        public static TypeWriteMethodBase<T> GetStrongTyped<T>(CommandSchemaWriter schema, string recordName)
            where T : CommandObject
        {
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
                throw new Exception("Specified type must have a parameterless constructor. This can be a private constructor.");
            }

            return new CommandObjectWriteMethod<T>(schema, recordName);
        }

        private static readonly MethodInfo Method = typeof(TypeWrite).GetMethod("GetStrongTyped", BindingFlags.Static | BindingFlags.NonPublic);
    }
}
