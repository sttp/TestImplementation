using System;
using System.Collections.Generic;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    public class MetadataColumn
        : DocumentObject<MetadataColumn>
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        [DocumentField()]
        public string Name { get; private set; }
        /// <summary>
        /// The type of this column
        /// </summary>
        public CtpTypeCode TypeCode { get; private set; }

        [DocumentField("TypeCode")]
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
        private MetadataColumn()
        { }
       
    }
}