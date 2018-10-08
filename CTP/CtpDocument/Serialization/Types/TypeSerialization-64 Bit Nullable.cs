using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt64Null
        : TypeSerializationMethodBase<ulong?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override ulong? Load(CtpObject reader)
        {
            return (ulong?)reader;
        }

        public override CtpObject Save(ulong? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt64Null
        : TypeSerializationMethodBase<long?>
    {

        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override long? Load(CtpObject reader)
        {
            return (long?)reader;
        }

        public override CtpObject Save(long? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationDoubleNull
        : TypeSerializationMethodBase<double?>
    {

        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override double? Load(CtpObject reader)
        {
            return (double?)reader;
        }

        public override CtpObject Save(double? obj)
        {
            return (CtpObject)obj;
        }
    }
  
    internal class TypeSerializationDateTimeNull
        : TypeSerializationMethodBase<DateTime?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override DateTime? Load(CtpObject reader)
        {
            return (DateTime?)reader;
        }

        public override CtpObject Save(DateTime? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationCtpTimeNull
        : TypeSerializationMethodBase<CtpTime?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override CtpTime? Load(CtpObject reader)
        {
            return (CtpTime?)reader;
        }

        public override CtpObject Save(CtpTime? obj)
        {
            return (CtpObject)obj;
        }
    }
}