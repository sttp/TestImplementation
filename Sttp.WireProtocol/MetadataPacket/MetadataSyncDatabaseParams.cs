using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.Data
{
    public class MetadataSyncDatabaseParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.SyncDatabase;
        public Guid MajorVersion;
        public long MinorVersion;
        public List<Tuple<int, int>> ColumnList;
    }
}