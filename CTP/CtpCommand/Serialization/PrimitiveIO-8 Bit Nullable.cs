using System;
using CTP;

namespace CTP.Serialization
{
    internal class PrimitiveIouInt8Null
        : PrimitiveIOMethodBase<byte?>
    {
        public override void Save(byte? obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
        public override byte? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte?)reader.Value;
        }
    }

    internal class PrimitiveIoInt8Null
        : PrimitiveIOMethodBase<sbyte?>
    {
        public override void Save(sbyte? obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
        public override sbyte? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (sbyte?)reader.Value;
        }
    }

    internal class PrimitiveIoBoolNull
        : PrimitiveIOMethodBase<bool?>
    {
        public override void Save(bool? obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
        public override bool? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (bool?)reader.Value;
        }
    }
}