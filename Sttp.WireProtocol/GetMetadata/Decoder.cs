using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadata
{
    public class Decoder
    {
        public Guid SchemaVersion;
        public long Revision;
        public bool IsUpdateQuery;
        public SttpQueryExpression Expression;

        public CommandCode CommandCode => CommandCode.GetMetadata;

        public void Fill(PacketReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            IsUpdateQuery = reader.ReadBoolean();
            Expression = reader.Read<SttpQueryExpression>();
        }
    }
}
