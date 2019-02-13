using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt32
        : TypeSerializationMethodBase<uint>
    {
        public override uint Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint)reader.Value;
        }

        public override void Save(uint obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationInt32
        : TypeSerializationMethodBase<int>
    {
        public override int Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (int)reader.Value;
        }

        public override void Save(int obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationSingle
        : TypeSerializationMethodBase<float>
    {
        public override float Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (float)reader.Value;
        }

        public override void Save(float obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
}