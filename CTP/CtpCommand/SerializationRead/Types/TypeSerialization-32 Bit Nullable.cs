using System;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeSerializationUInt32Null
        : TypeSerializationMethodBase<uint?>
    {
        public override uint? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (uint?)reader.Value;
        }

    }

    internal class TypeSerializationInt32Null
        : TypeSerializationMethodBase<int?>
    {
        public override int? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (int?)reader.Value;
        }

     
    }

    internal class TypeSerializationSingleNull
        : TypeSerializationMethodBase<float?>
    {
        public override float? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (float?)reader.Value;
        }

        
    }
}