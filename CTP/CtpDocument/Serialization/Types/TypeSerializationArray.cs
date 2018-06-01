using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    internal class TypeSerializationArray<T>
        : TypeSerializationMethodBase<T[]>
    {
        public override bool IsArrayType => true;
        public override bool IsValueType => false;
        private TypeSerializationMethodBase<T> m_serializeT;

        public override CtpObject Save(T[] obj)
        {
            throw new NotSupportedException();
        }

        public override T[] Load(CtpObject reader)
        {
            throw new NotSupportedException();
        }

        public override T[] Load(CtpDocumentElement reader)
        {
            List<T> items = new List<T>();
            if (!reader.IsArray)
                throw new Exception("Expecting an array type");
            foreach (var element in reader.ChildElements)
            {
                items.Add(m_serializeT.Load(element));
            }
            foreach (var element in reader.ChildValues)
            {
                items.Add(m_serializeT.Load(element.Value));
            }
            return items.ToArray();
        }

        public override void Save(T[] obj, CtpDocumentWriter writer)
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

        public override void InitializeSerializationMethod()
        {
            m_serializeT = DocumentSerializationHelper<T>.Serialization;
        }
    }
}