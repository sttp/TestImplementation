using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt16Null
        : TypeSerializationMethodValueType<ushort?>
    {
        public override ushort? Load(CtpObject reader)
        {
            return (ushort?)reader;
        }

        public override CtpObject Save(ushort? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt16Null
        : TypeSerializationMethodValueType<short?>
    {
        public override short? Load(CtpObject reader)
        {
            return (short?)reader;
        }

        public override CtpObject Save(short? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationCharNull
        : TypeSerializationMethodValueType<char?>
    {
        public override char? Load(CtpObject reader)
        {
            return (char?)reader;
        }

        public override CtpObject Save(char? obj)
        {
            return (CtpObject)obj;
        }
    }
}