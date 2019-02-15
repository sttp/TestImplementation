using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16Null
        : TypeWriteMethodBase<ushort?>
    {
        public override void Save(ushort? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteInt16Null
        : TypeWriteMethodBase<short?>
    {
        public override void Save(short? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteCharNull
        : TypeWriteMethodBase<char?>
    {
        public override void Save(char? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}