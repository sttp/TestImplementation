using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.IO;

namespace CTP.Serialization
{
    internal class IDictionarySerialization<TDictionary, TKey, TValue>
            : TypeIOMethodBase<TDictionary>
        where TDictionary : class, IDictionary<TKey, TValue>
    {
        private TupleHelper<TKey, TValue> m_serializeT;
        private Func<Dictionary<TKey, TValue>, TDictionary> m_castToType;
        private string m_recordName;

        public IDictionarySerialization(string recordName, Func<Dictionary<TKey, TValue>, TDictionary> castToType)
        {
            m_recordName = recordName;
            m_castToType = castToType;
            m_serializeT = new TupleHelper<TKey, TValue>(recordName, "Key", "Value");
        }

        public override void Save(TDictionary obj, CtpObjectWriter writer)
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
                    m_serializeT.Save(false, item.Key, item.Value, writer);
                }
            }
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.StartArray(m_recordName);
            m_serializeT.WriteSchema(schema);
            schema.EndArray();
        }

        public override TDictionary Load(CtpCommandReader reader)
        {
            if (!reader.IsArray)
                throw new Exception("Expecting An Array Type");
            if (reader.IsElementOrArrayNull)
            {
                reader.Read();

                return null;
            }

            Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>(reader.ArrayCount);

            int x = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case CommandSchemaSymbol.StartElement:
                    case CommandSchemaSymbol.StartArray:
                    case CommandSchemaSymbol.Value:
                        m_serializeT.Load(reader, out bool isNull, out TKey key, out TValue value);
                        if (isNull)
                            throw new NotSupportedException();
                        items[key] = value;
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
