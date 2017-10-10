using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataAddTableParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.AddTable;
        public int TableIndex;
        public string TableName;
        public TableFlags TableFlags;
    }
}