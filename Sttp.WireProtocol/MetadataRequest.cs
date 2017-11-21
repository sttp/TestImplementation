using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    public class MetadataRequest
    {
        public Guid SchemaVersion;
        public long Revision;
        public bool IsUpdateQuery;
        public SttpQueryExpression Expression;

        public void Load(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            IsUpdateQuery = reader.ReadBoolean();
            Expression = reader.Read<SttpQueryExpression>();
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(SchemaVersion);
            writer.Write(Revision);
            writer.Write(IsUpdateQuery);
            writer.Write(Expression);
        }
    }
}
