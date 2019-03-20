using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Serialization
{
    internal class NullableSerializationMethod<T>
        : TypeIOMethodBase<T?>
        where T : struct
    {
        private TypeIOMethodBase<T> m_serializeT;

        public NullableSerializationMethod(string recordName)
        {
            m_serializeT = TypeIO.Create<T>(recordName);
        }

        public override void Save(T? obj, CtpObjectWriter writer)
        {
            if (obj.HasValue)
            {
                m_serializeT.Save(obj.Value, writer);
            }
            else
            {
                //ToDo: What if the enum is a complex data type.
                writer.Write(null);
            }
        }

        public override void WriteSchema(CommandSchemaWriter schema)
        {
            m_serializeT.WriteSchema(schema);
        }

        public override T? Load(CtpCommandReader reader)
        {
            //ToDo: What if the enum is a complex data type.
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Expecting a value type");
            if (reader.Value.IsNull)
                return null;
            return m_serializeT.Load(reader);
        }
    }
}
