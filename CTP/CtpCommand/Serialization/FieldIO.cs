using System;
using System.Reflection;
using GSF.Reflection;

namespace CTP.Serialization
{
    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal abstract class FieldIO
    {
        private class FieldWrite2<T>
            : FieldIO
        {
            private TypeIOMethodBase<T> m_io;
            private Func<object, T> m_get;
            private Action<object, T> m_set;

            public FieldWrite2(MemberInfo member, string recordName)
            {
                RecordName = recordName;
                if (member is PropertyInfo info)
                {
                    m_set = info.CompileSetter<T>();
                    m_get = info.CompileGetter<T>();
                }
                else if (member is FieldInfo fieldInfo)
                {
                    m_set = ((FieldInfo)member).CompileSetter<T>();
                    m_get = fieldInfo.CompileGetter<T>();
                }
                else
                {
                    throw new NotSupportedException();
                }

                m_io = TypeIO.Create<T>(recordName);
            }

            public override void Save(object obj, CtpObjectWriter writer)
            {
                var item = m_get(obj);
                if (item == null)
                    return;
                m_io.Save(item, writer);
            }

            public override void WriteSchema(CommandSchemaWriter schema)
            {
                m_io.WriteSchema(schema);
            }

            public override void Load(object obj, CtpCommandReader reader)
            {
                T item = m_io.Load(reader);
                m_set(obj, item);
            }
        }

        public string RecordName { get; protected set; }
        /// <summary>
        /// Gets the corresponding field/property from the <see cref="obj"/> and writes the value to the <see cref="writer"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="writer"></param>
        public abstract void Save(object obj, CtpObjectWriter writer);
        public abstract void WriteSchema(CommandSchemaWriter schema);
        public abstract void Load(object obj, CtpCommandReader reader);

        #region [ Static ]

        private static readonly MethodInfo Method = typeof(FieldIO).GetMethod("CreateFieldWriteGeneric", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Uses reflection to compile a way to read/write fields/properties without boxing. 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="member">The field/property of the <see cref="targetType"/> that will be serialized</param>
        /// <param name="recordName"></param>
        /// <returns></returns>
        public static FieldIO Create(Type targetType, MemberInfo member, string recordName)
        {
            var genericMethod = Method.MakeGenericMethod(targetType);
            return (FieldIO)genericMethod.Invoke(null, new object[] { member, recordName });
        }

        // ReSharper disable once UnusedMember.Local
        private static FieldIO CreateFieldWriteGeneric<T>(MemberInfo member, string recordName)
        {
            return new FieldWrite2<T>(member, recordName);
        }

        #endregion
    }


}