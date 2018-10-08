using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal class RuntimeSerializationMethod<T>
       : TypeSerializationMethodBase<T>
    {

        private readonly List<FieldSerialization> m_records = new List<FieldSerialization>();
        private readonly Func<T> m_constructor;

        public RuntimeSerializationMethod(ConstructorInfo c)
        {
            DocumentSerializationHelper<T>.Serialization = this; //This is required to fix circular reference issues.

            var type = typeof(T);
            m_constructor = c.Compile<T>();

            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                TryCreateFieldOptions(member);
            }

            //Test for collisions
            HashSet<string> ids = new HashSet<string>();
            foreach (var f in m_records)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load Names: {0} detected in class {1}.", f.RecordName, type.ToString()));
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
            DocumentFieldAttribute autoLoad = attributes.OfType<DocumentFieldAttribute>().FirstOrDefault();
            if (autoLoad != null)
            {
                m_records.Add(FieldSerialization.CreateFieldOptions(member, targetType, autoLoad));
            }
        }

        public override bool CanAcceptNulls => true;

        public override bool IsValueRecord => false;

        public override T Load(CtpDocumentElement reader)
        {
            if (reader == null)
                return default(T);

            var rv = m_constructor();
            foreach (var item in m_records)
            {
                item.Load(rv, reader);
            }
            return rv;
        }

        public override void Save(T obj, CtpDocumentWriter writer)
        {
            if (obj == null)
                return;

            foreach (var item in m_records)
            {
                item.Save(obj, writer);
            }
        }

    }
}