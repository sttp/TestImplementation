using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.Metadata
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.Metadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void VersionNotCompatible()
        {
            Stream.Write(SubCommand.VersionNotCompatible);
        }

        public void DefineResponse(bool isUpdateQuery, long updatedFromRevision, Guid schemaVersion, long revision, string tableName, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            Stream.Write(SubCommand.DefineResponse);
            Stream.Write(isUpdateQuery);
            Stream.Write(updatedFromRevision);
            Stream.Write(schemaVersion);
            Stream.Write(revision);
            Stream.Write(tableName);
            Stream.Write(columns);
        }

        public void DefineRow(SttpValue primaryKey, SttpValueSet fields)
        {
            Stream.Write(SubCommand.DefineRow);
            Stream.Write(primaryKey);
            Stream.Write(fields);
        }

        public void UndefineRow(SttpValue primaryKey)
        {
            Stream.Write(SubCommand.UndefineRow);
            Stream.Write(primaryKey);
        }

        public void Finished()
        {
            Stream.Write(SubCommand.Finished);
        }

        public void Schema(MetadataSchemaDefinition schema)
        {
            BeginCommand();
            Stream.Write(schema);
            EndCommand();
        }

    }
}
