using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt32Null
        : TypeSerializationMethodBase<uint?>
    {
        public override bool CanAcceptNulls => true;

        public override uint? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint?)reader.Value;
        }

        public override void Save(uint? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt32Null
        : TypeSerializationMethodBase<int?>
    {
        public override bool CanAcceptNulls => true;

        public override int? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (int?)reader.Value;
        }

        public override void Save(int? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationSingleNull
        : TypeSerializationMethodBase<float?>
    {
        public override bool CanAcceptNulls => true;

        public override float? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (float?)reader.Value;
        }

        public override void Save(float? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}