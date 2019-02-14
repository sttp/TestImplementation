using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeSerializationUInt8
        : TypeSerializationMethodBase<byte>
    {
        public override byte Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte)reader.Value;
        }

    }

    internal class TypeSerializationInt8
        : TypeSerializationMethodBase<sbyte>
    {
        public override sbyte Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (sbyte)reader.Value;
        }

    }

    internal class TypeSerializationBool
        : TypeSerializationMethodBase<bool>
    {
        public override bool Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (bool)reader.Value;
        }

    }
}