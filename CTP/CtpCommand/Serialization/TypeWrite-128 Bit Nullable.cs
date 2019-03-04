using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimalNull
        : NativeMethodsIOBase<decimal?>
    {
        public override void Save(decimal? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteGuidNull
        : NativeMethodsIOBase<Guid?>
    {
        public override void Save(Guid? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}