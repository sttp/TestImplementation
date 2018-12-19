using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class TypeSerializationDecimalNull
        : TypeSerializationMethodBase<decimal?>
    {
        public override decimal? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (decimal?)reader.Value;
        }

        public override void Save(decimal? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationGuidNull
        : TypeSerializationMethodBase<Guid?>
    {
        public override Guid? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (Guid?)reader.Value;
        }

        public override void Save(Guid? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}