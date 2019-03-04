using System;
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
        /// <summary>
        /// Used by other serialization methods to acquire child serialization methods
        /// </summary>
        /// <returns></returns>
        public static TypeIOMethodBase<T> Create<T>(string recordName)
        {
            var serialization = PrimitiveIOMethods.TryGetWriteMethod<T>(recordName);
            if (serialization != null)
                return serialization;

            serialization = EnumerableIOType.TryCreate<T>(recordName);
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
                throw new Exception("Specified type must have a parameterless constructor. This can be a private constructor.");
            }

            return new CommandObjectIOMethod<T>(c, recordName);
        }
    }
}
