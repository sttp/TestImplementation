using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt64
        : TypeWriteMethodBase<ulong>
    {
        public override void Save(ulong obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt64
        : TypeWriteMethodBase<long>
    {
        public override void Save(long obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteDouble
        : TypeWriteMethodBase<double>
    {
       
        public override void Save(double obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
  
    internal class TypeWriteDateTime
        : TypeWriteMethodBase<DateTime>
    {
        public override void Save(DateTime obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteCtpTime
        : TypeWriteMethodBase<CtpTime>
    {
        public override void Save(CtpTime obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}