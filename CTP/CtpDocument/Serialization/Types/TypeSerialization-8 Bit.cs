using System;
using CTP;

namespace CTP.Serialization
{
    internal class TypeSerializationUInt8
        : TypeSerializationMethodBase<byte>
    {
        public override byte Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (byte)reader.Value;
        }

        public override void Save(byte obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationInt8
        : TypeSerializationMethodBase<sbyte>
    {
        public override sbyte Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (sbyte)reader.Value;
        }

        public override void Save(sbyte obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }

    internal class TypeSerializationBool
        : TypeSerializationMethodBase<bool>
    {
        public override bool Load(CtpDocumentReader reader)
        {
            if (reader.NodeType != CtpDocumentNodeType.Value)
                throw new Exception("Parsing Error");
            return (bool)reader.Value;
        }

        public override void Save(bool obj, CtpDocumentWriter writer, CtpDocumentName recordName)
        {
            writer.WriteValue(recordName, obj);
        }
    }
}