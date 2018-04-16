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

        public MetadataColumn(CtpDocumentElement documentElement)
        {
            Name = (string)documentElement.GetValue("Name");
            TypeCode = (CtpTypeCode)Enum.Parse(typeof(CtpTypeCode), (string)documentElement.GetValue("TypeCode"));
            documentElement.ErrorIfNotHandled();
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
       
        public void Save(CtpDocumentWriter sml)
        {
            sml.WriteValue("Name", Name);
            sml.WriteValue("TypeCode", TypeCode.ToString());
        }
    }
}