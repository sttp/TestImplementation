using System;
using System.Threading;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimalNull
        : TypeWriteMethodBase<decimal?>
    {
        public override void Save(decimal? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteGuidNull
        : TypeWriteMethodBase<Guid?>
    {
        public override void Save(Guid? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}