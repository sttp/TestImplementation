using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class MetadataSchema
    {
        public Guid SchemaVersion;
        public long Revision;
        public bool IncludesSchema;

        public List<MetadataTables> Tables;
        public List<MetadataTableRelationships> TableRelationships;

        public MetadataSchema(PacketReader reader)
        {
            Tables = new List<MetadataTables>();
            TableRelationships = new List<MetadataTableRelationships>();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            IncludesSchema = reader.ReadBoolean();
            if (IncludesSchema)
            {
                int cnt = reader.ReadInt32();
                while (cnt > 0)
                {
                    cnt--;
                    Tables.Add(new MetadataTables(reader));
                }

                cnt = reader.ReadInt32();
                while (cnt > 0)
                {
                    cnt--;
                    TableRelationships.Add(new MetadataTableRelationships(reader));
                }
            }
        }

        public void Save(PacketWriter writer, bool includeSchema)
        {
            writer.Write(SchemaVersion);
            writer.Write(Revision);
            writer.Write(includeSchema);
            if (includeSchema)
            {
                writer.Write(Tables.Count);
                foreach (var table in Tables)
                {
                    table.Save(writer);
                }
                writer.Write(TableRelationships.Count);
                foreach (var relationship in TableRelationships)
                {
                    relationship.Save(writer);
                }
            }
        }

    }


    public class MetadataTables
    {
        public string TableName;
        public TableFlags TableFlags;
        public List<Tuple<string, SttpValueTypeCode>> Columns;

        public MetadataTables(PacketReader reader)
        {
            TableName = reader.ReadString();
            TableFlags = reader.Read<TableFlags>();
            Columns = reader.ReadList<string, SttpValueTypeCode>();
        }

        public void Save(PacketWriter writer)
        {
            writer.Write(TableName);
            writer.Write(TableFlags);
            writer.Write(Columns);
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