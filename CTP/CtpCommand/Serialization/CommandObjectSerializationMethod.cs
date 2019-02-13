using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal class CommandObjectSerializationMethod
    {
        private static readonly MethodInfo Method2 = typeof(CommandObjectSerializationMethod).GetMethod("Create2", BindingFlags.Static | BindingFlags.NonPublic);

        public static TypeSerializationMethodBase<T> Create<T>(ConstructorInfo c)
        {
            var genericMethod = Method2.MakeGenericMethod(typeof(T));
            return (TypeSerializationMethodBase<T>)genericMethod.Invoke(null, new object[] { c });
        }

        // ReSharper disable once UnusedMember.Local
        private static TypeSerializationMethodBase<T> Create2<T>(ConstructorInfo c)
            where T : CommandObject
        {
            return new CommandObjectSerializationMethod<T>(c);
        }
    }

    internal class CommandObjectSerializationMethod<T>
       : TypeSerializationMethodBase<T>
        where T : CommandObject
    {
        private readonly FieldSerialization[] m_records;

        private readonly RuntimeMapping m_recordsLookup = new RuntimeMapping();

        private readonly Func<T> m_constructor;

        public CommandObjectSerializationMethod(ConstructorInfo c)
        {
            TypeSerialization<T>.Set(this); //This is required to fix circular reference issues.

            var type = typeof(T);
            m_constructor = c.Compile<T>();

            var records = new List<FieldSerialization>();
            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                TryCreateFieldOptions(member, records);
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

        private void TryCreateFieldOptions(MemberInfo member, List<FieldSerialization> records)
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
                var field = FieldSerialization.CreateFieldOptions(member, targetType, attribute);
                m_recordsLookup.Add(field.RecordName.RuntimeID, records.Count);
                records.Add(field);
            }
        }

        public override T Load(CtpCommandReader reader)
        {
            var rv = m_constructor();
            rv.OnBeforeLoad();
            FieldSerialization serialization;
            int id;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        if (m_recordsLookup.TryGetValue(reader.ElementName.RuntimeID, out id))
                        {
                            serialization = m_records[id];
                            serialization.Load(rv, reader);
                        }
                        else
                        {
                            rv.OnMissingElement(reader.ElementName.Value);
                            reader.SkipElement();
                        }
                        break;
                    case CtpCommandNodeType.Value:
                        if (m_recordsLookup.TryGetValue(reader.ValueName.RuntimeID, out id))
                        {
                            serialization = m_records[id];
                            serialization.Load(rv, reader);
                        }
                        else
                        {
                            rv.OnMissingValue(reader.ValueName.Value, reader.Value);
                            reader.SkipElement();
                        }
                        break;
                    case CtpCommandNodeType.EndElement:
                        rv.OnAfterLoad();
                        return rv;
                    case CtpCommandNodeType.EndOfCommand:
                    case CtpCommandNodeType.StartOfCommand:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            rv.OnAfterLoad();
            return rv;
        }

        public override void Save(T obj, CtpCommandWriter writer, int recordName)
        {
            //Root elements have a record name == null. These do not need to start an element.
            if ((object)recordName == null)
            {
                foreach (var item in m_records)
                {
                    item.Save(obj, writer, null);
                }
            }
            else
            {
                using (writer.StartElement(recordName, false))
                {
                    foreach (var item in m_records)
                    {
                        item.Save(obj, writer, null);
                    }
                }
            }

        }

    }
}