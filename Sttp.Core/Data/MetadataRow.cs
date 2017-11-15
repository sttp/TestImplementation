using System;
using System.Collections.Generic;
using System.Linq;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    public class MetadataRow
    {
        public SttpValue Key;
        public SttpValueSet Fields;
        public MetadataRow[] ForeignKeys;

        public MetadataRow(SttpValue key, SttpValueSet fields)
        {
            Key = key;
            Fields = fields;
            ForeignKeys = new MetadataRow[fields.Values.Count];
        }

        public void Update(SttpValueSet values)
        {
            Fields = values;
        }
    }
}
