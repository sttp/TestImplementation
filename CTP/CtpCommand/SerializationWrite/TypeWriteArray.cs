using System;
using System.Collections.Generic;

namespace CTP.SerializationWrite
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TypeWriteArray<T>
        : TypeWriteMethodBase<T[]>
    {
        private TypeWriteMethodBase<T> m_serializeT;
        private string m_recordName;
        public TypeWriteArray(string recordName)
        {
            m_recordName = recordName;
            m_serializeT = TypeWrite.Create<T>("Item");
        }

        public override void Save(T[] obj, CtpObjectWriter writer)
        {
            writer.Write(obj.Length);
            for (int i = 0; i < obj.Length; i++)
            {
                m_serializeT.Save(obj[i], writer);
            }
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.DefineArray(m_recordName);
            m_serializeT.WriteSchema(schema);
        }
    }
}