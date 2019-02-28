using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimalNull
        : TypeWriteMethodBase<decimal?>
    {
        public override void Save(decimal? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteGuidNull
        : TypeWriteMethodBase<Guid?>
    {
        public override void Save(Guid? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}