using System;
using System.Collections.Generic;

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

        private int m_recordName;

        public TypeWriteEnumerable(SerializationSchema schema, int recordName)
        {
            m_recordName = recordName;
            m_serializeT = TypeWrite.Get<T>(schema, -1);
        }

        public override void Save(TEnum obj, CtpCommandWriter writer)
        {
            if (obj == null)
                return;

            using (writer.StartElement(m_recordName, true))
            {
                foreach (var item in obj)
                {
                    m_serializeT.Save(item, writer);
                }

            }
        }

    }
}