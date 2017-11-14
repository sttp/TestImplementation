using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class MetadataSchema
    {
        public bool IsUpdateResponse;
        public long UpdatedFromRevision;
        public Guid SchemaVersion;
        public long Revision;
        public List<MetadataTables> Tables;
        public List<MetadataTableRelationships> TableRelationships;

        public MetadataSchema(PacketReader reader)
        {
            Tables = new List<MetadataTables>();
            TableRelationships = new List<MetadataTableRelationships>();
            IsUpdateResponse = reader.ReadBoolean();
            UpdatedFromRevision = reader.ReadInt64();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            while (reader.ReadBoolean())
            {
                Tables.Add(new MetadataTables(reader, IsUpdateResponse));
            }
            if (!IsUpdateResponse)
            {
                while (reader.ReadBoolean())
                {
                    TableRelationships.Add(new MetadataTableRelationships(reader));
                }
            }
        }

        public void Save(PacketWriter writer)
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

        public void SaveChanges(PacketWriter writer, Guid oldVersion, long oldRevision)
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

    public class MetadataTables
    {
        public string TableName;
        public long LastModifiedRevision;
        public TableFlags TableFlags;
        public List<Tuple<string, SttpValueTypeCode>> Columns;

        public MetadataTables(PacketReader reader, bool isUpdateResponse)
        {
            TableName = reader.ReadString();
            LastModifiedRevision = reader.ReadInt64();
            if (!isUpdateResponse)
            {
                TableFlags = reader.Read<TableFlags>();
                Columns = reader.ReadList<string, SttpValueTypeCode>();
            }
        }

        public void Save(PacketWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
            writer.Write(TableFlags);
            writer.Write(Columns);
        }

        public void SaveChanges(PacketWriter writer)
        {
            writer.Write(TableName);
            writer.Write(LastModifiedRevision);
        }
    }


    public class MetadataTableRelationships
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

        public MetadataTableRelationships(PacketReader reader)
        {
            TableName = reader.ReadString();
            ColumnName = reader.ReadString();
            ForeignTableName = reader.ReadString();
            ForeignTableColumn = reader.ReadString();
        }

        public void Save(PacketWriter writer)
        {
            writer.Write(TableName);
            writer.Write(ColumnName);
            writer.Write(ForeignTableName);
            writer.Write(ForeignTableColumn);
        }
    }
}