using System.Text;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationString
        : TypeSerializationMethodValueType<string>
    {
        public override string Load(CtpObject reader)
        {
            return (string)reader;
        }

        public override CtpObject Save(string obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationByteArray
       : TypeSerializationMethodValueType<byte[]>
    {
        public override byte[] Load(CtpObject reader)
        {
            return (byte[])reader;
        }

        public override CtpObject Save(byte[] obj)
        {
            return (CtpObject)obj;
        }
    }


    internal class TypeSerializationCtpObject
        : TypeSerializationMethodValueType<CtpObject>
    {
        public override CtpObject Load(CtpObject reader)
        {
            return (CtpObject)reader;
        }

        public override CtpObject Save(CtpObject obj)
        {
            return (CtpObject)obj;
        }
    }
}