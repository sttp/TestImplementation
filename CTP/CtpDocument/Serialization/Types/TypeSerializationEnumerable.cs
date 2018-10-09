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

        public override TEnum Load(CtpDocumentElement reader)
        {
            if (reader == null)
                return default(TEnum);

            List<T> items = new List<T>();
            foreach (var element in reader.ChildElements)
            {
                if (element.ElementName != "Item")
                    throw new Exception("Expecting An Array Type");
                items.Add(m_serializeT.Load(element));
            }
            foreach (var element in reader.ChildValues)
            {
                if (element.ValueName != "Item")
                    throw new Exception("Expecting An Array Type");
                items.Add(m_serializeT.Load(element.Value));
            }
            return m_castToType(items);
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