using System;
using System.Text;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationString
        : TypeSerializationMethodBase<string>
    {
        public override string Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (string)reader.Value;
        }

        public override void Save(string obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationByteArray
       : TypeSerializationMethodBase<byte[]>
    {
        public override byte[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte[])reader.Value;
        }

        public override void Save(byte[] obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationCharArray
        : TypeSerializationMethodBase<char[]>
    {
        public override char[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char[])reader.Value;
        }

        public override void Save(char[] obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationDocument
        : TypeSerializationMethodBase<CtpCommand>
    {
        public override CtpCommand Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpCommand)reader.Value;
        }

        public override void Save(CtpCommand obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationBuffer
        : TypeSerializationMethodBase<CtpBuffer>
    {
        public override CtpBuffer Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpBuffer)reader.Value;
        }

        public override void Save(CtpBuffer obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }


    internal class TypeSerializationCtpObject
        : TypeSerializationMethodBase<CtpObject>
    {
        public override CtpObject Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpObject)reader.Value;
        }

        public override void Save(CtpObject obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationObject
        : TypeSerializationMethodBase<object>
    {
        public override object Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return reader.Value.ToNativeType;
        }

        public override void Save(object obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
}