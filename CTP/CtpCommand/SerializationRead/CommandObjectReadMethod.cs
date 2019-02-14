using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Reflection;

namespace CTP.SerializationRead
{
    internal class CommandObjectReadMethod
    {
        private static readonly MethodInfo Method2 = typeof(CommandObjectReadMethod).GetMethod("Create2", BindingFlags.Static | BindingFlags.NonPublic);

        public static TypeReadMethodBase<T> Create<T>(ConstructorInfo c)
        {
            var genericMethod = Method2.MakeGenericMethod(typeof(T));
            return (TypeReadMethodBase<T>)genericMethod.Invoke(null, new object[] { c });
        }

        // ReSharper disable once UnusedMember.Local
        private static TypeReadMethodBase<T> Create2<T>(ConstructorInfo c)
            where T : CommandObject
        {
            return new CommandObjectReadMethod<T>(c);
        }
    }

    internal class CommandObjectReadMethod<T>
       : TypeReadMethodBase<T>
        where T : CommandObject
    {
        private readonly FieldRead[] m_records;

        private readonly RuntimeMapping m_recordsLookup = new RuntimeMapping();

        private readonly Func<T> m_constructor;

        public CommandObjectReadMethod(ConstructorInfo c)
        {
            TypeRead<T>.Set(this); //This is required to fix circular reference issues.

            var type = typeof(T);
            m_constructor = c.Compile<T>();

            var records = new List<FieldRead>();
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

        private void TryCreateFieldOptions(MemberInfo member, List<FieldRead> records)
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
                var field = FieldRead.CreateFieldOptions(member, targetType, attribute);
                m_recordsLookup.Add(field.RecordName.RuntimeID, records.Count);
                records.Add(field);
            }
        }

        public override T Load(CtpCommandReader reader)
        {
            var rv = m_constructor();
            rv.OnBeforeLoad();
            FieldRead read;
            int id;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        if (m_recordsLookup.TryGetValue(reader.ElementName.RuntimeID, out id))
                        {
                            read = m_records[id];
                            read.Load(rv, reader);
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
                            read = m_records[id];
                            read.Load(rv, reader);
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

       

    }
}