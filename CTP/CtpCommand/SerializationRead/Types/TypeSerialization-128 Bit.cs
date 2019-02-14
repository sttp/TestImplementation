using System;
using CTP;
using GSF;

namespace CTP.SerializationRead
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

    }
}