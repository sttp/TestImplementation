using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt16
        : TypeSerializationMethodBase<ushort>
    {
        public override bool CanAcceptNulls => false;

        public override ushort Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (ushort)reader.Value;
        }

        public override void Save(ushort obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt16
        : TypeSerializationMethodBase<short>
    {
        public override bool CanAcceptNulls => false;

        public override short Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (short)reader.Value;
        }

        public override void Save(short obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationChar
        : TypeSerializationMethodBase<char>
    {
        public override bool CanAcceptNulls => false;

        public override char Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (char)reader.Value;
        }

        public override void Save(char obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}