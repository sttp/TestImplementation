using System;
using CTP;

namespace CTP.Serialization
{
    internal class PrimitiveIouInt16Null
        : PrimitiveIOMethodBase<ushort?>
    {
        public override void Save(ushort? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override ushort? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (ushort?)reader.Value;
        }
    }

    internal class PrimitiveIoInt16Null
        : PrimitiveIOMethodBase<short?>
    {
        public override void Save(short? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override short? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (short?)reader.Value;
        }
    }

    internal class PrimitiveIoCharNull
        : PrimitiveIOMethodBase<char?>
    {
        public override void Save(char? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override char? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (char?)reader.Value;
        }
    }
}