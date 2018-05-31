using System;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class TypeSerializationDecimal
        : TypeSerializationMethodValueType<decimal>
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
        : TypeSerializationMethodValueType<Guid>
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