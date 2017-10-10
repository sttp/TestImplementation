using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.Data
{
    public class MetadataSyncTableOrQueryParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.SyncTableOrQuery;
        public Guid MajorVersion;
        public long MinorVersion;
        public List<Tuple<int, int>> ColumnList;
        public List<Tuple<int, int>> CriticalColumnList;
    }
}