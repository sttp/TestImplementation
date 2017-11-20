using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataSchemaDefinition
    {
        public bool IsUpdateResponse;
        public long UpdatedFromRevision;
        public Guid SchemaVersion;
        public long Revision;
        public List<MetadataSchemaTables> Tables;
        public List<MetadataSchemaTableRelationships> TableRelationships;

        public MetadataSchemaDefinition()
        {
            Tables = new List<MetadataSchemaTables>();
            TableRelationships = new List<MetadataSchemaTableRelationships>();
        }

        public MetadataSchemaDefinition(PayloadReader reader)
        {
            Tables = new List<MetadataSchemaTables>();
            TableRelationships = new List<MetadataSchemaTableRelationships>();
            IsUpdateResponse = reader.ReadBoolean();
            UpdatedFromRevision = reader.ReadInt64();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            while (reader.ReadBoolean())
            {
                Tables.Add(new MetadataSchemaTables(reader, IsUpdateResponse));
            }
            if (!IsUpdateResponse)
            {
                while (reader.ReadBoolean())
                {
                    TableRelationships.Add(new MetadataSchemaTableRelationships(reader));
                }
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
            writer.Write(TableRelationships.Count);
            foreach (var relationship in TableRelationships)
            {
                writer.Write(true);
                relationship.Save(writer);
            }
            writer.Write(true);
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
        public TableFlags TableFlags;
        public List<Tuple<string, SttpValueTypeCode>> Columns;

        public MetadataSchemaTables()
        {
            
        }
        public MetadataSchemaTables(PayloadReader reader, bool isUpdateResponse)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
            if (!isUpdateResponse)
            {
                TableFlags = reader.Read<TableFlags>();
                Columns = reader.ReadList<string, SttpValueTypeCode>();
            }
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
            writer.Write(TableFlags);
            writer.Write(Columns);
        }

        public void SaveChanges(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
        }
    }


    public class MetadataSchemaTableRelationships
    {
        /// <summary>
        /// The table that has the column with the foreign key.
        /// </summary>
        public string TableName;
        /// <summary>
        /// The name of the column with the foreign key.
        /// </summary>
        public string ColumnName;
        /// <summary>
        /// The foreign table that has the key. 
        /// It could be itself of course.
        /// </summary>
        public string ForeignTableName;
        /// <summary>
        /// The foreign table that has the key. 
        /// It could be itself of course.
        /// </summary>
        public string ForeignTableColumn;

        public MetadataSchemaTableRelationships(PayloadReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            ForeignTableName = reader.ReadString();
            ForeignTableColumn = reader.ReadString();
        }

        public void Save(PayloadWriter writer)
        {
            writer.Write(TableName);
            writer.Write(ColumnName);
            writer.Write(ForeignTableName);
            writer.Write(ForeignTableColumn);
        }
    }
}