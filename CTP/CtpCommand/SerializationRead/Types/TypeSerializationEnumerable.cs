using System;
using System.Collections.Generic;

namespace CTP.SerializationRead
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class TypeSerializationEnumerable<TEnum, T>
        : TypeSerializationMethodBase<TEnum>
        where TEnum : IEnumerable<T>
    {
        private TypeSerializationMethodBase<T> m_serializeT;

        private Func<List<T>, TEnum> m_castToType;

        public TypeSerializationEnumerable(Func<List<T>, TEnum> castToType)
        {
            TypeSerialization<TEnum>.Set(this); //This is required to fix circular reference issues.
            m_castToType = castToType;
            m_serializeT = TypeSerialization<T>.Get();
        }

        public override TEnum Load(CtpCommandReader reader)
        {
            if (!reader.IsArray)
                throw new Exception("Expecting An Array Type");
            List<T> items = new List<T>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CtpCommandNodeType.Value:
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CtpCommandNodeType.EndElement:
                        return m_castToType(items);
                    case CtpCommandNodeType.EndOfCommand:
                    case CtpCommandNodeType.StartOfCommand:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new ArgumentOutOfRangeException();

        }
        
    }
}