using System;
using System.Collections.Generic;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataDatabaseSource
    {
        private MetadataSchemaDefinition m_metadataSchema = new MetadataSchemaDefinition();

        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid SchemaVersion { get; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long Revision { get; }

        private Dictionary<string, MetadataTable> m_tables;

        public MetadataDatabaseSource()
        {
            m_tables = new Dictionary<string, MetadataTable>();
        }

        public MetadataTable this[string tableName]
        {
            get
            {
                return m_tables[tableName];
            }
        }

        public void DefineTable(string tableName, TableFlags flags, List<Tuple<string,SttpValueTypeCode>> columns)
        {
            if (!m_tables.ContainsKey(tableName))
            {
                var tbl = new MetadataTable(tableName, flags, columns);
                m_tables.Add(tableName, tbl);
            }
        }

        public void AddOrUpdateRow(string tableName, SttpValue key, SttpValueSet fields )
        {
            m_tables[tableName].AddOrUpdateRow(key, fields);
        }

        public void DeleteRow(string tableName, SttpValue key)
        {
            m_tables[tableName].DeleteRow(key);
        }

        public void RefreshSchema()
        {
            m_metadataSchema = new MetadataSchemaDefinition();
            foreach (var table in m_tables.Values)
            {
                var t = new MetadataSchemaTables();
                t.TableName = table.TableName;
                t.LastModifiedRevision = table.LastModifiedRevision;
                foreach (var col in table.Columns)
                {
                    t.Columns.Add(Tuple.Create(col.Name, col.TypeCode));
                }
                t.TableFlags = table.TableFlags;
                m_metadataSchema.Tables.Add(t);
            }
            m_metadataSchema.SchemaVersion = SchemaVersion;
            m_metadataSchema.Revision = Revision;
        }

        public void ProcessCommand(WireProtocol.GetMetadataSchema.Decoder command, WireProtocol.WireEncoder encoder)
        {
            encoder.GetMetadataSchemaResponse.GetMetadataSchemaResponse(m_metadataSchema);
            if (command.SchemaVersion != SchemaVersion && command.SchemaVersion != Guid.Empty)
            {

            }
        }
        public void ProcessCommand(WireProtocol.GetMetadata.Decoder command, WireProtocol.WireEncoder encoder)
        {

        }


    }

}
