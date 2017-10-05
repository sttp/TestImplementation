using System;
using Sttp.WireProtocol.MetadataPacket;

namespace Sttp.WireProtocol.Data
{
    public class MetadataAddTableParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.AddTable;
        public Guid MajorVersion;
        public long MinorVersion;
        public string TableName;
        public TableFlags TableFlags;
    }
}