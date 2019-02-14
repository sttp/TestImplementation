using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadUInt32
        : TypeReadMethodBase<uint>
    {
        public override uint Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint)reader.Value;
        }

      
    }

    internal class TypeReadInt32
        : TypeReadMethodBase<int>
    {
        public override int Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (int)reader.Value;
        }

    }

    internal class TypeReadSingle
        : TypeReadMethodBase<float>
    {
        public override float Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (float)reader.Value;
        }

    }
}