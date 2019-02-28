using System;
using System.Text;
using CTP;

namespace CTP.SerializationWrite
{
    internal class TypeWriteString
        : TypeWriteMethodBase<string>
    {
        public override void Save(string obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteByteArray
       : TypeWriteMethodBase<byte[]>
    {
        public override void Save(byte[] obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)CtpBuffer.DoNotClone(obj));
        }
    }

    internal class TypeWriteCharArray
        : TypeWriteMethodBase<char[]>
    {
        public override void Save(char[] obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteCommand
        : TypeWriteMethodBase<CtpCommand>
    {
        public override void Save(CtpCommand obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteBuffer
        : TypeWriteMethodBase<CtpBuffer>
    {
        public override void Save(CtpBuffer obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteNumeric
        : TypeWriteMethodBase<CtpNumeric>
    {
        public override void Save(CtpNumeric obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }

    internal class TypeWriteNumericNull
        : TypeWriteMethodBase<CtpNumeric?>
    {
        public override void Save(CtpNumeric? obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }


    internal class TypeWriteCtpObject
        : TypeWriteMethodBase<CtpObject>
    {
        public override void Save(CtpObject obj, CtpCommandWriter writer)
        {
            writer.WriteValue(obj);
        }
    }

    internal class TypeWriteObject
        : TypeWriteMethodBase<object>
    {
        public override void Save(object obj, CtpCommandWriter writer)
        {
            writer.WriteValue((CtpObject)obj);
        }
    }
}