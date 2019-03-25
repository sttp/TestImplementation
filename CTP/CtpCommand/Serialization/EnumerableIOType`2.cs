using System;
using System.Collections.Generic;
using System.Linq;

namespace CTP.Serialization
{
    /// <summary>
    /// Can serialize an array type.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class EnumerableIOType<TEnum, T>
        : TypeIOMethodBase<TEnum>
        where TEnum : class, ICollection<T>
    {
        private TypeIOMethodBase<T> m_serializeT;
        private Func<T[], TEnum> m_castToType;
        private string m_recordName;

        public EnumerableIOType(string recordName, Func<T[], TEnum> castToType)
        {
            m_recordName = recordName;
            m_castToType = castToType;
            m_serializeT = TypeIO.Create<T>("Item", false);
        }

        public override void Save(TEnum obj, CtpObjectWriter writer)
        {
            if (obj == null)
            {
                writer.Write(-1);
            }
            else
            {
                writer.Write(obj.Count());
                foreach (var item in obj)
                {
                    m_serializeT.Save(item, writer);
                }
            }
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.StartArray(m_recordName);
            m_serializeT.WriteSchema(schema);
            schema.EndArray();
        }

        public override TEnum Load(CtpCommandReader reader)
        {
            if (!reader.IsArray)
                throw new Exception("Expecting An Array Type");
            if (reader.IsElementOrArrayNull)
            {
                reader.Read();
                return null;
            }

            T[] items = new T[reader.ArrayCount];

            int x = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.StartArray:
                    case CommandSchemaSymbol.Value:
                        items[x] = m_serializeT.Load(reader);
                        x++;
                        break;
                    case CommandSchemaSymbol.EndArray:
                        return m_castToType(items);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new ArgumentOutOfRangeException();
        }
    }
}