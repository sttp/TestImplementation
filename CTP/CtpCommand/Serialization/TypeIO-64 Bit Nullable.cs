using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeIOUInt64Null
        : NativeMethodsIOBase<ulong?>
    {

        public override void Save(ulong? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override ulong? Load(CtpCommandReader reader)
        {
            return (ulong?)reader.Value;
        }
    }

    internal class TypeIOInt64Null
        : NativeMethodsIOBase<long?>
    {

        public override void Save(long? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override long? Load(CtpCommandReader reader)
        {
            return (long?)reader.Value;
        }
    }

    internal class TypeIODoubleNull
        : NativeMethodsIOBase<double?>
    {

        public override void Save(double? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override double? Load(CtpCommandReader reader)
        {
            return (double?)reader.Value;
        }
    }

    internal class TypeIODateTimeNull
        : NativeMethodsIOBase<DateTime?>
    {

        public override void Save(DateTime? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override DateTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime?)reader.Value;
        }

    }

    internal class TypeIOCtpTimeNull
        : NativeMethodsIOBase<CtpTime?>
    {

        public override void Save(CtpTime? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime?)reader.Value;
        }
    }
}