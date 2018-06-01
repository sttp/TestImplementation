using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal class RuntimeSerializationMethod<T>
       : TypeSerializationMethodBase<T>
    {
        public override bool IsArrayType => false;
        private readonly Type m_type;
        private readonly List<FieldSerialization> m_records = new List<FieldSerialization>();
        private readonly Func<T> m_constructor;

        public RuntimeSerializationMethod(ConstructorInfo c)
        {
            m_type = typeof(T);
            m_constructor = c.Compile<T>();

            foreach (var member in m_type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                TryCreateFieldOptions(member);
            }

            //Test for collisions
            HashSet<string> ids = new HashSet<string>();
            foreach (var f in m_records)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load IDs: {0} detected in class {1}.", f.RecordName, m_type.ToString()));
            }
        }

        private void TryCreateFieldOptions(MemberInfo member)
        {
            Type targetType;

            if (member is FieldInfo)
                targetType = ((FieldInfo)member).FieldType;
            else if (member is PropertyInfo)
                targetType = ((PropertyInfo)member).PropertyType;
            else
                return;

            object[] attributes = member.GetCustomAttributes(true);
            CtpSerializeFieldAttribute autoLoad = attributes.OfType<CtpSerializeFieldAttribute>().FirstOrDefault();
            if (autoLoad != null)
            {
                m_records.Add(FieldSerialization.CreateFieldOptions(member, targetType, autoLoad));
            }
        }

        public override void InitializeSerializationMethod()
        {
            foreach (var property in m_records)
            {
                property.InitializeSerializationMethod();
            }
        }

        public override bool IsValueType => false;

        public override CtpObject Save(T obj)
        {
            throw new NotSupportedException();
        }

        public override T Load(CtpObject reader)
        {
            throw new NotSupportedException();
        }

        public override T Load(CtpDocumentElement reader)
        {
            var rv = m_constructor();
            foreach (var item in m_records)
            {
                item.Load(rv, reader);
            }
            return rv;
        }

        public override void Save(T obj, CtpDocumentWriter writer)
        {
            foreach (var item in m_records)
            {
                item.Save(obj, writer);
            }
        }

    }
}