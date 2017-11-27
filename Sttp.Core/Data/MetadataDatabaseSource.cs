using System;
using System.Collections.Generic;
using System.Linq;
using Sttp.Codec;
using Sttp.WireProtocol;

namespace Sttp.Data
{
    /// <summary>
    /// The set of all metadata used by the protocol.
    /// </summary>
    public class MetadataDatabaseSource
    {
        /// <summary>
        /// If logging is enabled, this is the ID for the transaction log.
        /// </summary>
        public Guid SchemaVersion { get; private set; }

        /// <summary>
        /// This identifies the transaction number of the supplied log. 
        /// </summary>
        public long Revision { get; private set; }

        private Dictionary<string, int> m_tablesLookup;
        private List<MetadataTable> m_tables;

        private bool m_isReadOnly;
        private List<MetadataSchemaTables> m_metadataSchema;

        /// <summary>
        /// Gets/Sets the readonly nature of this class.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return m_isReadOnly;
            }
            set
            {
                if (m_isReadOnly && !value) //True, changing to false
                    throw new Exception("This class is immutable");
                if (!m_isReadOnly && value) //False changing to true
                {
                    foreach (var metadataTable in m_tables)
                    {
                        metadataTable.SetIsReadOnly(Revision, TableLookup, LookupForeignKey);
                    }
                    RefreshSchema();
                    m_isReadOnly = true;
                }
            }
        }

        private int TableLookup(string arg)
        {
            if (m_tablesLookup.TryGetValue(arg, out int index))
            {
                return index;
            }
            return -1;
        }

        private int LookupForeignKey(int i, SttpValue sttpValue)
        {
            return m_tables[i].LookupRowIndex(sttpValue);
        }

        public MetadataDatabaseSource()
        {
            m_tablesLookup = new Dictionary<string, int>();
            m_tables = new List<MetadataTable>();
        }

        private MetadataDatabaseSource(MetadataDatabaseSource other)
        {
            m_tablesLookup = new Dictionary<string, int>(other.m_tablesLookup);
            m_tables = other.m_tables.Select(x => x.CloneEditable()).ToList();
            SchemaVersion = other.SchemaVersion;
            Revision = other.Revision + 1;
        }

        public MetadataTable this[string tableName]
        {
            get
            {
                return m_tables[m_tablesLookup[tableName]];
            }
        }

        public void AddOrReplaceTable(string tableName, TableFlags tableFlags, List<MetadataColumn> columns, List<MetadataForeignKey> tableRelationships)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            SchemaVersion = Guid.NewGuid();
            Revision = 0;
            int index = m_tablesLookup[tableName];
            m_tables[index] = new MetadataTable(tableName, tableFlags, columns, tableRelationships);
        }

        public void AddOrReplaceRow(string tableName, SttpValue key, SttpValueSet fields)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            int index = m_tablesLookup[tableName];
            m_tables[index].AddOrUpdateRow(key, fields, Revision);
        }

        public void DeleteRow(string tableName, SttpValue key)
        {
            if (IsReadOnly)
                throw new Exception("This class is immutable");
            int index = m_tablesLookup[tableName];
            m_tables[index].DeleteRow(key, Revision);
        }

        public MetadataDatabaseSource CloneEditable()
        {
            return new MetadataDatabaseSource(this);
        }

        private void RefreshSchema()
        {
            m_metadataSchema = new List<MetadataSchemaTables>();
            foreach (var table in m_tables)
            {
                var t = new MetadataSchemaTables();
                t.TableName = table.TableName;
                t.LastModifiedRevision = table.LastModifiedRevision;
                foreach (var col in table.Columns)
                {
                    t.Columns.Add(Tuple.Create(col.Name, col.TypeCode));
                }
                t.TableFlags = table.TableFlags;
                m_metadataSchema.Add(t);
            }
        }

        public void ProcessCommand(CommandGetMetadata command, WireEncoder encoder)
        {
            if (command.SchemaVersion != SchemaVersion &&
                (command.AreUpdateQueries || command.SchemaVersion != Guid.Empty))
            {
                encoder.MetadataVersionNotCompatible();
                return;
            }

            foreach (var query in command.Queries)
            {
                ProcessQuery(command, encoder, query);
            }
        }

        private void ProcessQuery(CommandGetMetadata command, WireEncoder encoder, SttpQueryExpression query)
        {
            if (query.Tables.Count != 1)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "Cannot parse a query unless it has exactly 1 table.");
            if (query.Joins.Count != 1)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "Joins aren't supported yet");
            if (query.Procedures.Count != 1)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "Procedures aren't supported yet");

            var table = m_tables[TableLookup(query.Tables[0].TableName)];
            Dictionary<int, SttpValue> variables = new Dictionary<int, SttpValue>();

            List<Tuple<int, int>> columnIndexes = new List<Tuple<int, int>>();
            foreach (var column in query.ColumnInputs)
            {
                columnIndexes.Add(Tuple.Create(column.VariableIndex, table.Columns.FindIndex(x => x.Name == column.ColumnName)));
            }

            var send = encoder.MetadataCommandBuilder();

            send.DefineResponse(false, 0, SchemaVersion, Revision, table.TableName, query.Outputs.Select(x => Tuple.Create(x.ColumnName, x.ColumnType)).ToList());

            foreach (var row in table.Rows)
            {
                foreach (var input in query.ValueInputs)
                {
                    variables[input.VariableIndex] = input.Value;
                }
                foreach (var input in columnIndexes)
                {
                    variables[input.Item1] = row.Fields.Values[input.Item2];
                }

                SttpValueSet values = new SttpValueSet();
                foreach (var item in query.Outputs)
                {
                    values.Values.Add(variables[item.VariableIndex]);
                }
                send.DefineRow(row.Key, values);
            }

        }

        public void ProcessCommand(CommandGetMetadataSchema command, WireEncoder encoder)
        {
            if (command.SchemaVersion != SchemaVersion)
            {
                encoder.MetadataSchema(SchemaVersion, Revision, m_metadataSchema);
            }
            else
            {
                List<Tuple<string, long>> tableRevisions = new List<Tuple<string, long>>();
                foreach (var tables in m_metadataSchema)
                {
                    if (tables.LastModifiedRevision > command.Revision)
                    {
                        tableRevisions.Add(Tuple.Create(tables.TableName, tables.LastModifiedRevision));
                    }
                }
                encoder.MetadataSchemaUpdate(SchemaVersion, Revision, command.Revision, tableRevisions);
            }
        }


    }

}
