using System;
using System.Linq;
using System.Reflection;

namespace CTP.Serialization
{
    internal class FieldOptions<T>
    {
        private CtpSerializeFieldAttribute m_autoLoad;
        private CompiledSaveLoad m_saveLoad;
        private MemberInfo m_info;
        private Type m_fieldType;

        public FieldOptions(MemberInfo info)
        {
            object[] attributes = info.GetCustomAttributes(true);
            m_autoLoad = attributes.OfType<CtpSerializeFieldAttribute>().FirstOrDefault();
            m_info = info;

            if (m_info is FieldInfo)
            {
                m_fieldType = ((FieldInfo)m_info).FieldType;
            }
            else if (m_info is PropertyInfo)
            {
                m_fieldType = ((PropertyInfo)m_info).PropertyType;
            }
            else
            {
                throw new NotSupportedException();
            }
            RecordName = m_autoLoad?.RecordName ?? info.Name;

        }

        public void InitializeSerializationMethod()
        {
            m_saveLoad = TypeSerialization.GetMethod(m_fieldType).CompiledSaveLoad(m_info, m_autoLoad);
        }

        public bool IsValid
        {
            get
            {
                return m_autoLoad != null;
            }
        }

        public string RecordName { get; private set; }

        public void Load(object obj, CtpDocumentElement reader)
        {
            m_saveLoad.Load(obj, reader, RecordName);
        }

        public void Save(object obj, CtpDocumentWriter writer)
        {
            m_saveLoad.Save(obj, writer, RecordName);
        }
    }
}