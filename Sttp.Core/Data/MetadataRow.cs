using System;
using System.Collections.Generic;
using System.Linq;
using CTP;

namespace Sttp.Data
{
    public class MetadataRow
    {
        public readonly List<CtpValue> Fields;

        public MetadataRow(List<CtpValue> fields)
        {
            Fields = fields;
        }

        public override string ToString()
        {
            return string.Join(", ", Fields.Select(x => x.ToString()));
        }
    }
}
