using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDefineTable : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineTable;

        public Guid SchemaVersion;
        public long Revision;
        public string TableName;
        public List<Tuple<string, SttpValueTypeCode>> Columns;

        public void Load(PacketReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            TableName = reader.ReadString();
            Columns = reader.ReadList<string, SttpValueTypeCode>();
        }
    }
}