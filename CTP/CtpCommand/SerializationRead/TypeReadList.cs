using System;
using System.Collections.Generic;

namespace CTP.SerializationRead
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TypeReadList<T>
        : TypeReadMethodBase<List<T>>
    {
        private TypeReadMethodBase<T> m_serializeT;

        public TypeReadList()
        {
            TypeRead<List<T>>.Set(this); //This is required to fix circular reference issues.
            m_serializeT = TypeRead<T>.Get();
        }

        public override List<T> Load(CtpCommandReader reader)
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
                        return items;
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