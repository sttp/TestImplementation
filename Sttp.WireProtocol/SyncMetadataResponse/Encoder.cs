using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SyncMetadataResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void Clear()
        {
            Stream.Write(SubCommand.Clear);
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

        public void DefineValue(short tableIndex, short columnIndex, int rowIndex, SttpValue value)
        {
            Stream.Write(SubCommand.DefineValue);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(rowIndex);
            Stream.Write(value);
        }

        public void RemoveRow(short tableIndex, int rowIndex)
        {
            Stream.Write(SubCommand.RemoveRow);
            Stream.Write(tableIndex);
            Stream.Write(rowIndex);
        }

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(majorVersion);
            Stream.Write(minorVersion);
        }

    }
}
