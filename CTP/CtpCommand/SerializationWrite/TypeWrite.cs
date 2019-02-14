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
        public static void Get<T>(out TypeWriteMethodBase<T> method, out SerializationSchema schema, out CtpCommandKeyword keyword)
           where T : CommandObject
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(false).OfType<CommandNameAttribute>().FirstOrDefault();
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

            keyword = CtpCommandKeyword.Create(attribute?.CommandName ?? type.Name);
            schema = new SerializationSchema(keyword);
            method = CommandObjectWriteMethod.Create<T>(false, schema, schema.WriteName(keyword));
        }

        /// <summary>
        /// Used by other serialization methods to acquire the serialization method
        /// </summary>
        /// <returns></returns>
        public static TypeWriteMethodBase<T> Get<T>(SerializationSchema schema, int recordName)
        {
            var serialization = NativeWriteMethods.TryGetMethod<T>(recordName);
            if (serialization != null)
                return serialization;

            var type = typeof(T);

            if (!type.IsClass)
                throw new Exception("Specified type must be of type class");
            if (type.IsAbstract)
                throw new Exception("Specified type cannot be an abstract or static type");
            if (type.IsInterface)
                throw new Exception("Specified type cannot be an interface type");

            serialization = WriteEnumerableMethods.TryCreate<T>(schema, recordName);
            if (serialization != null)
                return serialization;

            var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if ((object)c == null)
            {
                throw new Exception("Specified type must have a parameterless constructor. This can be a private constructor.");
            }
            serialization = CommandObjectWriteMethod.Create<T>(false, schema, recordName);
            return serialization;
        }
    }
}
