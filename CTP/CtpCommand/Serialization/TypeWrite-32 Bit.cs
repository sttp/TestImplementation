using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt32
        : NativeMethodsIOBase<uint>
    {
        public override void Save(uint obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt32
        : NativeMethodsIOBase<int>
    {
        public override void Save(int obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteSingle
        : NativeMethodsIOBase<float>
    {
        public override void Save(float obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}