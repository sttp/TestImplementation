using System.Linq;
using System.Reflection;

namespace CTP.Serialization
{
    internal class FieldOptions
    {
        private CtpSerializeFieldAttribute m_autoLoad;
        private CompiledSaveLoad m_saveLoad;
        private FieldInfo m_info;

        public FieldOptions(FieldInfo info)
        {
            object[] attributes = info.GetCustomAttributes(true);
            m_autoLoad = attributes.OfType<CtpSerializeFieldAttribute>().FirstOrDefault();
            m_info = info;
            RecordName = m_autoLoad?.RecordName ?? info.Name;

        }

        public void InitializeSerializationMethod()
        {
            m_saveLoad = TypeSerialization.GetMethod(m_info.FieldType).CompiledSaveLoad(m_info, m_autoLoad);
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