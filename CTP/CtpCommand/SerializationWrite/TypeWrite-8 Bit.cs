using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt8
        : TypeWriteMethodBase<byte>
    {
       
        public override void Save(byte obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }

    internal class TypeWriteInt8
        : TypeWriteMethodBase<sbyte>
    {
       
        public override void Save(sbyte obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }

    internal class TypeWriteBool
        : TypeWriteMethodBase<bool>
    {
        
        public override void Save(bool obj, CtpObjectWriter writer)
        {
            writer.Write( (CtpObject)obj);
        }
    }
}