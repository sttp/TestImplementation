using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt32
        : TypeSerializationMethodBase<uint>
    {
        public override bool CanAcceptNulls => false;

        public override uint Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint)reader.Value;
        }

        public override void Save(uint obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt32
        : TypeSerializationMethodBase<int>
    {
        public override bool CanAcceptNulls => false;

        public override int Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (int)reader.Value;
        }

        public override void Save(int obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationSingle
        : TypeSerializationMethodBase<float>
    {
        public override bool CanAcceptNulls => false;

        public override float Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (float)reader.Value;
        }

        public override void Save(float obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}