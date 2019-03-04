using System;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteUInt64Null
        : NativeMethodsIOBase<ulong?>
    {

        public override void Save(ulong? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteInt64Null
        : NativeMethodsIOBase<long?>
    {

        public override void Save(long? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteDoubleNull
        : NativeMethodsIOBase<double?>
    {

        public override void Save(double? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteDateTimeNull
        : NativeMethodsIOBase<DateTime?>
    {

        public override void Save(DateTime? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }

    }

    internal class TypeWriteCtpTimeNull
        : NativeMethodsIOBase<CtpTime?>
    {

        public override void Save(CtpTime? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}