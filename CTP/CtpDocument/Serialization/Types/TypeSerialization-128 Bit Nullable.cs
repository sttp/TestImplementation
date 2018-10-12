using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.Serialization
{
    internal class TypeSerializationDecimalNull
        : TypeSerializationMethodBase<decimal?>
    {
        public override decimal? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (decimal?)reader.Value;
        }

        public override void Save(decimal? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationGuidNull
        : TypeSerializationMethodBase<Guid?>
    {
        public override Guid? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (Guid?)reader.Value;
        }

        public override void Save(Guid? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}