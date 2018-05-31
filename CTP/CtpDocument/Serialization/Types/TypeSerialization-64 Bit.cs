using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64
        : TypeSerializationMethodValueType<ulong>
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
        : TypeSerializationMethodValueType<long>
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
        : TypeSerializationMethodValueType<double>
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
        : TypeSerializationMethodValueType<DateTime>
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