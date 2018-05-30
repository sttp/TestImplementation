using System;
using System.Reflection;
using System.Xml;
using CTP;
using GSF.Reflection;

namespace CTP.Serialization
{
    public abstract class CompiledSaveLoad
    {
        public abstract void Save(object obj, CtpDocumentWriter writer, string elementName);
        public abstract void Load(object obj, CtpDocumentElement reader, string elementName);
    }

    public class CompiledSaveLoad<T>
        : CompiledSaveLoad
    {
        private CtpSerializeFieldAttribute m_autoLoad;
        private TypeSerializationMethodBase<T> m_method;
        private Func<object, T> m_read;
        private Action<object, T> m_write;

        public CompiledSaveLoad(TypeSerializationMethodBase<T> method, PropertyInfo field, CtpSerializeFieldAttribute autoLoad)
        {
            m_autoLoad = autoLoad;
            m_method = method;
            m_read = field.CompileGetter<T>();
            m_write = field.CompileSetter<T>();
        }

        public CompiledSaveLoad(TypeSerializationMethodBase<T> method, FieldInfo field, CtpSerializeFieldAttribute autoLoad)
        {
            m_autoLoad = autoLoad;
            m_method = method;
            m_read = field.CompileGetter<T>();
            m_write = field.CompileSetter<T>();
        }

        public override void Save(object obj, CtpDocumentWriter writer, string elementName)
        {
            var item = m_read(obj);
            if (m_method.IsValueType)
            {
                writer.WriteValue(elementName, m_method.Save(item));
            }
            else
            {
                using (writer.StartElement(elementName))
                {
                    m_method.Save(item, writer);
                }
            }
        }

        public override void Load(object obj, CtpDocumentElement reader, string elementName)
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