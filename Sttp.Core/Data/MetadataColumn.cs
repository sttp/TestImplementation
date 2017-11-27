using System.Collections.Generic;
using Sttp.WireProtocol;

namespace Sttp.Data
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

    }
}