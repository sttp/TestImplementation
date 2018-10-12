using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64Null
        : TypeSerializationMethodBase<ulong?>
    {
        public override ulong? Load(CtpDocumentReader reader)
        {
            return (ulong?)reader.Value;
        }

        public override void Save(ulong? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt64Null
        : TypeSerializationMethodBase<long?>
    {

        public override long? Load(CtpDocumentReader reader)
        {
            return (long?)reader.Value;
        }

        public override void Save(long? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationDoubleNull
        : TypeSerializationMethodBase<double?>
    {

        public override double? Load(CtpDocumentReader reader)
        {
            return (double?)reader.Value;
        }

        public override void Save(double? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
  
    internal class TypeSerializationDateTimeNull
        : TypeSerializationMethodBase<DateTime?>
    {
        public override DateTime? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime?)reader.Value;
        }

        public override void Save(DateTime? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationCtpTimeNull
        : TypeSerializationMethodBase<CtpTime?>
    {
        public override CtpTime? Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime?)reader.Value;
        }

        public override void Save(CtpTime? obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}