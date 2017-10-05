using System;

namespace Sttp.WireProtocol.Data
{
    public class MetadataSyncTableParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.SyncTable;
        public int TableIndex;
        public Guid MajorVersion;
        public long MinorVersion;
        public int[] ColumnList;
    }
}