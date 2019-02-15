using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt64Null
        : TypeWriteMethodBase<ulong?>
    {

        public override void Save(ulong? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteInt64Null
        : TypeWriteMethodBase<long?>
    {

        public override void Save(long? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteDoubleNull
        : TypeWriteMethodBase<double?>
    {

        public override void Save(double? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteDateTimeNull
        : TypeWriteMethodBase<DateTime?>
    {

        public override void Save(DateTime? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }

    }

    internal class TypeWriteCtpTimeNull
        : TypeWriteMethodBase<CtpTime?>
    {

        public override void Save(CtpTime? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}