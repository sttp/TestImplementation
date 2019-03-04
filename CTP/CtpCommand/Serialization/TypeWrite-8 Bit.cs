using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt8
        : NativeMethodsIOBase<byte>
    {
       
        public override void Save(byte obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }

    internal class TypeWriteInt8
        : NativeMethodsIOBase<sbyte>
    {
       
        public override void Save(sbyte obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }

    internal class TypeWriteBool
        : NativeMethodsIOBase<bool>
    {
        
        public override void Save(bool obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }
}