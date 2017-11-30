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
            public int JoinIndex;
            public int NewTableIndex;
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

        private class Procedure
        {
            public MetadataFunctions Function;

            public Procedure(SttpQueryProcedureStep step)
            {
                switch (step.Function)
                {
                    case "MUL":
                        Function = new FuncMultiply(step.InputVariables.ToArray(), step.OutputVariable);
                        break;
                    case "EQU":
                        Function = new FuncEquals(step.InputVariables.ToArray(), step.OutputVariable);
                        break;
                    default:
                        throw new Exception("Function does not exist");
                }
            }
        }


        public MetadataQueryExecutionEngine(MetadataDatabaseSource db, CommandGetMetadata command, WireEncoder encoder, SttpQueryStatement query)
        {
            if (query.HavingProcedure.Count > 0)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "HAVING clauses are not supported by this engine");
            if (query.HavingBooleanVariable >= 0)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "HAVING clauses are not supported by this engine");
            if (query.GroupByVariables.Count > 0)
                encoder.RequestFailed(CommandCode.GetMetadata, false, "Query Not Supported", "GROUP BY clauses are not supported by this engine");

            query.ValidateAndRemapAllIndexes(out int variableIndexCount, out int tableIndexCount);

            var variables = new SttpValue[variableIndexCount];
            var tables = FindAllTables(db, query, tableIndexCount);
            var inputColumns = MapInputColumns(query, tables);
            var procedures = CompileProcedures(query);
            var outputColumns = MapOutputColumns(query, variableIndexCount, inputColumns, tables, procedures);
            var joinPath = MapAllJoins(db, query);
            var send = encoder.MetadataCommandBuilder();
            send.DefineResponse(false, 0, db.SchemaVersion, db.Revision, tables[0].TableName, outputColumns.ToList());

            MetadataRow[] tableRows = new MetadataRow[tableIndexCount];
            foreach (var row in tables[0].Rows)
            {
                TraverseAllJoinsForRows(db, tableRows, row, joinPath, tables);

                foreach (var input in query.Literals)
                {
                    variables[input.Variable] = input.Value;
                }
                foreach (var input in inputColumns)
                {
                    var r = tableRows[input.TableIndex];
                    if (r == null)
                    {
                        variables[input.VariableNumber] = new SttpValue();
                    }
                    else
                    {
                        variables[input.VariableNumber] = r.Fields.Values[input.ColumnIndex];
                    }
                }

                foreach (var procedure in procedures)
                {
                    procedure.Function.Execute(variables);
                }

                if (!query.WhereBooleanVariable.HasValue 
                    || (!variables[query.WhereBooleanVariable.Value].IsNull && variables[query.WhereBooleanVariable.Value].AsBool))
                {
                    SttpValueSet values = new SttpValueSet();
                    foreach (var item in query.Outputs)
                    {
                        values.Values.Add(variables[item.Variable]);
                    }
                    send.DefineRow(row.Key, values);
                }

                
            }
            send.Finished();
            send.EndCommand();
        }

        private void TraverseAllJoinsForRows(MetadataDatabaseSource db, MetadataRow[] tableRows, MetadataRow row, JoinedTablePath[] joinPath, MetadataTable[] tables)
        {
            tableRows[0] = row;
            foreach (var item in joinPath)
            {
                MetadataRow joinedRow = tableRows[item.TableIndex];
                if (joinedRow != null)
                {
                    MetadataTable joinedTable = tables[item.TableIndex];

                    int nextTableIndex = joinedTable.ForeignKeys[item.JoinIndex].TableIndex;
                    int nextRowIndex = joinedRow.ForeignKeys[item.JoinIndex];
                    joinedTable = db.LookupTable(nextTableIndex);

                    if (nextRowIndex >= 0)
                    {
                        joinedRow = joinedTable.LookupRow(nextRowIndex);
                        tableRows[item.NewTableIndex] = joinedRow;
                    }
                    else
                    {
                        tableRows[item.NewTableIndex] = null;
                    }
                }
                else
                {
                    tableRows[item.NewTableIndex] = null;
                }
            }
        }


        private static JoinedTablePath[] MapAllJoins(MetadataDatabaseSource db, SttpQueryStatement query)
        {
            //Export:
            //JoinIndex. This is the Column->Table join index that exists in the table.

            MetadataTable previousTable = db[query.DirectTable];

            var rv = new JoinedTablePath[query.JoinedTables.Count];
            for (int x = 0; x < rv.Length; x++)
            {
                var path = new JoinedTablePath();
                var qry = query.JoinedTables[x];
                path.TableIndex = qry.ExistingTableIndex;
                path.JoinIndex = previousTable.ForeignKeys.FindIndex(y => y.ColumnName == qry.ExistingForeignKeyColumn && y.ForeignTableName == qry.ForeignTable);
                path.NewTableIndex = qry.ForeignTableIndex;
                rv[x] = path;
            }

            return rv;

        }

        private static MetadataColumn[] MapOutputColumns(SttpQueryStatement query, int variableIndexCount, ColumnsLookedUp[] inputColumns, MetadataTable[] tables, Procedure[] procedures)
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

            foreach (var procedure in procedures)
            {
                procedure.Function.PropagateType(typeCodes);
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
                rv[item.ForeignTableIndex] = db[item.ForeignTable];
            }
            return rv;
        }

        private static ColumnsLookedUp[] MapInputColumns(SttpQueryStatement query, MetadataTable[] tables)
        {
            var rv = new ColumnsLookedUp[query.ColumnInputs.Count];
            for (var x = 0; x < query.ColumnInputs.Count; x++)
            {
                var column = query.ColumnInputs[x];
                rv[x] = new ColumnsLookedUp(tables[column.TableIndex].Columns.FindIndex(y => y.Name == column.ColumnName), column.TableIndex, column.Variable);
            }
            return rv;
        }

        private static Procedure[] CompileProcedures(SttpQueryStatement query)
        {
            var rv = new Procedure[query.Procedure.Count];
            for (var x = 0; x < query.Procedure.Count; x++)
            {
                rv[x] = new Procedure(query.Procedure[x]);
            }
            return rv;
        }

    }
}
