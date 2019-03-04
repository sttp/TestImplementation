using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt8Null
        : NativeMethodsIOBase<byte?>
    {
        public override void Save(byte? obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }

    internal class TypeWriteInt8Null
        : NativeMethodsIOBase<sbyte?>
    {
        public override void Save(sbyte? obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }

    internal class TypeWriteBoolNull
        : NativeMethodsIOBase<bool?>
    {
        public override void Save(bool? obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }
}