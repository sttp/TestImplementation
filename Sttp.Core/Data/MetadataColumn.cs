using Sttp.WireProtocol;
using ValueType = Sttp.WireProtocol.ValueType;

namespace Sttp.Data
{
    public class MetadataColumn
    {
        /// <summary>
        /// The 0 based index of this column in the data table.
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// The name of the column
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The type of this column
        /// </summary>
        public readonly ValueType Type;

        public MetadataColumn(int index, string name, ValueType type)
        {
            Index = index;
            Type = type;
            Name = name;
        }

        public byte[] Encode(object value)
        {
            return ValueTypeCodec.Encode(Type, value);
        }

        public object Decode(byte[] data)
        {
            return ValueTypeCodec.Decode(Type, data);
        }

    }
}