using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadUInt64Null
        : TypeReadMethodBase<ulong?>
    {
        public override ulong? Load(CtpCommandReader reader)
        {
            return (ulong?)reader.Value;
        }

        
    }

    internal class TypeReadInt64Null
        : TypeReadMethodBase<long?>
    {

        public override long? Load(CtpCommandReader reader)
        {
            return (long?)reader.Value;
        }

       
    }

    internal class TypeReadDoubleNull
        : TypeReadMethodBase<double?>
    {

        public override double? Load(CtpCommandReader reader)
        {
            return (double?)reader.Value;
        }

       
    }
  
    internal class TypeReadDateTimeNull
        : TypeReadMethodBase<DateTime?>
    {
        public override DateTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (DateTime?)reader.Value;
        }

       
    }

    internal class TypeReadCtpTimeNull
        : TypeReadMethodBase<CtpTime?>
    {
        public override CtpTime? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpTime?)reader.Value;
        }

    }
}