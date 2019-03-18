using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CTP.Serialization;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal class CommandObjectIOMethod<T> 
        : TypeIOMethodBase<T>
    {
        private string m_recordName;
        private readonly FieldIO[] m_records;
        private readonly Dictionary<string, int> m_recordsLookup = new Dictionary<string, int>();
        private readonly Func<T> m_constructor;

        public CommandObjectIOMethod(ConstructorInfo c, string recordName)
        {
            m_recordName = recordName;
            var type = typeof(T);
            m_constructor = c.Compile<T>();

            var records = new List<FieldIO>();
            HashSet<string> ids = new HashSet<string>();
            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                AddIfValid(ids, records, member);
            }

            m_records = records.ToArray();
        }

        private void AddIfValid(HashSet<string> duplicateNameChecker, List<FieldIO> records, MemberInfo member)
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

                m_recordsLookup.Add(recordName, records.Count);
                records.Add(FieldIO.Create(targetType, member, recordName));
            }
        }

        public override void Save(T obj, CtpObjectWriter writer)
        {
            if (obj == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                foreach (var item in m_records)
                {
                    item.Save(obj, writer);
                }
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

        public override T Load(CtpCommandReader reader)
        {
            if (reader.IsElementOrArrayNull)
            {
                return default(T);
            }

            var rv = m_constructor();
            ICommandObjectOptionalMethods rv2 = rv as ICommandObjectOptionalMethods;
            rv2?.BeforeLoad();
            FieldIO read;
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