using System;
using System.Reflection;
using System.Xml;
using CTP;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal class CompiledSaveLoad<T>
    {
        private CtpSerializeFieldAttribute m_autoLoad;
        private TypeSerializationMethodBase<T> m_method;
        private Func<object, T> m_read;
        private Action<object, T> m_write;

        public CompiledSaveLoad(TypeSerializationMethodBase<T> method, MemberInfo field, CtpSerializeFieldAttribute autoLoad)
        {
            if (field is PropertyInfo)
            {
                m_autoLoad = autoLoad;
                m_method = method;
                m_read = ((PropertyInfo)field).CompileGetter<T>();
                m_write = ((PropertyInfo)field).CompileSetter<T>();
            }
            else if (field is FieldInfo)
            {
                m_autoLoad = autoLoad;
                m_method = method;
                m_read = ((FieldInfo)field).CompileGetter<T>();
                m_write = ((FieldInfo)field).CompileSetter<T>();
            }
            else
            {
                throw new NotSupportedException();
            }

        }

        public void Save(object obj, CtpDocumentWriter writer, string elementName)
        {
            var item = m_read(obj);
            if (m_method.IsValueType)
            {
                writer.WriteValue(elementName, m_method.Save(item));
            }
            else
            {
                using (writer.StartElement(elementName, m_method.IsArrayType))
                {
                    m_method.Save(item, writer);
                }
            }
        }

        public void Load(object obj, CtpDocumentElement reader, string elementName)
        {
            T item;
            if (m_method.IsValueType)
            {
                item = m_method.Load(reader.GetValue(elementName));
            }
            else
            {
                item = m_method.Load(reader.GetElement(elementName));
            }
            m_write(obj, item);
        }
    }
}