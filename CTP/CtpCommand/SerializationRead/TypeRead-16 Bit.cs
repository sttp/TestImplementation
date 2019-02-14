using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadUInt16
        : TypeReadMethodBase<ushort>
    {
        public override ushort Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (ushort)reader.Value;
        }

       
    }

    internal class TypeReadInt16
        : TypeReadMethodBase<short>
    {
        public override short Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (short)reader.Value;
        }

    }

    internal class TypeReadChar
        : TypeReadMethodBase<char>
    {
        public override char Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char)reader.Value;
        }

       
    }
}