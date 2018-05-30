using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt16
        : TypeSerializationMethodBase2<ushort>
    {
        public override ushort Load(CtpObject reader)
        {
            return (ushort)reader;
        }

        public override CtpObject Save(ushort obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt16
        : TypeSerializationMethodBase2<short>
    {
        public override short Load(CtpObject reader)
        {
            return (short)reader;
        }

        public override CtpObject Save(short obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationChar
        : TypeSerializationMethodBase2<char>
    {
        public override char Load(CtpObject reader)
        {
            return (char)reader;
        }

        public override CtpObject Save(char obj)
        {
            return (CtpObject)obj;
        }
    }
}