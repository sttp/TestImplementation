using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt32
        : TypeWriteMethodBase<uint>
    {
        public override void Save(uint obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteInt32
        : TypeWriteMethodBase<int>
    {
        public override void Save(int obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteSingle
        : TypeWriteMethodBase<float>
    {
        public override void Save(float obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}