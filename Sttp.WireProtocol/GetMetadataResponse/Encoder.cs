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

        public void Clear()
        {
            Stream.Write(SubCommand.Clear);
        }

        public void AddTable(short tableIndex, string tableName, TableFlags tableFlags)
        {
            Stream.Write(SubCommand.AddTable);
            Stream.Write(tableIndex);
            Stream.Write(tableName);
            Stream.Write(tableFlags);
        }

        public void AddColumn(short tableIndex, short columnIndex, string columnName, byte columnTypeCode)
        {
            Stream.Write(SubCommand.AddColumn);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(columnName);
            Stream.Write(columnTypeCode);
        }

        public void AddRow(short tableIndex, int rowIndex)
        {
            Stream.Write(SubCommand.AddRow);
            Stream.Write(tableIndex);
            Stream.Write(rowIndex);
        }

        public void AddValue(short tableIndex, short columnIndex, int rowIndex, SttpValue value)
        {
            Stream.Write(SubCommand.AddValue);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(rowIndex);
            Stream.Write(value);
        }

        public void DeleteRow(short tableIndex, int rowIndex)
        {
            Stream.Write(SubCommand.DeleteRow);
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
