using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandMetadataSchema
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly List<MetadataSchemaTables> Tables;

        public CommandMetadataSchema(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            Tables = reader.ReadList<MetadataSchemaTables>();
        }
    }
}