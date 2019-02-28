using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt32Null
        : TypeWriteMethodBase<uint?>
    {
        public override void Save(uint? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt32Null
        : TypeWriteMethodBase<int?>
    {
        public override void Save(int? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteSingleNull
        : TypeWriteMethodBase<float?>
    {
        public override void Save(float? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}