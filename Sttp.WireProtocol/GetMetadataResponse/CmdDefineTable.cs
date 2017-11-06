using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDefineTable : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineTable;
        public string TableName;
        public TableFlags TableFlags;
        public List<Tuple<string, SttpValueTypeCode>> Columns;

        public void Load(PacketReader reader)
        {
            TableName = reader.ReadString();
            TableFlags = reader.Read<TableFlags>();
            Columns = reader.ReadList<string, SttpValueTypeCode>();
        }
    }
}