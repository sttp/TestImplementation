using System;
using System.Text;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationString
        : TypeSerializationMethodBase<string>
    {
        public override bool CanAcceptNulls => true;

        public override string Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (string)reader.Value;
        }

        public override void Save(string obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationByteArray
       : TypeSerializationMethodBase<byte[]>
    {
        public override bool CanAcceptNulls => true;

        public override byte[] Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte[])reader.Value;
        }

        public override void Save(byte[] obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationCharArray
        : TypeSerializationMethodBase<char[]>
    {
        public override bool CanAcceptNulls => true;

        public override char[] Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (char[])reader.Value;
        }

        public override void Save(char[] obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationDocument
        : TypeSerializationMethodBase<CtpDocument>
    {
        public override bool CanAcceptNulls => true;

        public override CtpDocument Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpDocument)reader.Value;
        }

        public override void Save(CtpDocument obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationBuffer
        : TypeSerializationMethodBase<CtpBuffer>
    {
        public override bool CanAcceptNulls => true;

        public override CtpBuffer Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpBuffer)reader.Value;
        }

        public override void Save(CtpBuffer obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }


    internal class TypeSerializationCtpObject
        : TypeSerializationMethodBase<CtpObject>
    {
        public override bool CanAcceptNulls => true;

        public override CtpObject Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (CtpObject)reader.Value;
        }

        public override void Save(CtpObject obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationObject
        : TypeSerializationMethodBase<object>
    {
        public override bool CanAcceptNulls => true;

        public override object Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return reader.Value.ToNativeType;
        }

        public override void Save(object obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}