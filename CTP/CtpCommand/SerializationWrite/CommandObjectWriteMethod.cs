using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CTP.SerializationWrite
{
    internal class CommandObjectWriteMethod<T>
       : TypeWriteMethodBase<T>
    {
        private string m_recordName;
        private readonly FieldWrite[] m_records;

        public CommandObjectWriteMethod(string recordName)
        {
            m_recordName = recordName;
            var type = typeof(T);
            var records = new List<FieldWrite>();
            HashSet<string> ids = new HashSet<string>();

            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                AddIfValid(ids, records, member);
            }
            m_records = records.ToArray();
        }

        private void AddIfValid(HashSet<string> duplicateNameChecker, List<FieldWrite> records, MemberInfo member)
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
                var recordName = attribute.RecordName ?? member.Name;

                if (!duplicateNameChecker.Add(recordName))
                    throw new Exception(string.Format("Duplicate Load Names: {0} detected in class {1}.", recordName, typeof(T).ToString()));

                records.Add(FieldWrite.Create(targetType, member, recordName));
            }
        }

        public override void Save(T obj, CtpObjectWriter writer)
        {
            foreach (var item in m_records)
            {
                item.Save(obj, writer);
            }
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.DefineElement(m_recordName);
            foreach (var member in m_records)
            {
                member.WriteSchema(schema);
            }
            schema.EndElement();
        }
    }
}