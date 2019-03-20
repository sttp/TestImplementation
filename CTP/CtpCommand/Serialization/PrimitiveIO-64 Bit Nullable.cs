using System;
using CTP;

namespace CTP.Serialization
{
    internal class PrimitiveIouInt64Null
        : PrimitiveIOMethodBase<ulong?>
    {

        public override void Save(ulong? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override ulong? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (ulong?)reader.Value;
        }
    }

    internal class PrimitiveIoInt64Null
        : PrimitiveIOMethodBase<long?>
    {

        public override void Save(long? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override long? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (long?)reader.Value;
        }
    }

    internal class PrimitiveIoDoubleNull
        : PrimitiveIOMethodBase<double?>
    {

        public override void Save(double? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override double? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (double?)reader.Value;
        }
    }

    internal class PrimitiveIoDateTimeNull
        : PrimitiveIOMethodBase<DateTime?>
    {

        public override void Save(DateTime? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override DateTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (DateTime?)reader.Value;
        }

    }

    internal class PrimitiveIoCtpTimeNull
        : PrimitiveIOMethodBase<CtpTime?>
    {

        public override void Save(CtpTime? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CommandSchemaSymbol.Value)
                throw new Exception("Parsing Error");
            return (CtpTime?)reader.Value;
        }
    }
}