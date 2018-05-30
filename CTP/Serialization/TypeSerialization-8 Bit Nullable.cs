using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt8Null
        : TypeSerializationMethodBase2<byte?>
    {
        public override byte? Load(CtpObject reader)
        {
            return (byte?)reader;
        }

        public override CtpObject Save(byte? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt8Null
        : TypeSerializationMethodBase2<sbyte?>
    {
        public override sbyte? Load(CtpObject reader)
        {
            return (sbyte?)reader;
        }

        public override CtpObject Save(sbyte? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationBoolNull
        : TypeSerializationMethodBase2<bool?>
    {
        public override bool? Load(CtpObject reader)
        {
            return (bool?)reader;
        }

        public override CtpObject Save(bool? obj)
        {
            return (CtpObject)obj;
        }
    }
}