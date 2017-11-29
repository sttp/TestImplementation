using System;
using System.Collections.Generic;
using Sttp.Codec;

namespace Sttp
{
    public class MetadataSchemaTables
    {
        public string TableName;
        public long LastModifiedRevision;
        public List<MetadataColumn> Columns = new List<MetadataColumn>();
        public List<MetadataForeignKey> ForeignKeys = new List<MetadataForeignKey>();

        public MetadataSchemaTables()
        {

        }

        public MetadataSchemaTables(PayloadReader reader)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
            Columns = reader.ReadList<MetadataColumn>();
            ForeignKeys = reader.ReadList<MetadataForeignKey>();
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
            writer.Write(Columns);
            writer.Write(ForeignKeys);
        }
    }
}