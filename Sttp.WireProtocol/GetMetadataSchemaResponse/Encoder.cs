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

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(majorVersion);
            Stream.Write(minorVersion);
        }

    }
}
