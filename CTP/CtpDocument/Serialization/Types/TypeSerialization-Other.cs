using System.Text;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationString
        : TypeSerializationMethodBase<string>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
       : TypeSerializationMethodBase<byte[]>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<char[]>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<CtpDocument>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<CtpBuffer>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<CtpObject>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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
        : TypeSerializationMethodBase<object>
    {
        public override bool IsValueRecord => true;
        public override bool CanAcceptNulls => true;

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