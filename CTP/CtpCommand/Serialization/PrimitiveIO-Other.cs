using System;
using System.Text;
using CTP;

namespace CTP.Serialization
{
    internal class PrimitiveIoString
        : PrimitiveIOMethodBase<string>
    {
        public override void Save(string obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override string Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (string)reader.Value;
        }
    }

    internal class PrimitiveIoByteArray
       : PrimitiveIOMethodBase<byte[]>
    {
        public override void Save(byte[] obj, CtpObjectWriter writer)
        {
            writer.Write(obj);
        }
        public override byte[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte[])reader.Value;
        }
    }

    internal class PrimitiveIoCharArray
        : PrimitiveIOMethodBase<char[]>
    {
        public override void Save(char[] obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override char[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char[])reader.Value;
        }
    }

    internal class PrimitiveIoCommand
        : PrimitiveIOMethodBase<CtpCommand>
    {
        public override void Save(CtpCommand obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpCommand Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpCommand)reader.Value;
        }
    }

    internal class PrimitiveIoBuffer
        : PrimitiveIOMethodBase<CtpBuffer>
    {
        public override void Save(CtpBuffer obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpBuffer Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpBuffer)reader.Value;
        }
    }

    internal class PrimitiveIoNumeric
        : PrimitiveIOMethodBase<CtpNumeric>
    {
        public override void Save(CtpNumeric obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpNumeric Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpNumeric)reader.Value;
        }
    }

    internal class PrimitiveIoNumericNull
        : PrimitiveIOMethodBase<CtpNumeric?>
    {
        public override void Save(CtpNumeric? obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override CtpNumeric? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpNumeric?)reader.Value;
        }
    }


    internal class PrimitiveIoCtpObject
        : PrimitiveIOMethodBase<CtpObject>
    {
        public override void Save(CtpObject obj, CtpObjectWriter writer)
        {
            writer.Write(obj);
        }
        public override CtpObject Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpObject)reader.Value;
        }
    }

    internal class PrimitiveIoObject
        : PrimitiveIOMethodBase<object>
    {
        public override void Save(object obj, CtpObjectWriter writer)
        {
            writer.Write((CtpObject)obj);
        }
        public override object Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return reader.Value.ToNativeType;
        }
    }
}