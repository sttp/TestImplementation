using System;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimal
        : TypeWriteMethodBase<decimal>
    {
        public override void Save(decimal obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteGuid
        : TypeWriteMethodBase<Guid>
    {
        public override void Save(Guid obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}