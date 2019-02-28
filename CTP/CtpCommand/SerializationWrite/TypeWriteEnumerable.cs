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

        public TypeWriteEnumerable(CommandSchemaWriter schema, string recordName)
        {
            schema.DefineArray(recordName);
            m_serializeT = TypeWrite.GetUnknownType<T>(schema, "Item");
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

    }
}