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

    internal class TypeSerializationCharArray
        : TypeSerializationMethodValueType<char[]>
    {
        public override char[] Load(CtpObject reader)
        {
            return (char[])reader;
        }

        public override CtpObject Save(char[] obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationDocument
        : TypeSerializationMethodValueType<CtpDocument>
    {
        public override CtpDocument Load(CtpObject reader)
        {
            return (CtpDocument)reader;
        }

        public override CtpObject Save(CtpDocument obj)
        {
            return (CtpObject)obj;
        }
    }

    internal class TypeSerializationBuffer
        : TypeSerializationMethodValueType<CtpBuffer>
    {
        public override CtpBuffer Load(CtpObject reader)
        {
            return (CtpBuffer)reader;
        }

        public override CtpObject Save(CtpBuffer obj)
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

    internal class TypeSerializationObject
        : TypeSerializationMethodValueType<object>
    {
        public override object Load(CtpObject reader)
        {
            return reader.ToNativeType;
        }

        public override CtpObject Save(object obj)
        {
            return (CtpObject)obj;
        }
    }
}