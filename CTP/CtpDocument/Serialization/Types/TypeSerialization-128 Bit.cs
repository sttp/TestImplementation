using System;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class TypeSerializationDecimal
        : TypeSerializationMethodBase<decimal>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => false;

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
        : TypeSerializationMethodBase<Guid>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => false;

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