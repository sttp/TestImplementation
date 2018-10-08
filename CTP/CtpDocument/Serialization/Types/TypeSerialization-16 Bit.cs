using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt16
        : TypeSerializationMethodBase<ushort>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => false;

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
        : TypeSerializationMethodBase<short>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => false;

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
        : TypeSerializationMethodBase<char>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => false;

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