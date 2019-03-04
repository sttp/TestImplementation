using System;
using System.Reflection;
using GSF.Reflection;

namespace CTP.SerializationRead
{
    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal abstract class FieldRead
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
        public abstract void Load(object obj, CtpCommandReader reader);

        private static readonly MethodInfo Method2 = typeof(FieldRead).GetMethod("CreateFieldSerializationInternal", BindingFlags.Static | BindingFlags.NonPublic);

        public static FieldRead CreateFieldOptions(MemberInfo member, Type targetType, CommandFieldAttribute autoLoad)
        {
            var genericMethod = Method2.MakeGenericMethod(targetType);
            return (FieldRead)genericMethod.Invoke(null, new object[] { member, autoLoad });
        }

        // ReSharper disable once UnusedMember.Local
        private static FieldRead CreateFieldSerializationInternal<TFieldType>(MemberInfo member, CommandFieldAttribute autoLoad)
        {
            return new FieldRead<TFieldType>(member, autoLoad);
        }
    }

    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal class FieldRead<T>
        : FieldRead
    {
        private string m_recordName;
        private TypeReadMethodBase<T> m_method;
        private Action<object, T> m_write;

        public FieldRead(MemberInfo member, CommandFieldAttribute autoLoad)
        {
            m_recordName = autoLoad.RecordName ?? member.Name;

            if (member is PropertyInfo)
            {
                m_write = ((PropertyInfo)member).CompileSetter<T>();
            }
            else if (member is FieldInfo)
            {
                m_write = ((FieldInfo)member).CompileSetter<T>();
            }
            else
            {
                throw new NotSupportedException();
            }
            m_method = TypeRead<T>.Get();
        }

        public override string RecordName => m_recordName;

        public override void Load(object obj, CtpCommandReader reader)
        {
            T item = m_method.Load(reader);
            m_write(obj, item);
        }

      
    }
}