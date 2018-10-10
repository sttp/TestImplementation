using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64
        : TypeSerializationMethodBase<ulong>
    {
        public override bool CanAcceptNulls => false;

        public override ulong Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (ulong)reader.Value;
        }

        public override void Save(ulong obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt64
        : TypeSerializationMethodBase<long>
    {

        public override bool CanAcceptNulls => false;

        public override long Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (long)reader.Value;
        }

        public override void Save(long obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationDouble
        : TypeSerializationMethodBase<double>
    {

        public override bool CanAcceptNulls => false;

        public override double Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (double)reader.Value;
        }

        public override void Save(double obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
  
    internal class TypeSerializationDateTime
        : TypeSerializationMethodBase<DateTime>
    {
        public override bool CanAcceptNulls => false;

        public override DateTime Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime)reader.Value;
        }

        public override void Save(DateTime obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationCtpTime
        : TypeSerializationMethodBase<CtpTime>
    {
        public override bool CanAcceptNulls => false;

        public override CtpTime Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime)reader.Value;
        }

        public override void Save(CtpTime obj, CtpDocumentWriter writer, CtpDocumentNames recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}