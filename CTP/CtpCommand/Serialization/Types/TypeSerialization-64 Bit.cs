using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64
        : TypeSerializationMethodBase<ulong>
    {
        public override ulong Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (ulong)reader.Value;
        }

        public override void Save(ulong obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationInt64
        : TypeSerializationMethodBase<long>
    {

        public override long Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (long)reader.Value;
        }

        public override void Save(long obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationDouble
        : TypeSerializationMethodBase<double>
    {

        public override double Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (double)reader.Value;
        }

        public override void Save(double obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
  
    internal class TypeSerializationDateTime
        : TypeSerializationMethodBase<DateTime>
    {
        public override DateTime Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime)reader.Value;
        }

        public override void Save(DateTime obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationCtpTime
        : TypeSerializationMethodBase<CtpTime>
    {
        public override CtpTime Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime)reader.Value;
        }

        public override void Save(CtpTime obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
}