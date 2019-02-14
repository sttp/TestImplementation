using System;
using CTP;

namespace CTP.SerializationRead
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

       
    }
}