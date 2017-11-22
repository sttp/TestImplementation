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
        public Guid SchemaVersion { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long Revision { get; private set; }

        private Dictionary<string, MetadataTable> m_tables;

        private Dictionary<string, MetadataTable> m_pendingChanges;

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

        public void AddOrReplaceTable(string tableName, TableFlags flags, List<Tuple<string, SttpValueTypeCode>> columns)
        {
            if (m_pendingChanges == null)
                m_pendingChanges = new Dictionary<string, MetadataTable>(m_tables);
            m_pendingChanges[tableName] = new MetadataTable(tableName, flags, columns);
        }

        public void AddOrReplaceRow(string tableName, SttpValue key, SttpValueSet fields)
        {
            if (m_pendingChanges != null)
            {
                m_pendingChanges[tableName].AddOrUpdateRow(key, fields);
            }
            else
            {
                m_tables[tableName].AddOrUpdateRow(key, fields);
            }
        }

        public void DeleteRow(string tableName, SttpValue key)
        {
            if (m_pendingChanges != null)
            {
                m_pendingChanges[tableName].DeleteRow(key);
            }
            else
            {
                m_tables[tableName].DeleteRow(key);
            }
        }

        public void RollbackChanges()
        {
            m_pendingChanges = null;
            foreach (var tables in m_tables.Values)
            {
                tables.RollbackChanges();
            }
        }

        public void CommitChanges()
        {
            if (m_pendingChanges != null)
            {
                SchemaVersion = Guid.NewGuid();
                Revision = 0;

                foreach (var tables in m_pendingChanges.Values)
                {
                    tables.CommitChanges(Revision, true);
                }
                m_tables = m_pendingChanges;
                m_pendingChanges = null;
            }
            else
            {
                foreach (var tables in m_tables.Values)
                {

                    tables.CommitChanges(Revision, false);
                }
            }
            RefreshSchema();
        }

        private void RefreshSchema()
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

        //public void ProcessCommand(WireProtocol.GetMetadataSchema.Decoder command, WireProtocol.WireEncoder encoder)
        //{
        //    encoder.GetMetadataSchemaResponse.GetMetadataSchemaResponse(m_metadataSchema);
        //    if (command.SchemaVersion != SchemaVersion && command.SchemaVersion != Guid.Empty)
        //    {

        //    }
        //}
        //public void ProcessCommand(WireProtocol.GetMetadata.Decoder command, WireProtocol.WireEncoder encoder)
        //{

        //}


    }

}
