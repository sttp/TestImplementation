using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Reflection;

namespace CTP.SerializationRead
{
   internal class CommandObjectReadMethod<T>
       : TypeReadMethodBase<T>
    {
        private readonly FieldRead[] m_records;

        private readonly Dictionary<string, int> m_recordsLookup = new Dictionary<string, int>();

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
            HashSet<string> ids = new HashSet<string>();
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
                m_recordsLookup.Add(field.RecordName, records.Count);
                records.Add(field);
            }
        }

        public override T Load(CtpCommandReader reader)
        {
            var rv = m_constructor();
            ICommandObjectOptionalMethods rv2 = rv as ICommandObjectOptionalMethods;
            rv2?.BeforeLoad();
            FieldRead read;
            int id;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpCommandNodeType.StartElement:
                        if (m_recordsLookup.TryGetValue(reader.ElementName, out id))
                        {
                            read = m_records[id];
                            read.Load(rv, reader);
                        }
                        else
                        {
                            if (rv2 == null)
                                throw new Exception("Missing an element");
                            rv2.MissingElement(reader.ElementName);
                            reader.SkipElement();
                        }
                        break;
                    case CtpCommandNodeType.Value:
                        if (m_recordsLookup.TryGetValue(reader.ValueName, out id))
                        {
                            read = m_records[id];
                            read.Load(rv, reader);
                        }
                        else
                        {
                            if (rv2 == null)
                                throw new Exception("Missing a value");
                            rv2.MissingValue(reader.ValueName, reader.Value);
                            reader.SkipElement();
                        }
                        break;
                    case CtpCommandNodeType.EndElement:
                        rv2?.AfterLoad();
                        return rv;
                    case CtpCommandNodeType.EndOfCommand:
                    case CtpCommandNodeType.StartOfCommand:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            rv2?.AfterLoad();
            return rv;
        }



    }
}