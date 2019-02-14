using System;
using CTP;

namespace CTP.SerializationRead
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

       
    }
}