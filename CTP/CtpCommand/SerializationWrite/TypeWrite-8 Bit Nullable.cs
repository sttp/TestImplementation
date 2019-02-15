using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt8Null
        : TypeWriteMethodBase<byte?>
    {
        public override void Save(byte? obj, CtpCommandWriter writer)
        {
            writer.WriteValue( (CtpObject)obj);
        }
    }

    internal class TypeWriteInt8Null
        : TypeWriteMethodBase<sbyte?>
    {
        public override void Save(sbyte? obj, CtpCommandWriter writer)
        {
            writer.WriteValue( (CtpObject)obj);
        }
    }

    internal class TypeWriteBoolNull
        : TypeWriteMethodBase<bool?>
    {
        public override void Save(bool? obj, CtpCommandWriter writer)
        {
            writer.WriteValue( (CtpObject)obj);
        }
    }
}