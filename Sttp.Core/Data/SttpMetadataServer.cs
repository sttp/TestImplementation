using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Sttp.Codec;
using Sttp.Data;

namespace Sttp.Core.Data
{
    public class SttpMetadataServer
    {
        private MetadataRepository m_repository;
        private MetadataRepository m_pendingRepository;
        private Dictionary<string, IMetadataProcedureHandler> m_procedureHandlers;

        public SttpMetadataServer()
        {
            m_procedureHandlers = new Dictionary<string, IMetadataProcedureHandler>();
        }

        public MetadataTable this[string tableName] => m_repository[tableName];

        public void RegisterProcedureHandler(string procedureName, IMetadataProcedureHandler handler)
        {
            m_procedureHandlers[procedureName] = handler;
        }

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
            m_pendingRepository.FillData(tableName, table);
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

        public void ProcessCommand(CommandGetMetadataProcedure command, WireEncoder encoder)
        {
            if (m_procedureHandlers.TryGetValue(command.ProcedureName, out IMetadataProcedureHandler handler))
            {
                using (var rdr = handler.ProcessRequest(command.Options))
                {
                    int fieldCount = rdr.FieldCount;
                    var col = new List<MetadataColumn>(fieldCount);
                    for (var x = 0; x < fieldCount; x++)
                    {
                        col.Add(new MetadataColumn(rdr.GetName(x), SttpValueTypeCodec.FromType(rdr.GetFieldType(x))));
                    }

                    var wr = encoder.MetadataCommandBuilder();
                    wr.DefineResponse(Guid.Empty, 0, null, command.ProcedureName, col);
                    object[] row = new object[fieldCount];
                    while (rdr.Read())
                    {
                        if (rdr.GetValues(row) != fieldCount)
                            throw new Exception("Field count did not match");

                        List<SttpValue> values = new List<SttpValue>();
                        values.AddRange(row.Select(SttpValue.FromObject));

                        wr.DefineRow(null, values);
                    }
                    wr.Finished();
                    wr.EndCommand();
                }
            }
            else
            {
                encoder.RequestFailed("GetMetadataProcedure", false, "Procedure handler does not exist on the server.", command.ProcedureName);
            }
        }

    }
}
