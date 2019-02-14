using System;
using System.Collections.Generic;

namespace CTP.SerializationRead
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TypeSerializationArray<T>
        : TypeSerializationMethodBase<T[]>
    {

        private TypeSerializationMethodBase<T> m_serializeT;

        public TypeSerializationArray()
        {
            TypeSerialization<T[]>.Set(this); //This is required to fix circular reference issues.
            m_serializeT = TypeSerialization<T>.Get();
        }

        public override T[] Load(CtpCommandReader reader)
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
                        return items.ToArray();
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