using System;
using System.Collections.Generic;

namespace CTP.Serialization
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
        public override bool IsValueRecord => false;

        public override bool CanAcceptNulls => true;

        private TypeSerializationMethodBase<T> m_serializeT;

        private Func<List<T>, TEnum> m_castToType;

        public TypeSerializationEnumerable(Func<List<T>, TEnum> castToType)
        {
            TypeSerialization<TEnum>.Serialization = this; //This is required to fix circular reference issues.
            m_castToType = castToType;
            m_serializeT = TypeSerialization<T>.Serialization;
        }

        public override TEnum Load(CtpDocumentReader2 reader)
        {
            if (reader == null)
                return default(TEnum);

            List<T> items = new List<T>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpDocumentNodeType.StartElement:
                        if (reader.ElementName != "Item")
                            throw new Exception("Expecting An Array Type");
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CtpDocumentNodeType.Value:
                        if (reader.ValueName != "Item")
                            throw new Exception("Expecting An Array Type");
                        items.Add(m_serializeT.Load(reader.Value));
                        break;
                    case CtpDocumentNodeType.EndElement:
                        return m_castToType(items);
                    case CtpDocumentNodeType.EndOfDocument:
                    case CtpDocumentNodeType.StartOfDocument:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new ArgumentOutOfRangeException();

        }

        public override void Save(TEnum obj, CtpDocumentWriter writer)
        {
            if (obj == null)
                return;

            if (m_serializeT.IsValueRecord)
            {
                foreach (var item in obj)
                {
                    writer.WriteValue("Item", m_serializeT.Save(item));
                }
            }
            else
            {
                foreach (var item in obj)
                {
                    using (writer.StartElement("Item"))
                    {
                        m_serializeT.Save(item, writer);
                    }
                }
            }
        }

    }
}