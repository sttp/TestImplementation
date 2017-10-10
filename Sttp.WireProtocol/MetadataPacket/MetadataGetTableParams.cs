using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.Data
{
    public class MetadataGetTableParams : IMetadataParams
    {
        public MetadataCommand Command => MetadataCommand.GetTable;
        public int TableIndex;
        public int[] ColumnList;
        public List<Tuple<int,string>> FilterExpression;
    }
}