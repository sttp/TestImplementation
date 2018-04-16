using System;
using System.Collections.Generic;
using System.Text;
using CTP;

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
        public readonly CtpTypeCode TypeCode;

        public MetadataColumn(CtpMarkupElement element)
        {
            Name = (string)element.GetValue("Name");
            TypeCode = (CtpTypeCode)Enum.Parse(typeof(CtpTypeCode), (string)element.GetValue("TypeCode"));
            element.ErrorIfNotHandled();
        }

        public MetadataColumn(string name, CtpTypeCode typeCode)
        {
            TypeCode = typeCode;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} ({TypeCode})";
        }
       
        public void Save(CtpMarkupWriter sml)
        {
            sml.WriteValue("Name", Name);
            sml.WriteValue("TypeCode", TypeCode.ToString());
        }
    }
}