using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt16
        : TypeWriteMethodBase<ushort>
    {
        public override void Save(ushort obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt16
        : TypeWriteMethodBase<short>
    {
        public override void Save(short obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteChar
        : TypeWriteMethodBase<char>
    {
        public override void Save(char obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}