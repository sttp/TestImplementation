using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt32
        : TypeSerializationMethodValueType<uint>
    {
        public override uint Load(CtpObject reader)
        {
            return (uint)reader;
        }

        public override CtpObject Save(uint obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationInt32
        : TypeSerializationMethodValueType<int>
    {
        public override int Load(CtpObject reader)
        {
            return (int)reader;
        }

        public override CtpObject Save(int obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationSingle
        : TypeSerializationMethodValueType<float>
    {
        public override float Load(CtpObject reader)
        {
            return (float)reader;
        }

        public override CtpObject Save(float obj)
        {
            return (CtpObject)obj;
        }
    }
}