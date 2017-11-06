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

        public void DefineTable(short tableIndex, string tableName, TableFlags tableFlags)
        {
            Stream.Write(SubCommand.DefineTable);
            Stream.Write(tableIndex);
            Stream.Write(tableName);
            Stream.Write(tableFlags);
        }

        public void DefineColumn(short tableIndex, short columnIndex, string columnName, byte columnTypeCode)
        {
            Stream.Write(SubCommand.DefineColumn);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(columnName);
            Stream.Write(columnTypeCode);
        }

        public void DefineRow(short tableIndex, int rowIndex)
        {
            Stream.Write(SubCommand.DefineRow);
            Stream.Write(tableIndex);
            Stream.Write(rowIndex);
        }

        public void RemoveRow(short tableIndex, int rowIndex)
        {
            Stream.Write(SubCommand.UndefineRow);
            Stream.Write(tableIndex);
            Stream.Write(rowIndex);
        }

        public void DatabaseVersion(Guid schemaVersion, long revision)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(schemaVersion);
            Stream.Write(revision);
        }

    }
}
