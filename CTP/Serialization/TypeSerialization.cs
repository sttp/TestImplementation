using System;
using System.Collections.Generic;
using CTP;

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
            Add(new TypeSerializationString());
            Add(new TypeSerializationByteArray());
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

        public static TypeSerializationMethodBase<T> GetMethod<T>()
        {
            return (TypeSerializationMethodBase<T>)GetMethod(typeof(T));
        }
    }

    /// <summary>
    /// Basic serialization of classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class TypeSerialization<T>
    {
        private static readonly TypeSerializationMethodBase<T> Serialization;

        static TypeSerialization()
        {
            Serialization = TypeSerialization.GetMethod<T>();
        }

        public static T Load(CtpDocumentElement reader, string elementName)
        {
            if (Serialization.IsValueType)
            {
                return Serialization.Load(reader.GetValue(elementName));
            }
            else
            {
                return Serialization.Load(reader.GetElement(elementName));
            }
        }

        public static void Save(T value, CtpDocumentWriter writer, string elementName)
        {
            if (Serialization.IsValueType)
            {
                writer.WriteValue(elementName, Serialization.Save(value));
            }
            else
            {
                using (writer.StartElement(elementName, Serialization.IsArrayType))
                {
                    Serialization.Save(value, writer);
                }
            }
        }

    }
}
