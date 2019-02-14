using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Reflection;

namespace CTP.SerializationWrite
{
    internal class CommandObjectWriteMethod
    {
        private static readonly MethodInfo Method2 = typeof(CommandObjectWriteMethod).GetMethod("Create2", BindingFlags.Static | BindingFlags.NonPublic);

        public static TypeWriteMethodBase<T> Create<T>(bool isRoot, SerializationSchema schema, int recordName)
        {
            var genericMethod = Method2.MakeGenericMethod(typeof(T));
            return (TypeWriteMethodBase<T>)genericMethod.Invoke(null, new object[] { isRoot, schema, recordName });
        }

        // ReSharper disable once UnusedMember.Local
        private static TypeWriteMethodBase<T> Create2<T>(bool isRoot, SerializationSchema schema, int recordName)
            where T : CommandObject
        {
            return new CommandObjectWriteMethod<T>(isRoot, schema, recordName);
        }
    }

    internal class CommandObjectWriteMethod<T>
       : TypeWriteMethodBase<T>
        where T : CommandObject
    {
        private readonly FieldWrite[] m_records;

        private readonly RuntimeMapping m_recordsLookup = new RuntimeMapping();

        private bool m_isRoot;

        private int m_recordName;

        public CommandObjectWriteMethod(bool isRoot, SerializationSchema schema, int recordName)
        {
            m_recordName = recordName;
            m_isRoot = isRoot;
            var type = typeof(T);

            var records = new List<FieldWrite>();
            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                TryCreateFieldOptions(member, records, schema);
            }
            m_records = records.ToArray();

            //Test for collisions
            HashSet<CtpCommandKeyword> ids = new HashSet<CtpCommandKeyword>();
            foreach (var f in m_records)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load Names: {0} detected in class {1}.", f.RecordName, type.ToString()));
            }
        }

        private void TryCreateFieldOptions(MemberInfo member, List<FieldWrite> records, SerializationSchema schema)
        {
            Type targetType;

            if (member is FieldInfo)
                targetType = ((FieldInfo)member).FieldType;
            else if (member is PropertyInfo)
                targetType = ((PropertyInfo)member).PropertyType;
            else
                return;

            object[] attributes = member.GetCustomAttributes(true);
            CommandFieldAttribute attribute = attributes.OfType<CommandFieldAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                var field = FieldWrite.CreateFieldOptions(member, targetType, attribute, schema);
                m_recordsLookup.Add(field.RecordName.RuntimeID, records.Count);
                records.Add(field);
            }
        }

        public override void Save(T obj, CtpCommandWriter writer)
        {
            //Root elements have a record name == null. These do not need to start an element.
            if (m_isRoot)
            {
                foreach (var item in m_records)
                {
                    item.Save(obj, writer);
                }
            }
            else
            {
                using (writer.StartElement(m_recordName, false))
                {
                    foreach (var item in m_records)
                    {
                        item.Save(obj, writer);
                    }
                }
            }

        }

    }
}