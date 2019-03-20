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
        where TEnum : ICollection<T>
    {
        private TypeIOMethodBase<T> m_serializeT;
        private Func<List<T>, TEnum> m_castToType;
        private string m_recordName;

        public EnumerableIOType(string recordName, Func<List<T>, TEnum> castToType)
        {
            m_recordName = recordName;
            m_castToType = castToType;
            m_serializeT = TypeIO.Create<T>("Item");
        }

        public override void Save(TEnum obj, CtpObjectWriter writer)
        {
            if (obj == null)
            {
                writer.Write(CtpObject.Null);
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
            schema.DefineArray(m_recordName);
            m_serializeT.WriteSchema(schema);
            schema.EndArray();
        }

        public override TEnum Load(CtpCommandReader reader)
        {
            if (!reader.IsArray)
                throw new Exception("Expecting An Array Type");
            if (reader.IsElementOrArrayNull)
            {
                return default(TEnum);
            }

            List<T> items = new List<T>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.StartArray:
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CommandSchemaSymbol.Value:
                        items.Add(m_serializeT.Load(reader));
                        break;
                    case CommandSchemaSymbol.EndElement:
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