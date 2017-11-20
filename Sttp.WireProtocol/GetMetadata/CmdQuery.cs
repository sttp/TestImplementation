using System;

namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdQuery : ICmd
    {
        public SubCommand SubCommand => SubCommand.Query;

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


    }
}