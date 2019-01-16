using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64Null
        : TypeSerializationMethodBase<ulong?>
    {
        public override ulong? Load(CtpCommandReader reader)
        {
            return (ulong?)reader.Value;
        }

        public override void Save(ulong? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationInt64Null
        : TypeSerializationMethodBase<long?>
    {

        public override long? Load(CtpCommandReader reader)
        {
            return (long?)reader.Value;
        }

        public override void Save(long? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationDoubleNull
        : TypeSerializationMethodBase<double?>
    {

        public override double? Load(CtpCommandReader reader)
        {
            return (double?)reader.Value;
        }

        public override void Save(double? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
  
    internal class TypeSerializationDateTimeNull
        : TypeSerializationMethodBase<DateTime?>
    {
        public override DateTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime?)reader.Value;
        }

        public override void Save(DateTime? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationCtpTimeNull
        : TypeSerializationMethodBase<CtpTime?>
    {
        public override CtpTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime?)reader.Value;
        }

        public override void Save(CtpTime? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
}