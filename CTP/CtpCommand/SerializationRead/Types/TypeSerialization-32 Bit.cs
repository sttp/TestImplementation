using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeSerializationUInt32
        : TypeSerializationMethodBase<uint>
    {
        public override uint Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint)reader.Value;
        }

      
    }

    internal class TypeSerializationInt32
        : TypeSerializationMethodBase<int>
    {
        public override int Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (int)reader.Value;
        }

    }

    internal class TypeSerializationSingle
        : TypeSerializationMethodBase<float>
    {
        public override float Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (float)reader.Value;
        }

    }
}