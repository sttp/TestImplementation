using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt64
        : NativeMethodsIOBase<ulong>
    {
        public override void Save(ulong obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt64
        : NativeMethodsIOBase<long>
    {
        public override void Save(long obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteDouble
        : NativeMethodsIOBase<double>
    {
       
        public override void Save(double obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
  
    internal class TypeWriteDateTime
        : NativeMethodsIOBase<DateTime>
    {
        public override void Save(DateTime obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteCtpTime
        : NativeMethodsIOBase<CtpTime>
    {
        public override void Save(CtpTime obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}