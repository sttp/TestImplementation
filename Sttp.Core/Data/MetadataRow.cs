using System;
using System.Collections.Generic;
using System.Linq;

namespace Sttp.Data
{
    public class MetadataRow
    {
        public readonly List<SttpValue> Fields;

        public MetadataRow(List<SttpValue> fields)
        {
            Fields = fields;
        }

        public override string ToString()
        {
            return string.Join(", ", Fields.Select(x => x.ToString()));
        }
    }
}
