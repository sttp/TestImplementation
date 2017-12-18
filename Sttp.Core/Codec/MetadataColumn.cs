using System;
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

        public MetadataColumn(SttpMarkupElement element)
        {
            if (element.ElementName != "Column")
                throw new Exception("Invalid command");

            Name = (string)element.GetValue("Name");
            TypeCode = (SttpValueTypeCode)Enum.Parse(typeof(SttpValueTypeCode), (string)element.GetValue("TypeCode"));
            element.ErrorIfNotHandled();
        }

        public MetadataColumn(string name, SttpValueTypeCode typeCode)
        {
            TypeCode = typeCode;
            Name = name;
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

        public void Save(SttpMarkupWriter sml)
        {
            using (sml.StartElement("Column"))
            {
                sml.WriteValue("Name", Name);
                sml.WriteValue("TypeCode", TypeCode.ToString());
            }
        }
    }
}