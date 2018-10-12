using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TypeSerializationList<T>
        : TypeSerializationMethodBase<List<T>>
    {
        private static CtpDocumentName Item = CtpDocumentName.Create("Item");

        private TypeSerializationMethodBase<T> m_serializeT;

        public TypeSerializationList()
        {
            TypeSerialization<List<T>>.Serialization = this; //This is required to fix circular reference issues.
            m_serializeT = TypeSerialization<T>.Serialization;
        }

        public override List<T> Load(CtpDocumentReader reader)
        {
            List<T> items = new List<T>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpDocumentNodeType.StartElement:
                        if (reader.ElementName != Item)
                            throw new Exception("Expecting An Array Type");
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CtpDocumentNodeType.Value:
                        if (reader.ValueName != Item)
                            throw new Exception("Expecting An Array Type");
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CtpDocumentNodeType.EndElement:
                        return items;
                    case CtpDocumentNodeType.EndOfDocument:
                    case CtpDocumentNodeType.StartOfDocument:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new ArgumentOutOfRangeException();

        }

        public override void Save(List<T> obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            using (writer.StartElement(recordName))
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    m_serializeT.Save(obj[i], writer, Item);
                }

            }
        }

    }
}