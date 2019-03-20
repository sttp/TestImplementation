using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.IO;

namespace CTP.Serialization
{
    internal class KeyValuePairSerialization<TKey, TValue>
        : TypeIOMethodBase<KeyValuePair<TKey, TValue>>
    {
        private TupleHelper<TKey, TValue> m_serializeT;
        private string m_recordName;

        public KeyValuePairSerialization(string recordName)
        {
            m_recordName = recordName;
            m_serializeT = new TupleHelper<TKey, TValue>(recordName, "Key", "Value");
        }

        public override void Save(KeyValuePair<TKey, TValue> obj, CtpObjectWriter writer)
        {
            writer.Write(true);
            m_serializeT.Save(false, obj.Key, obj.Value, writer);
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            m_serializeT.WriteSchema(schema);
        }

        public override KeyValuePair<TKey, TValue> Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.StartElement)
                throw new Exception("Expecting An Element Type");
            if (reader.IsElementOrArrayNull)
                throw new Exception("Null is not supported");

            m_serializeT.Load(reader, out bool isNull, out TKey key, out TValue value);
            if (isNull)
                throw new NotSupportedException();

            return new KeyValuePair<TKey, TValue>(key, value);


        }

    }
}
