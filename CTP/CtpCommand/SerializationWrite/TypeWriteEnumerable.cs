using System;
using System.Collections.Generic;
using System.Linq;

namespace CTP.SerializationWrite
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class TypeWriteEnumerable<TEnum, T>
        : TypeWriteMethodBase<TEnum>
        where TEnum : IEnumerable<T>
    {
        private TypeWriteMethodBase<T> m_serializeT;
        private string m_recordName;

        public TypeWriteEnumerable(string recordName)
        {
            m_recordName = recordName;
            m_serializeT = TypeWrite.Create<T>("Item");
        }

        public override void Save(TEnum obj, CtpObjectWriter writer)
        {
            if (obj == null)
                return;

            writer.Write(obj.Count());
            foreach (var item in obj)
            {
                m_serializeT.Save(item, writer);
            }

        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.DefineArray(m_recordName);
            m_serializeT.WriteSchema(schema);
        }

    }
}