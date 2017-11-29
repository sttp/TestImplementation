using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandMetadataSchemaUpdate
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly long UpdatedFromVersion;
        public readonly List<MetadataSchemaTableUpdate> Tables;

        public CommandMetadataSchemaUpdate(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            UpdatedFromVersion = reader.ReadInt64();
            Tables = reader.ReadListMetadataSchemaTableUpdate();
        }
    }
}