using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.GetMetadataResponse;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void VersionNotCompatible()
        {
            Stream.Write(SubCommand.VersionNotCompatible);
        }

        public void DefineTable(bool isUpdateQuery, Guid schemaVersion, long revision, string tableName, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            Stream.Write(SubCommand.DefineTable);
            Stream.Write(isUpdateQuery);
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

    }
}
