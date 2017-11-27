using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadata
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly bool AreUpdateQueries;
        public readonly List<SttpQueryExpression> Queries;

        public CommandGetMetadata(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            AreUpdateQueries = reader.ReadBoolean();
            Queries = reader.ReadList<SttpQueryExpression>();
        }
    }
}