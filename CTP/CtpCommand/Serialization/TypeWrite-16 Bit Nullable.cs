using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16Null
        : NativeMethodsIOBase<ushort?>
    {
        public override void Save(ushort? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt16Null
        : NativeMethodsIOBase<short?>
    {
        public override void Save(short? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteCharNull
        : NativeMethodsIOBase<char?>
    {
        public override void Save(char? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}