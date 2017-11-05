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

        public void DatabaseVersion(Guid majorVersion, long minorVersion)
        {
            Stream.Write(SubCommand.DatabaseVersion);
            Stream.Write(majorVersion);
            Stream.Write(minorVersion);
        }


        public void DefineTableRelationship(short tableIndex, short columnIndex, short foreignTableIndex)
        {
            Stream.Write(SubCommand.DefineTableRelationship);
            Stream.Write(tableIndex);
            Stream.Write(columnIndex);
            Stream.Write(foreignTableIndex);
        }


    }
}
