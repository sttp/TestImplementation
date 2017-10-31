using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class CmdAddTable : ICmd
    {
        public SubCommand SubCommand => SubCommand.AddTable;
        public short TableIndex;
        public string TableName;
        public TableFlags TableFlags;

        public void Load(PacketReader reader)
        {
            TableIndex = reader.ReadInt16();
            TableName = reader.ReadString();
            TableFlags = reader.Read<TableFlags>();
        }

        CmdDatabaseVersion ICmd.DatabaseVersion => null;
        CmdAddColumn ICmd.AddColumn => null;
        CmdAddTable ICmd.AddTable => this;
    }
}