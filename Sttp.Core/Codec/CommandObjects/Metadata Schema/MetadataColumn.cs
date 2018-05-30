using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable()]
    public class MetadataColumn
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        [CtpSerializeField()]
        public string Name { get; private set; }
        /// <summary>
        /// The type of this column
        /// </summary>
        public CtpTypeCode TypeCode { get; private set; }

        [CtpSerializeField()]
        private string TypeCodeInternal
        {
            get
            {
                return TypeCode.ToString();
            }
            set
            {
                TypeCode = (CtpTypeCode)Enum.Parse(typeof(CtpTypeCode), value);
            }

        }

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

        private MetadataColumn()
        {

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