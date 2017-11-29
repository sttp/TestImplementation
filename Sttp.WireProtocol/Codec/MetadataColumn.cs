using System.Collections.Generic;

namespace Sttp.Codec
{
    public class MetadataColumn
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The type of this column
        /// </summary>
        public readonly SttpValueTypeCode TypeCode;

        public MetadataColumn(string name, SttpValueTypeCode typeCode)
        {
            TypeCode = typeCode;
            Name = name;
        }

        public MetadataColumn(PayloadReader reader)
        {
            Name = reader.ReadString();
            TypeCode = reader.Read<SttpValueTypeCode>();
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(Name);
            writer.Write((byte)TypeCode);
        }

        public override string ToString()
        {
            return $"{Name} ({TypeCode})";
        }

    }
}