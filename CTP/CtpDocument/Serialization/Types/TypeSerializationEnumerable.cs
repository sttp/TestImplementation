using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    internal class TypeSerializationEnumerable<TEnum, T>
        : TypeSerializationMethodBase<TEnum>
        where TEnum : IEnumerable<T>
    {
        public override bool IsArrayType => true;
        public override bool IsValueType => false;
        private TypeSerializationMethodBase<T> m_serializeT;
        private Func<List<T>, TEnum> m_castToType;

        public TypeSerializationEnumerable(Func<List<T>, TEnum> castToType)
        {
            DocumentSerializationHelper<TEnum>.Serialization = this; //This is required to fix circular reference issues.
            m_castToType = castToType;
            m_serializeT = DocumentSerializationHelper<T>.Serialization;
        }

        public override TEnum Load(CtpDocumentElement reader)
        {
            if (!reader.IsArray)
                throw new Exception("Expecting an array type");
            List<T> items = new List<T>();
            foreach (var element in reader.ChildElements)
            {
                items.Add(m_serializeT.Load(element));
            }
            foreach (var element in reader.ChildValues)
            {
                items.Add(m_serializeT.Load(element.Value));
            }
            return m_castToType(items);
        }

        public override void Save(TEnum obj, CtpDocumentWriter writer)
        {
            if (!writer.IsArrayElement)
                throw new Exception("Expecting an array type");
            if (m_serializeT.IsValueType)
            {
                foreach (var item in obj)
                {
                    writer.WriteValue(null, m_serializeT.Save(item));
                }
            }
            else
            {
                foreach (var item in obj)
                {
                    using (writer.StartElement(null, m_serializeT.IsArrayType))
                    {
                        m_serializeT.Save(item, writer);
                    }
                }
            }
        }

    }
}