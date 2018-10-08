using System;
using System.Reflection;
using GSF.Reflection;

namespace CTP.Serialization
{
    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal abstract class FieldSerialization
    {
        /// <summary>
        /// The name of the field/element to use in CtpDocument.
        /// </summary>
        public abstract string RecordName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">The object that has the compiled filed.</param>
        /// <param name="reader"></param>
        public abstract void Load(object obj, CtpDocumentElement reader);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">The object that has the compiled filed.</param>
        /// <param name="writer"></param>
        public abstract void Save(object obj, CtpDocumentWriter writer);

        private static readonly MethodInfo Method2 = typeof(FieldSerialization).GetMethod("CreateFieldSerializationInternal", BindingFlags.Static | BindingFlags.NonPublic);

        public static FieldSerialization CreateFieldOptions(MemberInfo member, Type targetType, DocumentFieldAttribute autoLoad)
        {
            var genericMethod = Method2.MakeGenericMethod(targetType);
            return (FieldSerialization)genericMethod.Invoke(null, new object[] { member, autoLoad });
        }

        // ReSharper disable once UnusedMember.Local
        private static FieldSerialization CreateFieldSerializationInternal<TFieldType>(MemberInfo member, DocumentFieldAttribute autoLoad)
        {
            return new FieldSerialization<TFieldType>(member, autoLoad);
        }
    }

    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal class FieldSerialization<T>
        : FieldSerialization
    {
        private string m_recordName;
        private TypeSerializationMethodBase<T> m_method;
        private Func<object, T> m_read;
        private Action<object, T> m_write;

        public FieldSerialization(MemberInfo member, DocumentFieldAttribute autoLoad)
        {
            m_recordName = autoLoad.RecordName ?? member.Name;

            if (member is PropertyInfo)
            {
                m_read = ((PropertyInfo)member).CompileGetter<T>();
                m_write = ((PropertyInfo)member).CompileSetter<T>();
            }
            else if (member is FieldInfo)
            {
                m_read = ((FieldInfo)member).CompileGetter<T>();
                m_write = ((FieldInfo)member).CompileSetter<T>();
            }
            else
            {
                throw new NotSupportedException();
            }
            m_method = DocumentSerializationHelper<T>.Serialization;

        }

        public override string RecordName => m_recordName;

        public override void Load(object obj, CtpDocumentElement reader)
        {
            T item;
            if (m_method.IsValueRecord)
            {
                item = m_method.Load(reader.GetValue(RecordName));
            }
            else
            {
                item = m_method.Load(reader.GetElement(RecordName));
            }
            m_write(obj, item);
        }

        public override void Save(object obj, CtpDocumentWriter writer)
        {
            var item = m_read(obj);
            if (m_method.IsValueRecord)
            {
                writer.WriteValue(RecordName, m_method.Save(item));
            }
            else
            {
                using (writer.StartElement(RecordName))
                {
                    m_method.Save(item, writer);
                }
            }
        }
    }
}