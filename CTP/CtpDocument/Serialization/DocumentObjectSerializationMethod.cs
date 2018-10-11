using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal class DocumentObjectSerializationMethod
    {
        private static readonly MethodInfo Method2 = typeof(DocumentObjectSerializationMethod).GetMethod("Create2", BindingFlags.Static | BindingFlags.NonPublic);

        public static TypeSerializationMethodBase<T> Create<T>(ConstructorInfo c)
        {
            var genericMethod = Method2.MakeGenericMethod(typeof(T));
            return (TypeSerializationMethodBase<T>)genericMethod.Invoke(null, new object[] { c });
        }

        // ReSharper disable once UnusedMember.Local
        private static TypeSerializationMethodBase<T> Create2<T>(ConstructorInfo c)
            where T : DocumentObject
        {
            return new DocumentObjectSerializationMethod<T>(c);
        }
    }

    internal class DocumentObjectSerializationMethod<T>
       : TypeSerializationMethodBase<T>
        where T : DocumentObject
    {
        private FieldSerialization[] m_records;

        private readonly RuntimeMapping m_recordsLookup = new RuntimeMapping();

        private readonly Func<T> m_constructor;

        public DocumentObjectSerializationMethod(ConstructorInfo c)
        {
            TypeSerialization<T>.Serialization = this; //This is required to fix circular reference issues.

            var type = typeof(T);
            m_constructor = c.Compile<T>();

            var records = new List<FieldSerialization>();
            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                TryCreateFieldOptions(member, records);
            }
            m_records = records.ToArray();


            //Test for collisions
            HashSet<CtpDocumentName> ids = new HashSet<CtpDocumentName>();
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
            DocumentFieldAttribute attribute = attributes.OfType<DocumentFieldAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                var field = FieldSerialization.CreateFieldOptions(member, targetType, attribute);
                m_recordsLookup.Add(field.RecordName.RuntimeID, records.Count);
                records.Add(field);
            }


        }

        public override bool CanAcceptNulls => true;

        public override T Load(CtpDocumentReader reader)
        {
            var rv = m_constructor();
            rv.BeforeLoad();
            FieldSerialization serialization;
            int id;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpDocumentNodeType.StartElement:
                        if (m_recordsLookup.TryGetValue(reader.ElementName.RuntimeID, out id))
                        {
                            serialization = m_records[id];
                            serialization.Load(rv, reader);
                        }
                        else
                        {
                            rv.MissingElement(reader.ElementName.Value);
                            reader.SkipElement();
                        }
                        break;
                    case CtpDocumentNodeType.Value:
                        if (m_recordsLookup.TryGetValue(reader.ValueName.RuntimeID, out id))
                        {
                            serialization = m_records[id];
                            serialization.Load(rv, reader);
                        }
                        else
                        {
                            rv.MissingElement(reader.ElementName.Value);
                            reader.SkipElement();
                        }
                        break;
                    case CtpDocumentNodeType.EndElement:
                        rv.AfterLoad();
                        return rv;
                    case CtpDocumentNodeType.EndOfDocument:
                    case CtpDocumentNodeType.StartOfDocument:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            rv.AfterLoad();
            return rv;

        }

        public override void Save(T obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            if (recordName == null)
            {
                foreach (var item in m_records)
                {
                    item.Save(obj, writer);
                }
            }
            else
            {
                using (writer.StartElement(recordName))
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