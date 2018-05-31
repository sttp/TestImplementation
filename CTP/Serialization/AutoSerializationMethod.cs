using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GSF.Reflection;

namespace CTP.Serialization
{
    internal static class AutoInitialization
    {
        private static readonly MethodInfo Method = typeof(AutoInitialization).GetMethod("AutoSerializationMethod", BindingFlags.Static | BindingFlags.NonPublic);

        public static bool TryCreateMethod(Type type, out TypeSerializationMethodBase method)
        {
            method = null;
            if (type.IsClass)
            {
                var attr = type.GetCustomAttributes(true).OfType<CtpSerializableAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    var genericMethod = Method.MakeGenericMethod(type);
                    method = (TypeSerializationMethodBase)genericMethod.Invoke(null, new object[] { attr });
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.NoOptimization)] //This method is called via reflection.
        // ReSharper disable once UnusedMember.Local
        private static TypeSerializationMethodBase AutoSerializationMethod<T>(CtpSerializableAttribute attr)
            where T : class
        {
            return new AutoSerializationMethod<T>(attr);
        }

    }

    internal class AutoSerializationMethod<T>
       : TypeSerializationMethodBase<T>
    {
        public override bool IsArrayType => false;
        private readonly Type m_type;
        private readonly List<PropertyOptions> m_properties = new List<PropertyOptions>();
        private readonly List<FieldOptions> m_fields = new List<FieldOptions>();
        private readonly Func<T> m_constructor;
        public readonly CtpSerializableAttribute Attr;

        public AutoSerializationMethod(CtpSerializableAttribute attr)
        {
            Attr = attr;
            m_type = typeof(T);

            var c = m_type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (c == null)
                throw new Exception("class does not contain a default constructor");
            m_constructor = c.Compile<T>();


            foreach (var member in m_type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                var f = new PropertyOptions(member);
                if (f.IsValid)
                {
                    m_properties.Add(f);
                }
            }
            foreach (var member in m_type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                var f = new FieldOptions(member);
                if (f.IsValid)
                {
                    m_fields.Add(f);
                }
            }

            //Test for collisions
            HashSet<string> ids = new HashSet<string>();
            foreach (var f in m_properties)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load IDs: {0} detected in class {1}.", f.RecordName, m_type.ToString()));
            }
            foreach (var f in m_fields)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load IDs: {0} detected in class {1}.", f.RecordName, m_type.ToString()));
            }
        }

        public override CtpDocument SaveObject(object obj)
        {
            var wr = new CtpDocumentWriter(Attr.RootCommandName);
            SaveObject(obj, wr);
            return wr.ToCtpDocument();
        }

        public override void InitializeSerializationMethod()
        {
            foreach (var property in m_properties)
            {
                property.InitializeSerializationMethod();
            }
            foreach (var property in m_fields)
            {
                property.InitializeSerializationMethod();
            }
        }

        public override bool IsValueType => false;

        public override CtpObject Save(T obj)
        {
            throw new NotSupportedException();
        }

        public override T Load(CtpObject reader)
        {
            throw new NotSupportedException();
        }

        public override T Load(CtpDocumentElement reader)
        {
            var rv = m_constructor();
            foreach (var item in m_properties)
            {
                item.Load(rv, reader);
            }
            foreach (var item in m_fields)
            {
                item.Load(rv, reader);
            }
            return rv;
        }

        public override void Save(T obj, CtpDocumentWriter writer)
        {
            foreach (var item in m_properties)
            {
                item.Save(obj, writer);
            }
            foreach (var item in m_fields)
            {
                item.Save(obj, writer);
            }
        }

    }
}