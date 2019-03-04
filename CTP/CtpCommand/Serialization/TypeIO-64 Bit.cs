using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeIOUInt64
        : NativeMethodsIOBase<ulong>
    {
        public override void Save(ulong obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override ulong Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (ulong)reader.Value;
        }
    }

    internal class TypeIOInt64
        : NativeMethodsIOBase<long>
    {
        public override void Save(long obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override long Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (long)reader.Value;
        }
    }

    internal class TypeIODouble
        : NativeMethodsIOBase<double>
    {
       
        public override void Save(double obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override double Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (double)reader.Value;
        }
    }
  
    internal class TypeIODateTime
        : NativeMethodsIOBase<DateTime>
    {
        public override void Save(DateTime obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override DateTime Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime)reader.Value;
        }
    }

    internal class TypeIOCtpTime
        : NativeMethodsIOBase<CtpTime>
    {
        public override void Save(CtpTime obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpTime Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime)reader.Value;
        }
    }
}