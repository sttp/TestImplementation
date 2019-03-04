using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeIOUInt8Null
        : NativeMethodsIOBase<byte?>
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

    internal class TypeIOInt8Null
        : NativeMethodsIOBase<sbyte?>
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

    internal class TypeIOBoolNull
        : NativeMethodsIOBase<bool?>
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