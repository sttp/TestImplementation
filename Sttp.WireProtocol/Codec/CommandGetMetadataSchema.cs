using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadataSchema
    {
        public Guid SchemaVersion;
        public long Revision;

        public void Load(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
        }
    }
}