using System;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDefineTable : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineTable;
        public short TableIndex;
        public string TableName;
        public TableFlags TableFlags;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            TableName = reader.ReadString();
            TableFlags = reader.Read<TableFlags>();
        }
    }
}