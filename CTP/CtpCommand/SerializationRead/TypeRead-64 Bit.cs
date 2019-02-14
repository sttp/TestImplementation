using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadUInt64
        : TypeReadMethodBase<ulong>
    {
        public override ulong Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (ulong)reader.Value;
        }

       
    }

    internal class TypeReadInt64
        : TypeReadMethodBase<long>
    {

        public override long Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (long)reader.Value;
        }

      
    }

    internal class TypeReadDouble
        : TypeReadMethodBase<double>
    {

        public override double Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (double)reader.Value;
        }

        
    }
  
    internal class TypeReadDateTime
        : TypeReadMethodBase<DateTime>
    {
        public override DateTime Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime)reader.Value;
        }

    }

    internal class TypeReadCtpTime
        : TypeReadMethodBase<CtpTime>
    {
        public override CtpTime Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime)reader.Value;
        }

       
    }
}