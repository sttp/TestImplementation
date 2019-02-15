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
        private int m_recordName;

        public TypeWriteArray(CommandSchemaWriter schema, string recordName)
        {
            schema.DefineArray(recordName);
            m_serializeT = TypeWrite.Get<T>(schema, "Item");
        }

        public override void Save(T[] obj, CtpCommandWriter writer)
        {
            writer.WriteArray(obj.Length);
            for (int i = 0; i < obj.Length; i++)
            {
                m_serializeT.Save(obj[i], writer);
            }
        }

    }
}