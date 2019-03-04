using System;
using System.Text;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteString
        : NativeMethodsIOBase<string>
    {
        public override void Save(string obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteByteArray
       : NativeMethodsIOBase<byte[]>
    {
        public override void Save(byte[] obj, CtpObjectWriter writer)
        {
            writer.Write(obj);
        }
    }

    internal class TypeWriteCharArray
        : NativeMethodsIOBase<char[]>
    {
        public override void Save(char[] obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteCommand
        : NativeMethodsIOBase<CtpCommand>
    {
        public override void Save(CtpCommand obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteBuffer
        : NativeMethodsIOBase<CtpBuffer>
    {
        public override void Save(CtpBuffer obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteNumeric
        : NativeMethodsIOBase<CtpNumeric>
    {
        public override void Save(CtpNumeric obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteNumericNull
        : NativeMethodsIOBase<CtpNumeric?>
    {
        public override void Save(CtpNumeric? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }


    internal class TypeWriteCtpObject
        : NativeMethodsIOBase<CtpObject>
    {
        public override void Save(CtpObject obj, CtpObjectWriter writer)
        {
            writer.Write(obj);
        }
    }

    internal class TypeWriteObject
        : NativeMethodsIOBase<object>
    {
        public override void Save(object obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}