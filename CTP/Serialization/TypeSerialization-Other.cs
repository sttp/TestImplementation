using System.Text;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationString
        : TypeSerializationMethodBase2<string>
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
       : TypeSerializationMethodBase2<byte[]>
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

}