using System;
using System.Linq;
using System.Reflection;

namespace CTP.Serialization
{
    /// <summary>
    /// Basic serialization of classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class TypeSerialization<T>
    {
        //ToDO: Make a getter so errors can be thrown if there was a static constructor error.
        internal static readonly TypeSerializationMethodBase<T> Serialization;
        private static Exception LoadError;

        static TypeSerialization()
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
                    var attr = type.GetCustomAttributes(true).OfType<CtpSerializableAttribute>().FirstOrDefault();
                    if (attr == null)
                    {
                        LoadError = new Exception("Specified type must have the attribute CtpSerializableAttribute");
                        return;
                    }

                    Serialization = new RuntimeSerializationMethod<T>(c, attr);
                    Serialization.InitializeSerializationMethod();
                }
            }
        }

        public static CtpDocument Save(T obj)
        {
            if (LoadError != null)
                throw LoadError;
            var item = Serialization as RuntimeSerializationMethod<T>;
            if (item == null)
                throw new NotSupportedException();

            var wr = new CtpDocumentWriter(item.Attr.RootCommandName);
            Serialization.Save(obj, wr);
            return wr.ToCtpDocument();
        }

        public static T Load(CtpDocument document)
        {
            if (LoadError != null)
                throw LoadError;
            var item = Serialization as RuntimeSerializationMethod<T>;
            if (item == null)
                throw new NotSupportedException();
            if (item.Attr.RootCommandName != document.RootElement)
                throw new Exception("Document Mismatch");

            return Serialization.Load(document.MakeReader().ReadEntireElement());
        }
    }
}
