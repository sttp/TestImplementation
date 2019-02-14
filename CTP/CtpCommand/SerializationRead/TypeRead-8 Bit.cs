using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadUInt8
        : TypeReadMethodBase<byte>
    {
        public override byte Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte)reader.Value;
        }

    }

    internal class TypeReadInt8
        : TypeReadMethodBase<sbyte>
    {
        public override sbyte Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (sbyte)reader.Value;
        }

    }

    internal class TypeReadBool
        : TypeReadMethodBase<bool>
    {
        public override bool Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (bool)reader.Value;
        }

    }
}