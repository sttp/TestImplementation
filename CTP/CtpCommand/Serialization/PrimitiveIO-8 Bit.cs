using System;
using CTP;

namespace CTP.Serialization
{
    internal class PrimitiveIouInt8
        : PrimitiveIOMethodBase<byte>
    {

        public override void Save(byte obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override byte Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (byte)reader.Value;
        }
    }

    internal class PrimitiveIoInt8
        : PrimitiveIOMethodBase<sbyte>
    {

        public override void Save(sbyte obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override sbyte Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (sbyte)reader.Value;
        }
    }

    internal class PrimitiveIoBool
        : PrimitiveIOMethodBase<bool>
    {

        public override void Save(bool obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override bool Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (bool)reader.Value;
        }
    }
}