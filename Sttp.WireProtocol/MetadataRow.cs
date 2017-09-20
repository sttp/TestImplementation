using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataRow
    {
        public MetadataTable Table;
        public Guid RowID;
        public bool Deleted; //When versioning metadata, this provides a way to indicate that a row was removed entirely.
        public List<MetadataField> Fields;

        public void AddField(MetadataColumn column, object value)
        {
            Fields.Add(new MetadataField(column, value));
        }
    }
}