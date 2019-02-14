using System;
using System.Collections.Generic;

namespace CTP.SerializationRead
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class TypeReadEnumerable<TEnum, T>
        : TypeReadMethodBase<TEnum>
        where TEnum : IEnumerable<T>
    {
        private TypeReadMethodBase<T> m_serializeT;

        private Func<List<T>, TEnum> m_castToType;

        public TypeReadEnumerable(Func<List<T>, TEnum> castToType)
        {
            TypeRead<TEnum>.Set(this); //This is required to fix circular reference issues.
            m_castToType = castToType;
            m_serializeT = TypeRead<T>.Get();
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