using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64
        : TypeSerializationMethodBase2<ulong>
    {
        public override ulong Load(CtpObject reader)
        {
            return (ulong)reader;
        }

        public override CtpObject Save(ulong obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt64
        : TypeSerializationMethodBase2<long>
    {

        public override long Load(CtpObject reader)
        {
            return (long)reader;
        }

        public override CtpObject Save(long obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationDouble
        : TypeSerializationMethodBase2<double>
    {

        public override double Load(CtpObject reader)
        {
            return (double)reader;
        }

        public override CtpObject Save(double obj)
        {
            return (CtpObject)obj;
        }
    }
  
    internal class TypeSerializationDateTime
        : TypeSerializationMethodBase2<DateTime>
    {
        public override DateTime Load(CtpObject reader)
        {
            return (DateTime)reader;
        }

        public override CtpObject Save(DateTime obj)
        {
            return (CtpObject)obj;
        }
    }
}