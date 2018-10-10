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
        private readonly List<FieldSerialization> m_records = new List<FieldSerialization>();
        private readonly Dictionary<CtpDocumentNames, FieldSerialization> m_recordsLookup = new Dictionary<CtpDocumentNames, FieldSerialization>();

        private readonly Func<T> m_constructor;

        public DocumentObjectSerializationMethod(ConstructorInfo c)
        {
            TypeSerialization<T>.Serialization = this; //This is required to fix circular reference issues.

            var type = typeof(T);
            m_constructor = c.Compile<T>();

            foreach (var member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                TryCreateFieldOptions(member);
            }

            //Test for collisions
            HashSet<CtpDocumentNames> ids = new HashSet<CtpDocumentNames>();
            foreach (var f in m_records)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load Names: {0} detected in class {1}.", f.RecordName, type.ToString()));
            }
        }

        private void TryCreateFieldOptions(MemberInfo member)
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
                m_records.Add(field);
                m_recordsLookup.Add(field.RecordName, field);
            }
        }

        public override bool CanAcceptNulls => true;

        public override T Load(CtpDocumentReader reader)
        {
            var rv = m_constructor();
            rv.BeforeLoad();
            FieldSerialization serialization;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CtpDocumentNodeType.StartElement:
                        if (m_recordsLookup.TryGetValue(reader.ElementName, out serialization))
                        {
                            serialization.Load(rv, reader);
                        }
                        else
                        {
                            rv.MissingElement(reader.ElementName.Value);
                            reader.SkipElement();
                        }
                        break;
                    case CtpDocumentNodeType.Value:
                        if (m_recordsLookup.TryGetValue(reader.ValueName, out serialization))
                        {
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

        public override void Save(T obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            if (recordName == null)
            {
                foreach (var item in m_recordsLookup.Values)
                {
                    item.Save(obj, writer);
                }
            }
            else
            {
                using (writer.StartElement(recordName))
                {
                    foreach (var item in m_recordsLookup.Values)
                    {
                        item.Save(obj, writer);
                    }
                }
            }
           
        }

    }
}