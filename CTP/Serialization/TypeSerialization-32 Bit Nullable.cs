using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt32Null
        : TypeSerializationMethodBase2<uint?>
    {
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
        : TypeSerializationMethodBase2<int?>
    {
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
        : TypeSerializationMethodBase2<float?>
    {
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