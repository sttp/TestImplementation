using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt8Null
        : TypeSerializationMethodBase<byte?>
    {
        public override byte? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte?)reader.Value;
        }

        public override void Save(byte? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt8Null
        : TypeSerializationMethodBase<sbyte?>
    {
        public override sbyte? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (sbyte?)reader.Value;
        }

        public override void Save(sbyte? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationBoolNull
        : TypeSerializationMethodBase<bool?>
    {
        public override bool? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (bool?)reader.Value;
        }

        public override void Save(bool? obj, CtpCommandWriter writer, CtpCommandKeyword recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}