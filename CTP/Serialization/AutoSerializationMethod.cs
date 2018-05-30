using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
                var attr = type.GetCustomAttributes(true).OfType<CtpSerializable>().FirstOrDefault();
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
        private static TypeSerializationMethodBase AutoSerializationMethod<T>(CtpSerializable attr)
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
        private readonly ConstructorInfo m_constructor;

        public AutoSerializationMethod(CtpSerializable attr)
        {
            m_type = typeof(T);

            m_constructor = m_type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            if (m_constructor == null)
                throw new Exception("class does not contain a default constructor");

            foreach (var member in m_type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                var f = new PropertyOptions(member);
                if (f.IsValid)
                {
                    m_properties.Add(f);
                }
            }

            //Test for collisions
            HashSet<string> ids = new HashSet<string>();
            foreach (var f in m_properties)
            {
                if (!ids.Add(f.RecordName))
                    throw new Exception(string.Format("Duplicate Load IDs: {0} detected in class {1}.", f.RecordName, m_type.ToString()));
            }
        }

        public override void InitializeSerializationMethod()
        {
            foreach (var property in m_properties)
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
            var rv = (T)m_constructor.Invoke(new object[0]);
            foreach (var item in m_properties)
            {
                item.Load(this, reader);
            }
            return rv;
        }

        public override void Save(T obj, CtpDocumentWriter writer)
        {
            foreach (var item in m_properties)
            {
                item.Save(this, writer);
            }
        }

    }
}