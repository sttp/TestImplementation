using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
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