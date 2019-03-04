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
        private class FieldWrite2<T>
            : FieldWrite
        {
            private TypeWriteMethodBase<T> m_method;
            private Func<object, T> m_get;

            public FieldWrite2(Func<object, T> get, TypeWriteMethodBase<T> method)
            {
                m_get = get;
                m_method = method;
            }

            public override void Save(object obj, CtpObjectWriter writer)
            {
                var item = m_get(obj);
                if (item == null)
                    return;
                m_method.Save(item, writer);
            }

            public override void WriteSchema(CommandSchemaWriter schema)
            {
                m_method.WriteSchema(schema);
            }
        }

        /// <summary>
        /// Gets the corresponding field/property from the <see cref="obj"/> and writes the value to the <see cref="writer"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="writer"></param>
        public abstract void Save(object obj, CtpObjectWriter writer);
        public abstract void WriteSchema(CommandSchemaWriter schema);


        #region [ Static ]

        private static readonly MethodInfo Method = typeof(FieldWrite).GetMethod("CreateFieldWriteGeneric", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// Uses reflection to compile a way to read/write fields/properties without boxing. 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="member">The field/property of the <see cref="targetType"/> that will be serialized</param>
        /// <param name="recordName"></param>
        /// <returns></returns>
        public static FieldWrite Create(Type targetType, MemberInfo member, string recordName)
        {
            var genericMethod = Method.MakeGenericMethod(targetType);
            return (FieldWrite)genericMethod.Invoke(null, new object[] { member, recordName });
        }

        // ReSharper disable once UnusedMember.Local
        private static FieldWrite CreateFieldWriteGeneric<T>(MemberInfo member, string recordName)
        {
            Func<object, T> get;
            if (member is PropertyInfo info)
                get = info.CompileGetter<T>();
            else if (member is FieldInfo fieldInfo)
                get = fieldInfo.CompileGetter<T>();
            else
                throw new NotSupportedException();
            var writer = TypeWrite.Create<T>(recordName);

            return new FieldWrite2<T>(get, writer);
        }

        #endregion
    }


}