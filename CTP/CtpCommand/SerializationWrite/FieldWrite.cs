using System;
using System.Reflection;
using GSF.Reflection;

namespace CTP.SerializationWrite
{
    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal abstract class FieldWrite
    {
        /// <summary>
        /// The name of the field/element to use in CtpDocument.
        /// </summary>
        public abstract CtpCommandKeyword RecordName { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">The object that has the compiled filed.</param>
        /// <param name="writer"></param>
        public abstract void Save(CommandObject obj, CtpCommandWriter writer);

        private static readonly MethodInfo Method2 = typeof(FieldWrite).GetMethod("CreateFieldSerializationInternal", BindingFlags.Static | BindingFlags.NonPublic);

        public static FieldWrite CreateFieldOptions(MemberInfo member, Type targetType, CommandFieldAttribute autoLoad, SerializationSchema schema)
        {
            var genericMethod = Method2.MakeGenericMethod(targetType);
            return (FieldWrite)genericMethod.Invoke(null, new object[] { member, autoLoad, schema });
        }

        // ReSharper disable once UnusedMember.Local
        private static FieldWrite CreateFieldSerializationInternal<TFieldType>(MemberInfo member, CommandFieldAttribute autoLoad, SerializationSchema schema)
        {
            return new FieldWrite<TFieldType>(member, autoLoad, schema);
        }
    }

    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal class FieldWrite<T>
        : FieldWrite
    {
        private CtpCommandKeyword m_recordName;
        private TypeWriteMethodBase<T> m_method;
        private Func<object, T> m_read;

        public FieldWrite(MemberInfo member, CommandFieldAttribute autoLoad, SerializationSchema schema)
        {
            m_recordName = CtpCommandKeyword.Create(autoLoad.RecordName ?? member.Name);

            if (member is PropertyInfo)
            {
                m_read = ((PropertyInfo)member).CompileGetter<T>();
            }
            else if (member is FieldInfo)
            {
                m_read = ((FieldInfo)member).CompileGetter<T>();
            }
            else
            {
                throw new NotSupportedException();
            }
            m_method = TypeWrite.Get<T>(schema, schema.WriteName(m_recordName));
        }

        public override CtpCommandKeyword RecordName => m_recordName;

        public override void Save(CommandObject obj, CtpCommandWriter writer)
        {
            var item = m_read(obj);
            if (item == null)
                return;

            m_method.Save(item, writer);
        }
    }
}