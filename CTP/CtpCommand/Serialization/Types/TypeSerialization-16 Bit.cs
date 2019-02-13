using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt16
        : TypeSerializationMethodBase<ushort>
    {
        public override ushort Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (ushort)reader.Value;
        }

        public override void Save(ushort obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationInt16
        : TypeSerializationMethodBase<short>
    {
        public override short Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (short)reader.Value;
        }

        public override void Save(short obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }

    internal class TypeSerializationChar
        : TypeSerializationMethodBase<char>
    {
        public override char Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char)reader.Value;
        }

        public override void Save(char obj, CtpCommandWriter writer, int recordName)
        {
            writer.WriteValue(recordName, (CtpObject)obj);
        }
    }
}