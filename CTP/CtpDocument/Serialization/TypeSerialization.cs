using System;
using System.Linq;
using System.Reflection;

namespace CTP.Serialization
{
    /// <summary>
    /// This class assists in the automatic serialization of <see cref="CtpDocumentObject"/>s to and from <see cref="CtpDocument"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class DocumentSerializationHelper<T>
    {
        internal static readonly TypeSerializationMethodBase<T> Serialization;
        internal static readonly Exception LoadError;
        internal static readonly CtpCommandAttribute CommandAttribute;

        static DocumentSerializationHelper()
        {
            //ToDo: Do something if there is a static constructor error.
            Serialization = BuiltinSerializationMethods.TryGetMethod<T>();
            if (Serialization == null)
            {
                var type = typeof(T);
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

                try
                {
                    Serialization = ListSerializationMethods.TryCreate<T>();
                    Serialization?.InitializeSerializationMethod();
                }
                catch (Exception e)
                {
                    LoadError = e;
                    return;
                }

                if (Serialization == null)
                {
                    var c = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                    if (c == null)
                    {
                        LoadError = new Exception("Specified type must have a parameterless constructor. Not this can be a private constructor.");
                        return;
                    }
                    CommandAttribute = type.GetCustomAttributes(true).OfType<CtpCommandAttribute>().FirstOrDefault();
                    Serialization = new RuntimeSerializationMethod<T>(c);
                    Serialization.InitializeSerializationMethod();
                }
            }
        }


    }
}
