using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16
        : NativeMethodsIOBase<ushort>
    {
        public override void Save(ushort obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt16
        : NativeMethodsIOBase<short>
    {
        public override void Save(short obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteChar
        : NativeMethodsIOBase<char>
    {
        public override void Save(char obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}