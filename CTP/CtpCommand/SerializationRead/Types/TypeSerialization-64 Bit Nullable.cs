using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeSerializationUInt64Null
        : TypeSerializationMethodBase<ulong?>
    {
        public override ulong? Load(CtpCommandReader reader)
        {
            return (ulong?)reader.Value;
        }

        
    }

    internal class TypeSerializationInt64Null
        : TypeSerializationMethodBase<long?>
    {

        public override long? Load(CtpCommandReader reader)
        {
            return (long?)reader.Value;
        }

       
    }

    internal class TypeSerializationDoubleNull
        : TypeSerializationMethodBase<double?>
    {

        public override double? Load(CtpCommandReader reader)
        {
            return (double?)reader.Value;
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

    }
}