using System;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class TypeSerializationDecimal
        : TypeSerializationMethodBase<decimal>
    {
        public override decimal Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (decimal)reader.Value;
        }

        public override void Save(decimal obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationGuid
        : TypeSerializationMethodBase<Guid>
    {
        public override Guid Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (Guid)reader.Value;
        }

        public override void Save(Guid obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
}