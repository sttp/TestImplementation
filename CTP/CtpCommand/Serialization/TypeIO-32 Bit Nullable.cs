using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeIOUInt32Null
        : NativeMethodsIOBase<uint?>
    {
        public override void Save(uint? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override uint? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint?)reader.Value;
        }
    }

    internal class TypeIOInt32Null
        : NativeMethodsIOBase<int?>
    {
        public override void Save(int? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override int? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (int?)reader.Value;
        }
    }

    internal class TypeIOSingleNull
        : NativeMethodsIOBase<float?>
    {
        public override void Save(float? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override float? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (float?)reader.Value;
        }
    }
}