using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Sttp.Codec;
using Sttp.Data;

namespace Sttp.Core.Data
{
    public class SttpMetadataServer
    {
        private MetadataRepository m_repository;
        private MetadataRepository m_pendingRepository;

        public SttpMetadataServer()
        {

        }

        public MetadataTable this[string tableName] => m_repository[tableName];


        public void DefineSchema(DataSet dataSet)
        {
            m_pendingRepository = new MetadataRepository(dataSet);
        }

        public void CommitData()
        {
            m_pendingRepository.IsReadOnly = true;
            m_repository = m_pendingRepository;
            m_pendingRepository = null;
        }

        public void FillData(DataSet dataSet)
        {
            m_pendingRepository = m_pendingRepository ?? m_repository.CloneEditable();

            //-------------------------------------
            //--   Fill the data
            //-------------------------------------
            foreach (DataTable table in dataSet.Tables)
            {
                //Validate the schema
                FillData(table.TableName, table);
            }
        }

        public void FillData(string tableName, DataTable table)
        {
            m_pendingRepository = m_pendingRepository ?? m_repository.CloneEditable();
            m_pendingRepository.FillData(tableName,table);

        }

        public void FillData(string tableName, DbDataReader data)
        {
            m_pendingRepository = m_pendingRepository ?? m_repository.CloneEditable();
            m_pendingRepository.FillData(tableName, data);
        }

        public void ProcessCommand(CommandGetMetadataBasic command, WireEncoder encoder)
        {
            var repository = m_repository;
            if (command.SequenceNumber.HasValue && command.SchemaVersion != repository.SchemaVersion)
            {
                encoder.MetadataVersionNotCompatible();
                return;
            }
            var engine = new MetadataQueryExecutionEngine(repository, encoder, command.ToSttpQuery());
        }

        public void ProcessCommand(CommandGetMetadataAdvance command, WireEncoder encoder)
        {
            var repository = m_repository;
            if (command.SequenceNumber.HasValue && command.SchemaVersion != repository.SchemaVersion)
            {
                encoder.MetadataVersionNotCompatible();
                return;
            }
            var engine = new MetadataQueryExecutionEngine(repository, encoder, command);
        }

        public void ProcessCommand(CommandGetMetadataSchema command, WireEncoder encoder)
        {
            var repository = m_repository;
            if (command.SequenceNumber.HasValue && command.SchemaVersion != repository.SchemaVersion)
            {
                encoder.MetadataSchema(repository.SchemaVersion, repository.SequenceNumber, repository.MetadataSchema);
            }
            else
            {
                List<MetadataSchemaTableUpdate> tableRevisions = new List<MetadataSchemaTableUpdate>();
                foreach (var tables in repository.MetadataSchema)
                {
                    tableRevisions.Add(new MetadataSchemaTableUpdate(tables.TableName, tables.LastModifiedSequenceNumber));
                }
                encoder.MetadataSchemaUpdate(repository.SchemaVersion, repository.SequenceNumber, tableRevisions);
            }
        }

    }
}
