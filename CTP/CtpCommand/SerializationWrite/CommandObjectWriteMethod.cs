using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CTP.SerializationWrite
{
    internal class CommandObjectWriteMethod<T>
       : TypeWriteMethodBase<T>
    {
        private readonly FieldWrite[] m_records;

        public CommandObjectWriteMethod(CommandSchemaWriter schema, string recordName)
        {
            var type = typeof(T);
            var records = new List<FieldWrite>();
            var items = new List<Tuple<MemberInfo, string, Type>>();

            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (TryValidate(member, out var item))
                {
                    items.Add(item);
                }
            }

            //Test for collisions
            HashSet<string> ids = new HashSet<string>();
            foreach (var f in items)
            {
                if (!ids.Add(f.Item2))
                    throw new Exception(string.Format("Duplicate Load Names: {0} detected in class {1}.", f.Item2, type.ToString()));
            }

            schema.DefineElement(recordName, items.Count);
            foreach (var item in items)
            {
                records.Add(FieldWrite.CreateFieldOptions(item.Item1, item.Item3, item.Item2, schema));
            }

            m_records = records.ToArray();
        }

        private bool TryValidate(MemberInfo member, out Tuple<MemberInfo, string, Type> item)
        {
            item = null;
            Type targetType;

            if (member is FieldInfo)
                targetType = ((FieldInfo)member).FieldType;
            else if (member is PropertyInfo)
                targetType = ((PropertyInfo)member).PropertyType;
            else
                return false;

            object[] attributes = member.GetCustomAttributes(true);
            CommandFieldAttribute attribute = attributes.OfType<CommandFieldAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                var recordName = attribute.RecordName ?? member.Name;
                item = Tuple.Create(member, recordName, targetType);
                return true;
            }

            return false;
        }

        public override void Save(T obj, CtpCommandWriter writer)
        {
            //Root elements have a record name == null. These do not need to start an element.
            foreach (var item in m_records)
            {
                item.Save(obj, writer);
            }
        }

    }
}