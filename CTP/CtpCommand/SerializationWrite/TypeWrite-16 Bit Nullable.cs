using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16Null
        : TypeWriteMethodBase<ushort?>
    {
        public override void Save(ushort? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt16Null
        : TypeWriteMethodBase<short?>
    {
        public override void Save(short? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteCharNull
        : TypeWriteMethodBase<char?>
    {
        public override void Save(char? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}