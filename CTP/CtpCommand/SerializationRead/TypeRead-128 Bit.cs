using System;
using CTP;
using GSF;

namespace CTP.SerializationRead
{
    internal class TypeReadDecimal
        : TypeReadMethodBase<decimal>
    {
        public override decimal Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (decimal)reader.Value;
        }

    }

    internal class TypeReadGuid
        : TypeReadMethodBase<Guid>
    {
        public override Guid Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (Guid)reader.Value;
        }

    }
}