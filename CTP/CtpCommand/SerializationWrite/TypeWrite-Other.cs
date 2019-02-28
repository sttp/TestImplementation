using System;
using System.Text;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteString
        : TypeWriteMethodBase<string>
    {
        public override void Save(string obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteByteArray
       : TypeWriteMethodBase<byte[]>
    {
        public override void Save(byte[] obj, CtpObjectWriter writer)
        {
            writer.Write(obj);
        }
    }

    internal class TypeWriteCharArray
        : TypeWriteMethodBase<char[]>
    {
        public override void Save(char[] obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteCommand
        : TypeWriteMethodBase<CtpCommand>
    {
        public override void Save(CtpCommand obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteBuffer
        : TypeWriteMethodBase<CtpBuffer>
    {
        public override void Save(CtpBuffer obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteNumeric
        : TypeWriteMethodBase<CtpNumeric>
    {
        public override void Save(CtpNumeric obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }

    internal class TypeWriteNumericNull
        : TypeWriteMethodBase<CtpNumeric?>
    {
        public override void Save(CtpNumeric? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }


    internal class TypeWriteCtpObject
        : TypeWriteMethodBase<CtpObject>
    {
        public override void Save(CtpObject obj, CtpObjectWriter writer)
        {
            writer.Write(obj);
        }
    }

    internal class TypeWriteObject
        : TypeWriteMethodBase<object>
    {
        public override void Save(object obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
    }
}