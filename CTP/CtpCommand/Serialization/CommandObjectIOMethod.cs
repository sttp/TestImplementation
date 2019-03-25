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

        private List<Action<object>> BeforeLoad;
        private List<Action<object>> AfterLoad;
        private List<Action<object>> BeforeSave;
        private List<Action<object>> AfterSave;
        private List<Action<object, string, CtpObject>> MissingValue;
        private List<Action<object, string>> MissingElement;
        private bool m_isRootElement;

        public CommandObjectIOMethod(ConstructorInfo c, string recordName, bool isRootElement)
        {
            m_isRootElement = isRootElement;
            m_recordName = recordName;
            var type = typeof(T);
            m_constructor = c.Compile<T>();

            var records = new List<FieldIO>();
            HashSet<string> ids = new HashSet<string>();
            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                AddIfValid(ids, records, member);
                if (member is MethodInfo method)
                {
                    object[] attributes = member.GetCustomAttributes(true);
                    foreach (var attribute in attributes.OfType<CommandEventAttribute>())
                    {
                        switch (attribute.Events)
                        {
                            case CommandEvents.BeforeLoad:
                                TryAdd(method, ref BeforeLoad);
                                break;
                            case CommandEvents.AfterLoad:
                                TryAdd(method, ref AfterLoad);
                                break;
                            case CommandEvents.BeforeSave:
                                TryAdd(method, ref BeforeSave);
                                break;
                            case CommandEvents.AfterSave:
                                TryAdd(method, ref AfterSave);
                                break;
                            case CommandEvents.MissingValue:
                                TryAdd(method, ref MissingValue);
                                break;
                            case CommandEvents.MissingElement:
                                TryAdd(method, ref MissingElement);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }


                    }
                }
            }

            m_records = records.ToArray();
        }

        private void TryAdd(MethodInfo method, ref List<Action<object>> list)
        {
            if (method.ReturnType != typeof(void))
                throw new Exception("Method should not have a return type");
            var param = method.GetParameters();
            if (param.Length != 0)
                throw new Exception("Method should have 0 parameters");
            if (list == null)
                list = new List<Action<object>>();
            list.Add(method.CreateAction());
        }
        private void TryAdd(MethodInfo method, ref List<Action<object, string>> list)
        {

            if (method.ReturnType != typeof(void))
                throw new Exception("Method should not have a return type");
            var param = method.GetParameters();
            if (param.Length != 1)
                throw new Exception("Method should have 1 parameter");

            if (param[0].ParameterType != typeof(string))
                throw new Exception("Method's parameter should be of type String");

            if (list == null)
                list = new List<Action<object, string>>();
            list.Add(method.CreateAction<string>());
        }

        private void TryAdd(MethodInfo method, ref List<Action<object, string, CtpObject>> list)
        {

            if (method.ReturnType != typeof(void))
                throw new Exception("Method should not have a return type");
            var param = method.GetParameters();
            if (param.Length != 2)
                throw new Exception("Method should have 2 parameter");

            if (param[0].ParameterType != typeof(string))
                throw new Exception("Method's parameter should be of type String");
            if (param[1].ParameterType != typeof(CtpObject))
                throw new Exception("Method's parameter should be of type CtpObject");

            if (list == null)
                list = new List<Action<object, string, CtpObject>>();
            list.Add(method.CreateAction<string, CtpObject>());
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
                if (m_isRootElement)
                    throw new Exception("Root element cannot be null");
                writer.Write(false);
            }
            else
            {
                if (BeforeSave != null)
                    foreach (var item in BeforeSave)
                        item(obj);
                if (!m_isRootElement)
                    writer.Write(true);
                foreach (var item in m_records)
                {
                    item.Save(obj, writer);
                }
                if (AfterSave != null)
                    foreach (var item in AfterSave)
                        item(obj);
            }
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.StartElement(m_recordName);
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
                if (m_isRootElement)
                    throw new Exception("Root element cannot be null");
                reader.Read();
                return default(T);
            }

            var rv = m_constructor();
            if (BeforeLoad != null)
                foreach (var item in BeforeLoad)
                    item(rv);
            FieldIO read;
            int id;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.StartArray:
                        if (m_recordsLookup.TryGetValue(reader.ElementName, out id))
                        {
                            read = m_records[id];
                            read.Load(rv, reader);
                        }
                        else
                        {
                            if (MissingElement != null)
                                foreach (var item in MissingElement)
                                    item(rv, reader.ElementName);
                            else
                                throw new Exception("Missing an element");
                            reader.SkipElement();
                        }
                        break;
                    case CommandSchemaSymbol.Value:
                        if (m_recordsLookup.TryGetValue(reader.ValueName, out id))
                        {
                            read = m_records[id];
                            read.Load(rv, reader);
                        }
                        else
                        {
                            if (MissingValue != null)
                                foreach (var item in MissingValue)
                                    item(rv, reader.ValueName, reader.Value);
                            else
                                throw new Exception("Missing a value");
                        }
                        break;
                    case CommandSchemaSymbol.EndElement:
                        if (AfterLoad != null)
                            foreach (var item in AfterLoad)
                                item(rv);
                        return rv;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new Exception("Error loading.");

            //rv2?.AfterLoad();
            return rv;
        }
    }
}