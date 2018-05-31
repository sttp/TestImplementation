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

        [CtpSerializeField("TypeCode")]
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

        public MetadataColumn(string name, CtpTypeCode typeCode)
        {
            TypeCode = typeCode;
            Name = name;
        }

        //Exists to support CtpSerializable
        private MetadataColumn() { }

    }
}