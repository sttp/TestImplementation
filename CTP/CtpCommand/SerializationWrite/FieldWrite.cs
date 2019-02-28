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
        /// 
        /// </summary>
        /// <param name="obj">The object that has the compiled filed.</param>
        /// <param name="writer"></param>
        public abstract void Save(object obj, CtpObjectWriter writer);

        private static readonly MethodInfo Method1 = typeof(FieldWrite).GetMethod("CreateFieldSerializationInternal1", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo Method2 = typeof(FieldWrite).GetMethod("CreateFieldSerializationInternal2", BindingFlags.Static | BindingFlags.NonPublic);

        public static FieldWrite CreateFieldOptions(MemberInfo member, Type targetType, string recordName, CommandSchemaWriter schema)
        {
            //Since I have to do reflection anyway, by checking if inheritance exists, I can call GetStrongTyped instead of GetUnknownType.
            if (targetType.IsSubclassOf(typeof(CommandObject)))
            {
                var genericMethod = Method2.MakeGenericMethod(targetType);
                return (FieldWrite)genericMethod.Invoke(null, new object[] { member, schema, recordName });
            }
            else
            {
                var genericMethod = Method1.MakeGenericMethod(targetType);
                return (FieldWrite)genericMethod.Invoke(null, new object[] { member, schema, recordName });
            }
        }

        // ReSharper disable once UnusedMember.Local
        private static FieldWrite CreateFieldSerializationInternal1<TFieldType>(MemberInfo member, CommandSchemaWriter schema, string recordName)
        {
            var method = TypeWrite.GetUnknownType<TFieldType>(schema, recordName);
            return new FieldWrite<TFieldType>(method, member);
        }
        // ReSharper disable once UnusedMember.Local
        private static FieldWrite CreateFieldSerializationInternal2<TFieldType>(MemberInfo member, CommandSchemaWriter schema, string recordName)
           where TFieldType : CommandObject
        {
            var method = TypeWrite.GetStrongTyped<TFieldType>(schema, recordName);
            return new FieldWrite<TFieldType>(method, member);
        }
    }

    /// <summary>
    /// Responsible for creating runtime lambda expressions to assign fields/properties. This is done without boxing. 
    /// </summary>
    internal class FieldWrite<T>
        : FieldWrite
    {
        private TypeWriteMethodBase<T> m_method;
        private Func<object, T> m_read;

        public FieldWrite(TypeWriteMethodBase<T> method, MemberInfo member)
        {
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
            m_method = method;
        }

        public override void Save(object obj, CtpObjectWriter writer)
        {
            var item = m_read(obj);
            if (item == null)
                return;

            m_method.Save(item, writer);
        }
    }
}