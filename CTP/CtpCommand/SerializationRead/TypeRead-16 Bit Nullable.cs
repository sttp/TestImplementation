using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeReadUInt16Null
        : TypeReadMethodBase<ushort?>
    {
        public override ushort? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (ushort?)reader.Value;
        }

    }

    internal class TypeReadInt16Null
        : TypeReadMethodBase<short?>
    {
        public override short? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (short?)reader.Value;
        }

       
    }

    internal class TypeReadCharNull
        : TypeReadMethodBase<char?>
    {
        public override char? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char?)reader.Value;
        }

       
    }
}