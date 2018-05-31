using System;
using System.Collections.Generic;

namespace CTP.Serialization
{
    internal class TypeSerializationIList<TList, T>
        : TypeSerializationMethodBase<TList>
        where TList : IList<T>
    {

        public override bool IsArrayType => true;
        public override bool IsValueType => false;
        private TypeSerializationMethodBase<T> m_serializeT;
        private Func<int, TList> m_objConstructor;

        public TypeSerializationIList(Func<int, TList> objConstructor)
        {
            m_objConstructor = objConstructor;
            m_serializeT = TypeSerialization<T>.Serialization;
        }

        public override TList Load(CtpDocumentElement reader)
        {
            if (!reader.IsArray)
                throw new Exception("Expecting an array type");
            TList items = m_objConstructor(0);
            foreach (var element in reader.ChildElements)
            {
                items.Add(m_serializeT.Load(element));
            }
            foreach (var element in reader.ChildValues)
            {
                items.Add(m_serializeT.Load(element.Value));
            }
            return items;
        }

        public override TList Load(CtpObject reader)
        {
            throw new NotSupportedException();
        }

        public override CtpObject Save(TList obj)
        {
            throw new NotSupportedException();
        }

        public override void Save(TList obj, CtpDocumentWriter writer)
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
                    using (writer.StartElement(null))
                    {
                        m_serializeT.Save(item, writer);
                    }
                }
            }
        }

       
       
    }
}