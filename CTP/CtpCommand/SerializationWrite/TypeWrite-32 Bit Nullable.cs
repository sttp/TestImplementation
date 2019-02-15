using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt32Null
        : TypeWriteMethodBase<uint?>
    {
        public override void Save(uint? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteInt32Null
        : TypeWriteMethodBase<int?>
    {
        public override void Save(int? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteSingleNull
        : TypeWriteMethodBase<float?>
    {
        public override void Save(float? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}