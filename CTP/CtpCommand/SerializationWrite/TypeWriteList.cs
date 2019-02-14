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

        public TypeWriteList(SerializationSchema schema, int recordName)
        {
            m_recordName = recordName;
            m_serializeT = TypeWrite.Get<T>(schema, -1);
        }

        public override void Save(List<T> obj, CtpCommandWriter writer)
        {
            using (writer.StartElement(m_recordName, true))
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    m_serializeT.Save(obj[i], writer);
                }

            }
        }

    }
}