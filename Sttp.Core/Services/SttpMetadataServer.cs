using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using CTP;
using CTP.Net;
using Sttp.Codec;
using Sttp.Core;
using Sttp.Core.Data;
using Sttp.Data;

namespace Sttp.Services
{
    public class SttpMetadataServer
    {
        private MetadataRepository m_repository;
        private MetadataRepository m_pendingRepository;
        private Dictionary<string, IMetadataProcedureHandler> m_procedureHandlers;

        public SttpMetadataServer()
        {
            m_procedureHandlers = new Dictionary<string, IMetadataProcedureHandler>();
            m_repository = new MetadataRepository();
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

        public void ProcessCommand(CommandGetMetadata command, CtpSession encoder)
        {
            var repository = m_repository;
            if (repository.ContainsTable(command.Table))
            {
                var table = repository[command.Table];
                var columns = new List<MetadataColumn>();
                var columnIndexes = new List<int>();
                if (command.Columns == null || command.Columns.Count == 0)
                {
                    columns.AddRange(table.Columns);
                    for (int x = 0; x < columns.Count; x++)
                    {
                        columnIndexes.Add(x);
                    }
                }
                else
                {
                    foreach (var column in command.Columns)
                    {
                        var c = table.Columns.FindIndex(x => x.Name == column);
                        if (c < 0)
                        {
                            encoder.Send(new CommandMetadataRequestFailed("Syntax Error", "Could not find the specified column " + column));
                            return;
                        }
                        columnIndexes.Add(c);
                        columns.Add(table.Columns[c]);
                    }
                }

                byte channelID = 1;
                int rowCount = 0;

                var rowEncoder = new MetadataRowEncoder(columns);
                CtpObject[] values = new CtpObject[columns.Count];
                for (int x = 0; x < values.Length; x++)
                {
                    values[x] = new CtpObject();
                }
                encoder.Send(new CommandBeginMetadataResponse(channelID, Guid.Empty, repository.RuntimeID, repository.VersionNumber, command.Table, columns));
                foreach (var row in table.Rows)
                {
                    for (int x = 0; x < values.Length; x++)
                    {
                        values[x].SetValue(row.Fields[columnIndexes[x]]);
                    }
                    rowEncoder.AddRow(values);
                    if (rowEncoder.Size > 30000)
                    {
                        throw new NotImplementedException();
                        //encoder.SendRaw(rowEncoder.ToArray(), channelID);
                        rowEncoder.Clear();
                    }

                    rowCount++;
                }
                if (rowEncoder.Size > 0)
                {
                    throw new NotImplementedException();
                    //encoder.SendRaw(rowEncoder.ToArray(), channelID);
                    rowEncoder.Clear();
                }
                encoder.Send(new CommandEndMetadataResponse(channelID, rowCount));
            }
            else
            {
                encoder.Send(new CommandMetadataRequestFailed("Syntax Error", "Could not find the specified table " + command.Table));
            }
        }

        //public void ProcessCommand(CommandGetMetadataAdvance command, WireEncoder encoder)
        //{
        //    var repository = m_repository;
        //    if (command.SequenceNumber.HasValue && command.SchemaVersion != repository.SchemaVersion)
        //    {
        //        encoder.MetadataVersionNotCompatible();
        //        return;
        //    }
        //    var engine = new MetadataQueryExecutionEngine(repository, encoder, command);
        //}

        public void ProcessCommand(CommandGetMetadataSchema command, CtpSession encoder)
        {
            var repository = m_repository;
            if (!command.LastKnownRuntimeID.HasValue || command.LastKnownRuntimeID != repository.RuntimeID)
            {
                encoder.Send(new CommandMetadataSchema(repository.RuntimeID, repository.VersionNumber, repository.MetadataSchema));
            }
            else if (command.LastKnownVersionNumber != repository.VersionNumber)
            {
                List<MetadataSchemaTableUpdate> tableRevisions = new List<MetadataSchemaTableUpdate>();
                foreach (var tables in repository.MetadataSchema)
                {
                    tableRevisions.Add(new MetadataSchemaTableUpdate(tables.TableName, tables.LastModifiedVersionNumber));
                }
                encoder.Send(new CommandMetadataSchemaUpdate(repository.RuntimeID, repository.VersionNumber, tableRevisions));
            }
            else
            {
                encoder.Send(new CommandMetadataSchemaVersion(repository.RuntimeID, repository.VersionNumber));
            }
        }

        public CommandMetadataSchema GetMetadataSchema()
        {
            var repository = m_repository;
            return new CommandMetadataSchema(repository.RuntimeID, repository.VersionNumber, repository.MetadataSchema);
        }

        //public void ProcessCommand(CommandGetMetadataProcedure command, WireEncoder encoder)
        //{
        //    if (m_procedureHandlers.TryGetValue(command.ProcedureName, out IMetadataProcedureHandler handler))
        //    {
        //        using (var rdr = handler.ProcessRequest(command.Options))
        //        {
        //            int fieldCount = rdr.FieldCount;
        //            var col = new List<MetadataColumn>(fieldCount);
        //            for (var x = 0; x < fieldCount; x++)
        //            {
        //                col.Add(new MetadataColumn(rdr.GetName(x), SttpValueTypeCodec.FromType(rdr.GetFieldType(x))));
        //            }

        //            var wr = encoder.MetadataCommandBuilder();
        //            wr.DefineResponse(Guid.Empty, 0, null, command.ProcedureName, col);
        //            object[] row = new object[fieldCount];
        //            while (rdr.Read())
        //            {
        //                if (rdr.GetValues(row) != fieldCount)
        //                    throw new Exception("Field count did not match");

        //                List<SttpValue> values = new List<SttpValue>();
        //                values.AddRange(row.Select(SttpValue.FromObject));

        //                wr.DefineRow(null, values);
        //            }
        //            wr.Finished();
        //            wr.EndCommand();
        //        }
        //    }
        //    else
        //    {
        //        encoder.RequestFailed("GetMetadataProcedure", false, "Procedure handler does not exist on the server.", command.ProcedureName);
        //    }
        //}

    }
}
