using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void DefineTable(string tableName, TableFlags tableFlags, List<Tuple<string,SttpValueTypeCode>> columns)
        {
            Stream.Write(SubCommand.DefineTable);
            Stream.Write(tableName);
            Stream.Write(tableFlags);
            Stream.Write(columns);
        }

        public void DatabaseVersion(Guid schemaVersion, long revision)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(schemaVersion);
            Stream.Write(revision);
        }

        public void DefineTableRelationship(string tableName, string columnName, string foreignTableName)
        {
            Stream.Write(SubCommand.DefineTableRelationship);
            Stream.Write(tableName);
            Stream.Write(columnName);
            Stream.Write(foreignTableName);
        }

        public void RequestFailed(string reason, string details)
        {
            Stream.Write(SubCommand.DefineTableRelationship);
            Stream.Write(reason);
            Stream.Write(details);
        }


    }
}
