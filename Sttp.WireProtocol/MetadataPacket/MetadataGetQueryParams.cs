using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.Data
{
    public class MetadataGetQueryParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.GetQuery;
        public int TableIndex;
        public List<Tuple<int, int>> ColumnList;
        public List<Tuple<int, int, int>> JoinFields;
        public List<Tuple<int, int, string>> FilterExpression;
    }
}