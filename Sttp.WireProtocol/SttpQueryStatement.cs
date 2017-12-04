using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp
{
    public class SttpQueryColumn
    {
        public int TableIndex;
        public string ColumnName;
        public int Variable;

        public SttpQueryColumn(int tableIndex, string columnName, int variable)
        {
            TableIndex = tableIndex;
            ColumnName = columnName;
            Variable = variable;
        }

        public SttpQueryColumn(PayloadReader rd)
        {
            TableIndex = rd.ReadInt32();
            ColumnName = rd.ReadString();
            Variable = rd.ReadInt32();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(TableIndex);
            wr.Write(ColumnName);
            wr.Write(Variable);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryColumn)}) {Variable} = {TableIndex}.{ColumnName} ");
        }

    }

    public class SttpQueryJoinedTable
    {
        public int ExistingTableIndex;
        public string ExistingForeignKeyColumn;
        public string ForeignTable;
        public int ForeignTableIndex;

        public SttpQueryJoinedTable(int existingTableIndex, string existingForeignKeyColumn, string foreignTable, int foreignTableIndex)
        {
            ExistingTableIndex = existingTableIndex;
            ExistingForeignKeyColumn = existingForeignKeyColumn;
            ForeignTable = foreignTable;
            ForeignTableIndex = foreignTableIndex;
        }

        public SttpQueryJoinedTable(PayloadReader rd)
        {
            ExistingTableIndex = rd.ReadInt32();
            ExistingForeignKeyColumn = rd.ReadString();
            ForeignTable = rd.ReadString();
            ForeignTableIndex = rd.ReadInt32();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(ExistingTableIndex);
            wr.Write(ExistingForeignKeyColumn);
            wr.Write(ForeignTable);
            wr.Write(ForeignTableIndex);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryJoinedTable)}) ({ExistingTableIndex}) LEFT JOIN {ForeignTable} AS ({ForeignTableIndex}) ON ({ExistingTableIndex}).{ExistingForeignKeyColumn} = ({ForeignTableIndex}).[PrimaryKey]");
        }
    }

    public class SttpQueryLiterals
    {
        public SttpValue Value;
        public int Variable;

        public SttpQueryLiterals(SttpValue value, int variable)
        {
            Value = value;
            Variable = variable;
        }
        public SttpQueryLiterals(PayloadReader rd)
        {
            Variable = rd.ReadInt32();
            Value = rd.ReadSttpValue();
        }
        public void Save(PayloadWriter wr)
        {
            wr.Write(Variable);
            wr.Write(Value);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryLiterals)}) {Variable} = {Value.AsString} ");
        }
    }

    public class SttpQueryProcedureStep
    {
        public string Function;
        public List<int> InputVariables;
        public int OutputVariable;

        public SttpQueryProcedureStep(string function, List<int> inputVariables, int outputVariable)
        {
            Function = function;
            InputVariables = inputVariables;
            OutputVariable = outputVariable;
        }

        public SttpQueryProcedureStep(PayloadReader rd)
        {
            Function = rd.ReadString();
            InputVariables = rd.ReadListInt();
            OutputVariable = rd.ReadInt32();
        }
        public void Save(PayloadWriter wr)
        {
            wr.Write(Function);
            wr.Write(InputVariables);
            wr.Write(OutputVariable);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryProcedureStep)}) {OutputVariable} = {Function}({string.Join(",", InputVariables)}) ");
        }
    }

    public class SttpQueryOutputColumns
    {
        public int Variable;
        public string ColumnName;

        public SttpQueryOutputColumns(int variable, string columnName)
        {
            Variable = variable;
            ColumnName = columnName;
        }

        public SttpQueryOutputColumns(PayloadReader rd)
        {
            Variable = rd.ReadInt32();
            ColumnName = rd.ReadString();
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write(Variable);
            wr.Write(ColumnName);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryOutputColumns)}) {Variable} = {ColumnName} ");
        }


    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryStatement
    {
        public string DirectTable;
        public List<SttpQueryJoinedTable> JoinedTables = new List<SttpQueryJoinedTable>();
        public List<SttpQueryLiterals> Literals = new List<SttpQueryLiterals>();
        public List<SttpQueryColumn> ColumnInputs = new List<SttpQueryColumn>();
        public List<SttpQueryProcedureStep> Procedure = new List<SttpQueryProcedureStep>();
        public List<SttpQueryOutputColumns> Outputs = new List<SttpQueryOutputColumns>();
        public List<int> GroupByVariables = new List<int>();
        public int? WhereBooleanVariable;
        public List<SttpQueryProcedureStep> HavingProcedure = new List<SttpQueryProcedureStep>();
        public int? HavingBooleanVariable;
        public int? Limit;

        public SttpQueryStatement()
        {

        }

        /// <summary>
        /// This will remap all table and variable indexes to a sequential range starting with 0.
        /// As a nature of the remap, this will also validate the query's indexing.
        /// </summary>
        public void ValidateAndRemapAllIndexes(out int variableIndexCount, out int tableIndexCount)
        {
            Dictionary<int, int> tableIndexMap = new Dictionary<int, int>();
            Dictionary<int, int> variableIndexMap = new Dictionary<int, int>();

            //Fill the table indexes
            tableIndexMap.Add(0, 0); //The direct table gets this mapping.
            foreach (var table in JoinedTables)
            {
                if (!tableIndexMap.ContainsKey(table.ExistingTableIndex))
                {
                    throw new Exception("Column Index Invalid");
                }
                tableIndexMap.Add(table.ForeignTableIndex, tableIndexMap.Count);
            }

            //Fill the variable indexes
            foreach (var v in Literals)
            {
                variableIndexMap.Add(v.Variable, variableIndexMap.Count);
            }
            foreach (var v in ColumnInputs)
            {
                if (!tableIndexMap.ContainsKey(v.TableIndex))
                {
                    throw new Exception("Column Index Invalid");
                }
                variableIndexMap.Add(v.Variable, variableIndexMap.Count);
            }

            foreach (var v in Procedure)
            {
                foreach (var vIn in v.InputVariables)
                {
                    if (!variableIndexMap.ContainsKey(vIn))
                        throw new Exception("Function with undefined input variable");
                }
                if (!variableIndexMap.ContainsKey(v.OutputVariable))
                {
                    variableIndexMap.Add(v.OutputVariable, variableIndexMap.Count);
                }
            }

            foreach (var v in Outputs)
            {
                if (!variableIndexMap.ContainsKey(v.Variable))
                {
                    throw new Exception("Output Column with undefined variable");
                }
            }

            if (WhereBooleanVariable.HasValue && !variableIndexMap.ContainsKey(WhereBooleanVariable.Value))
            {
                throw new Exception("WhereBooleanVariable with undefined variable");
            }

            //ToDo: Still need Having, Group By defined.

            //Now: Remap since the mapping is complete.
            foreach (var table in JoinedTables)
            {
                table.ForeignTableIndex = tableIndexMap[table.ForeignTableIndex];
                table.ExistingTableIndex = tableIndexMap[table.ExistingTableIndex];
            }

            //Fill the variable indexes
            foreach (var v in Literals)
            {
                v.Variable = variableIndexMap[v.Variable];
            }

            foreach (var v in ColumnInputs)
            {
                v.TableIndex = tableIndexMap[v.TableIndex];
                v.Variable = variableIndexMap[v.Variable];
            }

            foreach (var v in Procedure)
            {
                for (var x = 0; x < v.InputVariables.Count; x++)
                {
                    v.InputVariables[x] = variableIndexMap[v.InputVariables[x]];
                }
                v.OutputVariable = variableIndexMap[v.OutputVariable];
            }

            foreach (var v in Outputs)
            {
                v.Variable = variableIndexMap[v.Variable];
            }

            if (WhereBooleanVariable.HasValue)
            {
                WhereBooleanVariable = variableIndexMap[WhereBooleanVariable.Value];
            }

            tableIndexCount = tableIndexMap.Count;
            variableIndexCount = variableIndexMap.Count;
        }

        public SttpQueryStatement(PayloadReader rd)
        {

        }

        public SttpQueryStatement(SttpConnectionString query)
        {
            if (query.TryGetValue("Syntax", out SttpValue value))
            {
                if (value.AsString != "SttpQueryStatement")
                {
                    throw new Exception("This query is not a STTP Statement query");
                }
            }

            foreach (var element in query.Values)
            {
                switch (element.RecordName)
                {
                    case "DirectTable":
                        DirectTable = element.Value.AsString;
                        break;
                    case "JoinedTables":

                    case "Literals":

                    case "ColumnInputs":

                    case "Procedure":

                    case "Outputs":

                    case "GroupByVariables":

                    case "WhereBooleanVariable":
                        WhereBooleanVariable = element.Value.AsInt32;
                        break;
                    case "HavingProcedure":

                    case "HavingBooleanVariable":
                        HavingBooleanVariable = element.Value.AsInt32;
                        break;
                    case "Limit":
                        Limit = element.Value.AsInt32;
                        break;
                    default:
                        if (element.Requirement != SttpConnectionStringCompatiblity.Optional)
                            throw new Exception("An unknown query command was presented and not marked as optional.");
                        break;
                }
            }
        }

        public void Save(PayloadWriter wr)
        {
            wr.Write((byte)1); wr.Write(DirectTable);
            wr.Write((byte)2); wr.Write(JoinedTables);
            wr.Write((byte)3); wr.Write(Literals);
            wr.Write((byte)4); wr.Write(ColumnInputs);
            wr.Write((byte)5); wr.Write(Procedure);
            wr.Write((byte)6); wr.Write(Outputs);
            wr.Write((byte)7); wr.Write(GroupByVariables);
            wr.Write((byte)8); wr.Write(WhereBooleanVariable);
            wr.Write((byte)9); wr.Write(HavingProcedure);
            wr.Write((byte)10); wr.Write(HavingBooleanVariable);
            wr.Write((byte)11); wr.Write(Limit);
            wr.Write((byte)0);
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine("(" + nameof(SttpQueryStatement) + ")");
            builder.Append(linePrefix); builder.AppendLine($"DirectTable: {DirectTable} ");
            builder.Append(linePrefix); builder.AppendLine($"WhereBooleanVariable: {WhereBooleanVariable} ");
            builder.Append(linePrefix); builder.AppendLine($"HavingBooleanVariable: {HavingBooleanVariable} ");
            builder.Append(linePrefix); builder.AppendLine($"Limit: {Limit} ");

            builder.Append(linePrefix); builder.AppendLine($"JoinedTables Count {JoinedTables.Count} ");
            foreach (var table in JoinedTables)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"Literals Count {Literals.Count} ");
            foreach (var table in Literals)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"ColumnInputs Count {ColumnInputs.Count} ");
            foreach (var table in ColumnInputs)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"Procedure Count {Procedure.Count} ");
            foreach (var table in Procedure)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"Outputs Count {Outputs.Count} ");
            foreach (var table in Outputs)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
            builder.Append(linePrefix); builder.AppendLine($"GroupByVariables Count {GroupByVariables.Count} ");
            builder.Append(linePrefix); builder.AppendLine($"Group By: ({string.Join(",", GroupByVariables)}) ");
            builder.Append(linePrefix); builder.AppendLine($"HavingProcedure Count {HavingProcedure.Count} ");
            foreach (var table in HavingProcedure)
            {
                table.GetFullOutputString(linePrefix + " ", builder);
            }
        }

        public SttpConnectionString ToConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
