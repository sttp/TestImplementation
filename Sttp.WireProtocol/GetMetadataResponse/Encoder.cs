using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void VersionNotCompatible()
        {
            Stream.Write(SubCommand.VersionNotCompatible);
        }

        public void DefineTable(string tableName, TableFlags tableFlags, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            Stream.Write(SubCommand.DefineTable);
            Stream.Write(tableName);
            Stream.Write(tableFlags);
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

        public void DatabaseVersion(Guid schemaVersion, long revision)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(schemaVersion);
            Stream.Write(revision);
        }

    }
}
