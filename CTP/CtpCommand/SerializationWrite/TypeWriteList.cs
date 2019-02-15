using System;
using System.Collections.Generic;

namespace CTP.SerializationWrite
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class TypeWriteList<T>
        : TypeWriteMethodBase<List<T>>
    {
        private TypeWriteMethodBase<T> m_serializeT;
        private int m_recordName;

        public TypeWriteList(CommandSchemaWriter schema, string recordName)
        {
            schema.DefineArray(recordName);
            m_serializeT = TypeWrite.Get<T>(schema, "Item");
        }

        public override void Save(List<T> obj, CtpCommandWriter writer)
        {
            writer.WriteArray(obj.Count);
            for (int i = 0; i < obj.Count; i++)
            {
                m_serializeT.Save(obj[i], writer);
            }
        }

    }
}