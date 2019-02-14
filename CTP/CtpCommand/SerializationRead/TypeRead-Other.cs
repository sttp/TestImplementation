using System;
using System.Text;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadString
        : TypeReadMethodBase<string>
    {
        public override string Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (string)reader.Value;
        }
    }

    internal class TypeReadByteArray
       : TypeReadMethodBase<byte[]>
    {
        public override byte[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte[])reader.Value;
        }
    }

    internal class TypeReadCharArray
        : TypeReadMethodBase<char[]>
    {
        public override char[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char[])reader.Value;
        }
    }

    internal class TypeReadCommand
        : TypeReadMethodBase<CtpCommand>
    {
        public override CtpCommand Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpCommand)reader.Value;
        }
    }

    internal class TypeReadBuffer
        : TypeReadMethodBase<CtpBuffer>
    {
        public override CtpBuffer Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpBuffer)reader.Value;
        }
    }

    internal class TypeReadNumeric
        : TypeReadMethodBase<CtpNumeric>
    {
        public override CtpNumeric Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpNumeric)reader.Value;
        }
    }

    internal class TypeReadNumericNull
        : TypeReadMethodBase<CtpNumeric?>
    {
        public override CtpNumeric? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpNumeric?)reader.Value;
        }
    }

    internal class TypeReadCtpObject
        : TypeReadMethodBase<CtpObject>
    {
        public override CtpObject Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpObject)reader.Value;
        }
    }

    internal class TypeReadObject
        : TypeReadMethodBase<object>
    {
        public override object Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return reader.Value.ToNativeType;
        }
    }
}