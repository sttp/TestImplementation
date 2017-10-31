using Sttp.WireProtocol;

namespace Sttp.Data
{
    public class MetadataColumn
    {
        /// <summary>
        /// The 0 based index of this column in the data table.
        /// </summary>
        public readonly short Index;
        /// <summary>
        /// The name of the column
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The type of this column
        /// </summary>
        public readonly SttpValueTypeCode TypeCode;

        public MetadataColumn(short index, string name, SttpValueTypeCode typeCode)
        {
            Index = index;
            TypeCode = typeCode;
            Name = name;
        }

        public byte[] Encode(object value)
        {
            return SttpValueTypeCodec.Encode(TypeCode, value);
        }

        public object Decode(byte[] data)
        {
            return SttpValueTypeCodec.Decode(TypeCode, data);
        }

    }
}