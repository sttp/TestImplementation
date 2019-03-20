using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class PrimitiveIoDecimalNull
        : PrimitiveIOMethodBase<decimal?>
    {
        public override void Save(decimal? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override decimal? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (decimal?)reader.Value;
        }
    }

    internal class PrimitiveIoGuidNull
        : PrimitiveIOMethodBase<Guid?>
    {
        public override void Save(Guid? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override Guid? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (Guid?)reader.Value;
        }
    }

}