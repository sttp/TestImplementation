using System.Collections.Generic;
using System.Text;

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

        public MetadataColumn(ByteReader reader)
        {
            Name = reader.ReadString();
            TypeCode = (SttpValueTypeCode)reader.ReadByte();
        }

        public void Save(ByteWriter writer)
        {
            writer.Write(Name);
            writer.Write((byte)TypeCode);
        }

        public override string ToString()
        {
            return $"{Name} ({TypeCode})";
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix);
            builder.Append($"({nameof(MetadataColumn)}) Name: { Name} ({TypeCode})");
            builder.AppendLine();
        }

    }
}