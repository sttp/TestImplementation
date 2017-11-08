using System;

namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdRequestQuery : ICmd
    {
        public SubCommand SubCommand => SubCommand.RequestQuery;
        public Guid SchemaVersion;
        public long Revision;
        public bool IsUpdateQuery;
        public SttpQueryExpression Expression;

        public void Load(PacketReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            IsUpdateQuery = reader.ReadBoolean();
            Expression = reader.Read<SttpQueryExpression>();
        }
    }
}