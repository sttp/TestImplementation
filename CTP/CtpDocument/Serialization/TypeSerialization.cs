using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    internal static class TypeSerialization
    {
        private static readonly Dictionary<Type, TypeSerializationMethodBase> Methods = new Dictionary<Type, TypeSerializationMethodBase>();

        static TypeSerialization()
        {
            Add(new TypeSerializationDecimal());
            Add(new TypeSerializationGuid());
            Add(new TypeSerializationUInt16());
            Add(new TypeSerializationInt16());
            Add(new TypeSerializationChar());
            Add(new TypeSerializationUInt32());
            Add(new TypeSerializationInt32());
            Add(new TypeSerializationSingle());
            Add(new TypeSerializationUInt64());
            Add(new TypeSerializationInt64());
            Add(new TypeSerializationDouble());
            Add(new TypeSerializationDateTime());
            Add(new TypeSerializationUInt8());
            Add(new TypeSerializationInt8());
            Add(new TypeSerializationBool());


            Add(new TypeSerializationDecimalNull());
            Add(new TypeSerializationGuidNull());
            Add(new TypeSerializationUInt16Null());
            Add(new TypeSerializationInt16Null());
            Add(new TypeSerializationCharNull());
            Add(new TypeSerializationUInt32Null());
            Add(new TypeSerializationInt32Null());
            Add(new TypeSerializationSingleNull());
            Add(new TypeSerializationUInt64Null());
            Add(new TypeSerializationInt64Null());
            Add(new TypeSerializationDoubleNull());
            Add(new TypeSerializationDateTimeNull());
            Add(new TypeSerializationUInt8Null());
            Add(new TypeSerializationInt8Null());
            Add(new TypeSerializationBoolNull());

            Add(new TypeSerializationString());
            Add(new TypeSerializationByteArray());
            Add(new TypeSerializationCtpObject());
        }

        static void Add(TypeSerializationMethodBase method)
        {
            lock (Methods)
            {
                Methods.Add(method.ObjectType, method);
            }
        }

        public static TypeSerializationMethodBase GetMethod(Type type)
        {
            lock (Methods)
            {
                TypeSerializationMethodBase value;
                if (Methods.TryGetValue(type, out value))
                {
                    return value;
                }

                if (AutoInitialization.TryCreateMethod(type, out value))
                {
                    Methods.Add(type, value);
                    try
                    {
                        value.InitializeSerializationMethod();
                    }
                    catch (Exception)
                    {
                        Methods.Remove(type);
                        throw;
                    }
                    return value;
                }

                if (GenericInitialization.TryCreateMethod(type, out value))
                {
                    Methods.Add(type, value);
                    try
                    {
                        value.InitializeSerializationMethod();
                    }
                    catch (Exception)
                    {
                        Methods.Remove(type);
                        throw;
                    }
                    return value;
                }

                throw new Exception("Unknown Serialization Ability: " + type.ToString());
            }
        }
    }

    /// <summary>
    /// Basic serialization of classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class TypeSerialization<T>
    {
        //ToDO: Make a getter so errors can be thrown if there was a static constructor error.
        internal static readonly TypeSerializationMethodBase<T> Serialization;

        static TypeSerialization()
        {
            //ToDo: Do something if there is a static constructor error.
            Serialization = (TypeSerializationMethodBase<T>)TypeSerialization.GetMethod(typeof(T));
        }

        public static CtpDocument Save(T obj)
        {
            var item = Serialization as AutoSerializationMethod<T>;
            if (item == null)
                throw new NotSupportedException();

            var wr = new CtpDocumentWriter(item.Attr.RootCommandName);
            Serialization.Save(obj, wr);
            return wr.ToCtpDocument();
        }

        public static T Load(CtpDocument document)
        {
            var item = Serialization as AutoSerializationMethod<T>;
            if (item == null)
                throw new NotSupportedException();
            if (item.Attr.RootCommandName != document.RootElement)
                throw new Exception("Document Mismatch");

            return Serialization.Load(document.MakeReader().ReadEntireElement());
        }
    }
}
