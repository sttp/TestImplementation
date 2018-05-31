using System;
using System.Linq;
using System.Reflection;

namespace CTP.Serialization
{
    internal abstract class FieldOptions
    {
        public abstract void InitializeSerializationMethod();

        public abstract string RecordName { get; }

        public abstract void Load(object obj, CtpDocumentElement reader);

        public abstract void Save(object obj, CtpDocumentWriter writer);

        private static readonly MethodInfo Method2 = typeof(AutoInitialization).GetMethod("CreateFieldOptionsInternal", BindingFlags.Static | BindingFlags.NonPublic);

        public static FieldOptions CreateFieldOptions(MemberInfo member, Type targetType, CtpSerializeFieldAttribute autoLoad)
        {
            var genericMethod = Method2.MakeGenericMethod(targetType);
            return (FieldOptions)genericMethod.Invoke(null, new object[] { member, targetType, autoLoad });
        }

        private static FieldOptions CreateFieldOptionsInternal<TFieldType>(MemberInfo member, Type targetType, CtpSerializeFieldAttribute autoLoad)
        {
            return new FieldOptions<TFieldType>(member, targetType, autoLoad);
        }
    }

    internal class FieldOptions<T>
        : FieldOptions
    {
        private CtpSerializeFieldAttribute m_autoLoad;
        private CompiledSaveLoad<T> m_saveLoad;
        private MemberInfo m_info;
        private Type m_fieldType;
        private string m_recordName;

        public FieldOptions(MemberInfo member, Type targetType, CtpSerializeFieldAttribute autoLoad)
        {
            m_autoLoad = autoLoad;
            m_recordName = m_autoLoad?.RecordName ?? member.Name;
        }

        public override void InitializeSerializationMethod()
        {
            m_saveLoad = new CompiledSaveLoad<T>(TypeSerialization<T>.Serialization, m_info, m_autoLoad);
        }

        public override string RecordName => m_recordName;

        public override void Load(object obj, CtpDocumentElement reader)
        {
            m_saveLoad.Load(obj, reader, RecordName);
        }

        public override void Save(object obj, CtpDocumentWriter writer)
        {
            m_saveLoad.Save(obj, writer, RecordName);
        }
    }
}