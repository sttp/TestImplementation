using System;
using CTP;
using GSF;

namespace CTP.SerializationWrite
{
    internal class TypeWriteDecimal
        : NativeMethodsIOBase<decimal>
    {
        public override void Save(decimal obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteGuid
        : NativeMethodsIOBase<Guid>
    {
        public override void Save(Guid obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}