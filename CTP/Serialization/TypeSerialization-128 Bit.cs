using System;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class TypeSerializationDecimal
        : TypeSerializationMethodBase2<decimal>
    {
        public override decimal Load(CtpObject reader)
        {
            return (decimal)reader;
        }

        public override CtpObject Save(decimal obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationGuid
        : TypeSerializationMethodBase2<Guid>
    {
        public override Guid Load(CtpObject reader)
        {
            return (Guid)reader;
        }

        public override CtpObject Save(Guid obj)
        {
            return (CtpObject)obj;
        }
    }
}