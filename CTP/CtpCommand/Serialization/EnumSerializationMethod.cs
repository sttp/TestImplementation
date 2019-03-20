using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Serialization
{
    internal class EnumSerializationMethod<T>
        : TypeIOMethodBase<T>
        where T : struct
    {
        private string m_recordName;

        public EnumSerializationMethod(string recordName)
        {
            m_recordName = recordName;
        }

        public override void Save(T obj, CtpObjectWriter writer)
        {
            writer.Write(obj.ToString());
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            schema.Value(m_recordName);
        }

        public override T Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Expecting a value type");
            if (Enum.TryParse(reader.Value.IsString, out T rv))
            {
                return rv;
            }
            throw new Exception("Invalid Enum Expression");
        }
    }
}
