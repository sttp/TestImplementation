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

        public static TypeWriteMethodBase<T> Create<T>(CommandSchemaWriter schema, string recordName)
        {
            var genericMethod = Method2.MakeGenericMethod(typeof(T));
            return (TypeWriteMethodBase<T>)genericMethod.Invoke(null, new object[] { schema, recordName });
        }

        // ReSharper disable once UnusedMember.Local
        private static TypeWriteMethodBase<T> Create2<T>(CommandSchemaWriter schema, string recordName)
            where T : CommandObject
        {
            return new CommandObjectWriteMethod<T>(schema, recordName);
        }
    }

    internal class CommandObjectWriteMethod<T>
       : TypeWriteMethodBase<T>
        where T : CommandObject
    {
        private readonly FieldWrite[] m_records;

        public CommandObjectWriteMethod(CommandSchemaWriter schema, string recordName)
        {
            var type = typeof(T);

            var records = new List<FieldWrite>();
            var items = new List<Tuple<MemberInfo, CommandFieldAttribute, Type>>();

            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (TryValidate(member, out var item))
                {
                    items.Add(item);
                }
            }

            schema.DefineElement(recordName, items.Count);
            foreach (var item in items)
            {
                records.Add(FieldWrite.CreateFieldOptions(item.Item1,item.Item3, item.Item2, schema));
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

        private bool TryValidate(MemberInfo member, out Tuple<MemberInfo, CommandFieldAttribute, Type> item)
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
                item = Tuple.Create(member, attribute, targetType);
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