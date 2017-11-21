using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadataSchema
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;

        public CommandGetMetadataSchema(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
        }
    }
}