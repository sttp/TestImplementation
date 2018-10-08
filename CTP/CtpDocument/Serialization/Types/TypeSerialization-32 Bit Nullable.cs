using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt32Null
        : TypeSerializationMethodBase<uint?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override uint? Load(CtpObject reader)
        {
            return (uint?)reader;
        }

        public override CtpObject Save(uint? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt32Null
        : TypeSerializationMethodBase<int?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override int? Load(CtpObject reader)
        {
            return (int?)reader;
        }

        public override CtpObject Save(int? obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationSingleNull
        : TypeSerializationMethodBase<float?>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

        public override float? Load(CtpObject reader)
        {
            return (float?)reader;
        }

        public override CtpObject Save(float? obj)
        {
            return (CtpObject)obj;
        }
    }
}