using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16
        : TypeWriteMethodBase<ushort>
    {
        public override void Save(ushort obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteInt16
        : TypeWriteMethodBase<short>
    {
        public override void Save(short obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteChar
        : TypeWriteMethodBase<char>
    {
        public override void Save(char obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}