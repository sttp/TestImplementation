using System;
using System.Text;
using CTP;

namespace CTP.SerializationRead
{
    internal class TypeSerializationString
        : TypeSerializationMethodBase<string>
    {
        public override string Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (string)reader.Value;
        }

      
    }

    internal class TypeSerializationByteArray
       : TypeSerializationMethodBase<byte[]>
    {
        public override byte[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte[])reader.Value;
        }

   
    }

    internal class TypeSerializationCharArray
        : TypeSerializationMethodBase<char[]>
    {
        public override char[] Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (char[])reader.Value;
        }

        
    }

    internal class TypeSerializationCommand
        : TypeSerializationMethodBase<CtpCommand>
    {
        public override CtpCommand Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpCommand)reader.Value;
        }

        
    }

    internal class TypeSerializationBuffer
        : TypeSerializationMethodBase<CtpBuffer>
    {
        public override CtpBuffer Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpBuffer)reader.Value;
        }

      
    }

    internal class TypeSerializationNumeric
        : TypeSerializationMethodBase<CtpNumeric>
    {
        public override CtpNumeric Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpNumeric)reader.Value;
        }
    }

    internal class TypeSerializationNumericNull
        : TypeSerializationMethodBase<CtpNumeric?>
    {
        public override CtpNumeric? Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpNumeric?)reader.Value;
        }
    }


    internal class TypeSerializationCtpObject
        : TypeSerializationMethodBase<CtpObject>
    {
        public override CtpObject Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpObject)reader.Value;
        }

      
    }

    internal class TypeSerializationObject
        : TypeSerializationMethodBase<object>
    {
        public override object Load(CtpCommandReader reader)
        {
            if (reader.NodeType != CtpCommandNodeType.Value)
                throw new Exception("Parsing Error");
            return reader.Value.ToNativeType;
        }

        
    }
}