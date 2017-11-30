using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.Codec;
using Sttp.Data;

namespace Sttp.Core.Data
{
    public class MetadataQueryExecutionEngine
    {
        private class JoinedTablePath
        {
            public int TableIndex;
            public List<int> JoinIndex = new List<int>();
            public List<bool> IsLeftJoin = new List<bool>();
        }

        private class ColumnsLookedUp
        {
            public int ColumnIndex;
            public int TableIndex;
            public int VariableNumber;

            public ColumnsLookedUp(int columnIndex, int tableIndex, int variableNumber)
            {
                ColumnIndex = columnIndex;
                TableIndex = tableIndex;
                VariableNumber = variableNumber;
            }
        }

        public MetadataQueryExecutionEngine(MetadataDatabaseSource db, CommandGetMetadata command, WireEncoder encoder, SttpQueryStatement query)
        {
            if (query.JoinedTables.Count > 0)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "Indirect table are not supported by this engine");
            if (query.Procedure.Count > 0)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "Procedures are not supported by this engine");
            if (query.WhereBooleanVariable >= 0)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "Boolean where clauses are not supported by this engine");

            query.ValidateAndRemapAllIndexes(out int tableIndexCount, out int variableIndexCount);
            var variables = new SttpValue[variableIndexCount];
            var table = db[query.DirectTable];
            var tables = FindAllTables(db, query, tableIndexCount);
            var inputColumns = FindInputColumns(query, tables);
            var outputColumns = CreateOutputColumns(query, variableIndexCount, inputColumns, tables);

            var joinPath = CreateJoinedTablePath(db, query);

            var send = encoder.MetadataCommandBuilder();
            send.DefineResponse(false, 0, db.SchemaVersion, db.Revision, table.TableName, outputColumns.ToList());

            MetadataRow[] tableRows = new MetadataRow[tableIndexCount];
            foreach (var row in table.Rows)
            {
                TraverseAllJoinsForRows(db, tableRows, row, table, joinPath);

                foreach (var input in query.Literals)
                {
                    variables[input.Variable] = input.Value;
                }
                foreach (var input in inputColumns)
                {
                    variables[input.VariableNumber] = tableRows[input.TableIndex].Fields.Values[input.ColumnIndex];
                }

                SttpValueSet values = new SttpValueSet();
                foreach (var item in query.Outputs)
                {
                    values.Values.Add(variables[item.Variable]);
                }
                send.DefineRow(row.Key, values);
            }
            send.Finished();
            send.EndCommand();
        }

        private void TraverseAllJoinsForRows(MetadataDatabaseSource db, MetadataRow[] tableRows, MetadataRow row, MetadataTable table, JoinedTablePath[] joinPath)
        {
            tableRows[0] = row;
            foreach (var item in joinPath)
            {
                MetadataRow joinedRow = row;
                MetadataTable joinedTable = table;

                for (int x = 0; x < item.JoinIndex.Count; x++)
                {
                    int nextTableIndex = joinedTable.ForeignKeys[item.JoinIndex[x]].TableIndex;
                    int nextRowIndex = joinedRow.ForeignKeys[item.JoinIndex[x]];

                    joinedTable = db.LookupTable(nextTableIndex);
                    joinedRow = joinedTable.LookupRow(nextRowIndex);
                }

                tableRows[item.TableIndex] = joinedRow;
            }
        }


        private static JoinedTablePath[] CreateJoinedTablePath(MetadataDatabaseSource db, SttpQueryStatement query)
        {
            //Export:
            //JoinIndex. This is the Column->Table join index that exists in the table.

            MetadataTable previousTable = db[query.DirectTable];

            var rv = new JoinedTablePath[query.JoinedTables.Count];
            for (int x = 0; x < rv.Length; x++)
            {
                var path = new JoinedTablePath();
                var qry = query.JoinedTables[x];
                path.TableIndex = qry.TableIndex;

                for (int i = 0; i < qry.JoinPath.Count; i++)
                {
                    var qPath = qry.JoinPath[x];
                    path.IsLeftJoin.Add(qPath.JoinType == JoinType.Left);
                    path.JoinIndex.Add(previousTable.ForeignKeys.FindIndex(y => y.ColumnName == qPath.JoinedColumn && y.ForeignTableName == qPath.JoinedTable));
                    previousTable = db[qPath.JoinedTable];
                }
                rv[x] = path;
            }

            return rv;

        }

        private static MetadataColumn[] CreateOutputColumns(SttpQueryStatement query, int variableIndexCount, ColumnsLookedUp[] inputColumns, MetadataTable[] tables)
        {
            var typeCodes = new SttpValueTypeCode[variableIndexCount];
            foreach (var input in query.Literals)
            {
                typeCodes[input.Variable] = input.Value.ValueTypeCode;
            }

            foreach (var input in inputColumns)
            {
                typeCodes[input.VariableNumber] = tables[input.TableIndex].Columns[input.ColumnIndex].TypeCode;
            }

            var rv = new MetadataColumn[query.Outputs.Count];
            for (var x = 0; x < query.Outputs.Count; x++)
            {
                var item = query.Outputs[x];
                rv[x] = new MetadataColumn(item.ColumnName, typeCodes[item.Variable]);
            }
            return rv;

        }

        private static MetadataTable[] FindAllTables(MetadataDatabaseSource db, SttpQueryStatement query, int tableIndexCount)
        {
            var rv = new MetadataTable[tableIndexCount];
            rv[0] = db[query.DirectTable];
            foreach (var item in query.JoinedTables)
            {
                rv[item.TableIndex] = db[item.JoinPath.Last().JoinedTable];
            }
            return rv;
        }

        private static ColumnsLookedUp[] FindInputColumns(SttpQueryStatement query, MetadataTable[] tables)
        {
            var rv = new ColumnsLookedUp[query.ColumnInputs.Count];
            for (var x = 0; x < query.ColumnInputs.Count; x++)
            {
                var column = query.ColumnInputs[x];
                rv[x] = new ColumnsLookedUp(tables[column.TableIndex].Columns.FindIndex(y => y.Name == column.ColumnName), column.TableIndex, column.Variable);
            }
            return rv;
        }
    }
}
