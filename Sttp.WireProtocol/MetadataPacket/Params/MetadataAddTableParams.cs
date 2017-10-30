using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataAddTableParams : IMetadataParams
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.AddTable;
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