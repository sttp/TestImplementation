using System;
using System.Collections.Generic;
using Sttp.Codec;

namespace Sttp
{
    public class MetadataSchemaDefinition
    {
        public bool IsUpdateResponse;
        public long UpdatedFromRevision;
        public Guid SchemaVersion;
        public long Revision;
        public List<MetadataSchemaTables> Tables;

        public MetadataSchemaDefinition()
        {
            Tables = new List<MetadataSchemaTables>();
        }

        public MetadataSchemaDefinition(PayloadReader reader)
        {
            Tables = new List<MetadataSchemaTables>();
            IsUpdateResponse = reader.ReadBoolean();
            UpdatedFromRevision = reader.ReadInt64();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            while (reader.ReadBoolean())
            {
                Tables.Add(new MetadataSchemaTables(reader, IsUpdateResponse));
            }
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(false);
            writer.Write(0L);
            writer.Write(SchemaVersion);
            writer.Write(Revision);
            foreach (var table in Tables)
            {
                writer.Write(true);
                table.Save(writer);
            }
            writer.Write(false);
        }

        public void SaveChanges(PayloadWriter writer, Guid oldVersion, long oldRevision)
        {
            if (SchemaVersion != oldVersion)
            {
                Save(writer);
                return;
            }

            writer.Write(true);
            writer.Write(oldRevision);
            writer.Write(SchemaVersion);
            writer.Write(Revision);
            foreach (var table in Tables)
            {
                if (table.LastModifiedRevision > oldRevision)
                {
                    writer.Write(true);
                    table.SaveChanges(writer);
                }
            }
            writer.Write(false);
        }
    }

    public class MetadataSchemaTables
    {
        public string TableName;
        public long LastModifiedRevision;
        public List<Tuple<string, SttpValueTypeCode>> Columns = new List<Tuple<string, SttpValueTypeCode>>();
        public List<Tuple<string, string>> Relationships = new List<Tuple<string, string>>();

        public MetadataSchemaTables()
        {

        }
        public MetadataSchemaTables(PayloadReader reader, bool isUpdateResponse)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
            if (!isUpdateResponse)
            {
                Columns = reader.ReadList<string, SttpValueTypeCode>();
                Relationships = reader.ReadList<string, string>();
            }
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
            writer.Write(Columns);
        }

        public void SaveChanges(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
        }
    }
}