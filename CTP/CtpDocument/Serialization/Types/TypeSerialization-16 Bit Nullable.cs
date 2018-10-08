using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt16Null
        : TypeSerializationMethodBase<ushort?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<short?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<char?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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