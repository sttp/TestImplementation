using System;
using CTP;

namespace CTP.Serialization
{
    internal class PrimitiveIouInt32
        : PrimitiveIOMethodBase<uint>
    {
        public override void Save(uint obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override uint Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint)reader.Value;
        }
    }

    internal class PrimitiveIoInt32
        : PrimitiveIOMethodBase<int>
    {
        public override void Save(int obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override int Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (int)reader.Value;
        }
    }

    internal class PrimitiveIoSingle
        : PrimitiveIOMethodBase<float>
    {
        public override void Save(float obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override float Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (float)reader.Value;
        }
    }
}