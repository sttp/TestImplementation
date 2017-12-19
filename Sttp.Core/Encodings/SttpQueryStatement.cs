using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
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

        public SttpQueryColumn(SttpMarkupElement element)
        {
            if (element.ElementName != "ColumnInput")
                throw new Exception("Element mismatch");

            TableIndex = (int)element.GetValue("TableIndex");
            ColumnName = (string)element.GetValue("ColumnName");
            Variable = (int)element.GetValue("Variable");

            element.ErrorIfNotHandled();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryColumn)}) {Variable} = {TableIndex}.{ColumnName} ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("ColumnInput"))
            {
                writer.WriteValue("TableIndex", TableIndex);
                writer.WriteValue("ColumnName", ColumnName);
                writer.WriteValue("Variable", Variable);
            }
        }
    }

    public class SttpQueryJoinedTable
    {
        public int ExistingTableIndex;
        public string ExistingForeignKeyColumn;
        public string ForeignTable;
        public int ForeignTableIndex;

        public SttpQueryJoinedTable(SttpMarkupElement element)
        {
            if (element.ElementName != "JoinedTable")
                throw new Exception("Element mismatch");

            ExistingTableIndex = (int)element.GetValue("ExistingTableIndex");
            ExistingForeignKeyColumn = (string)element.GetValue("ExistingForeignKeyColumn");
            ForeignTable = (string)element.GetValue("ForeignTable");
            ForeignTableIndex = (int)element.GetValue("ForeignTableIndex");

            element.ErrorIfNotHandled();
        }

        public SttpQueryJoinedTable(int existingTableIndex, string existingForeignKeyColumn, string foreignTable, int foreignTableIndex)
        {
            ExistingTableIndex = existingTableIndex;
            ExistingForeignKeyColumn = existingForeignKeyColumn;
            ForeignTable = foreignTable;
            ForeignTableIndex = foreignTableIndex;
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryJoinedTable)}) ({ExistingTableIndex}) LEFT JOIN {ForeignTable} AS ({ForeignTableIndex}) ON ({ExistingTableIndex}).{ExistingForeignKeyColumn} = ({ForeignTableIndex}).[PrimaryKey]");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("JoinedTable"))
            {
                writer.WriteValue("ExistingTableIndex", ExistingTableIndex);
                writer.WriteValue("ExistingForeignKeyColumn", ExistingForeignKeyColumn);
                writer.WriteValue("ForeignTable", ForeignTable);
                writer.WriteValue("ForeignTableIndex", ForeignTableIndex);
            }
        }
    }

    public class SttpQueryLiteral
    {
        public SttpValue Value;
        public int Variable;

        public SttpQueryLiteral(SttpValue value, int variable)
        {
            Value = value;
            Variable = variable;
        }

        public SttpQueryLiteral(SttpMarkupElement element)
        {
            if (element.ElementName != "Literal")
                throw new Exception("Element mismatch");

            Value = element.GetValue("Value");
            Variable = (int)element.GetValue("Variable");

            element.ErrorIfNotHandled();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryLiteral)}) {Variable} = {Value.AsString} ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("Literal"))
            {
                writer.WriteValue("Value", Value);
                writer.WriteValue("Variable", Variable);
            }
        }

    }

    public class SttpQueryProcedureStep
    {
        public string Function;
        public List<int> InputVariables = new List<int>();
        public int OutputVariable;

        public SttpQueryProcedureStep(string function, List<int> inputVariables, int outputVariable)
        {
            Function = function;
            InputVariables = inputVariables;
            OutputVariable = outputVariable;
        }

        public SttpQueryProcedureStep(SttpMarkupElement element, bool isHavingStep)
        {
            if (element.ElementName != (isHavingStep ? "Having" : "Procedure"))
                throw new Exception("Element mismatch");

            Function = (string)element.GetValue("Function");
            OutputVariable = (int)element.GetValue("OutputVariable");
            foreach (var item in element.GetElement("InputVariables").ForEachValue("Item"))
            {
                InputVariables.Add((int)item);
            }

            element.ErrorIfNotHandled();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryProcedureStep)}) {OutputVariable} = {Function}({string.Join(",", InputVariables)}) ");
        }

        public void Save(SttpMarkupWriter writer, bool isHavingStep)
        {
            using (writer.StartElement(isHavingStep ? "Having" : "Procedure"))
            {
                writer.WriteValue("Function", Function);
                writer.WriteValue("OutputVariable", OutputVariable);

                using (writer.StartElement("InputVariables"))
                {
                    foreach (var x in InputVariables)
                    {
                        writer.WriteValue("Item", x);
                    }
                }
            }
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

        public SttpQueryOutputColumns(SttpMarkupElement element)
        {
            if (element.ElementName != "Output")
                throw new Exception("Element mismatch");

            ColumnName = (string)element.GetValue("ColumnName");
            Variable = (int)element.GetValue("Variable");

            element.ErrorIfNotHandled();
        }

        public void GetFullOutputString(string linePrefix, StringBuilder builder)
        {
            builder.Append(linePrefix); builder.AppendLine($"({nameof(SttpQueryOutputColumns)}) {Variable} = {ColumnName} ");
        }

        public void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement("Output"))
            {
                writer.WriteValue("Variable", Variable);
                writer.WriteValue("ColumnName", ColumnName);
            }
        }

    }

    /// <summary>
    /// The STTP based query expression object
    /// </summary>
    public class SttpQueryStatement : SttpQueryBase
    {
        public string DirectTable;
        public List<SttpQueryJoinedTable> JoinedTables = new List<SttpQueryJoinedTable>();
        public List<SttpQueryLiteral> Literals = new List<SttpQueryLiteral>();
        public List<SttpQueryColumn> ColumnInputs = new List<SttpQueryColumn>();
        public List<SttpQueryProcedureStep> Procedure = new List<SttpQueryProcedureStep>();
        public List<SttpQueryOutputColumns> Outputs = new List<SttpQueryOutputColumns>();
        public List<int> GroupByVariables = new List<int>();
        public int? WhereBooleanVariable;
        public List<SttpQueryProcedureStep> HavingProcedure = new List<SttpQueryProcedureStep>();
        public int? HavingBooleanVariable;
        public int? Limit;

        public SttpQueryStatement()
            : base("SttpQuery")
        {

        }

        public SttpQueryStatement(SttpMarkupElement reader)
            : base("SttpQuery")
        {
            foreach (var element in reader.ChildElements)
            {
                switch (element.ElementName)
                {
                    case "JoinedTable":
                        JoinedTables.Add(new SttpQueryJoinedTable(element));
                        break;
                    case "ColumnInput":
                        ColumnInputs.Add(new SttpQueryColumn(element));
                        break;
                    case "Literal":
                        Literals.Add(new SttpQueryLiteral(element));
                        break;
                    case "Procedure":
                        Procedure.Add(new SttpQueryProcedureStep(element, false));
                        break;
                    case "Output":
                        Outputs.Add(new SttpQueryOutputColumns(element));
                        break;
                    case "Having":
                        HavingProcedure.Add(new SttpQueryProcedureStep(element, true));
                        break;
                    case "GroupBy":
                        foreach (var value in element.ForEachValue("Item"))
                        {
                            GroupByVariables.Add((int)value);
                        }
                        element.ErrorIfNotHandled();
                        break;
                    default:
                        throw new Exception("Unknown value");
                }
            }

            foreach (var item in reader.ChildValues)
            {
                switch (item.ValueName)
                {
                    case "DirectTable":
                        DirectTable = (string)item.Value;
                        break;
                    case "WhereBooleanVariable":
                        WhereBooleanVariable = (int?)item.Value;
                        break;
                    case "HavingBooleanVariable":
                        HavingBooleanVariable = (int?)item.Value;
                        break;
                    case "Limit":
                        Limit = (int?)item.Value;
                        break;
                    default:
                        throw new Exception("Unknown value");
                }
                item.Handled = true;
            }

            reader.ErrorIfNotHandled();
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

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("DirectTable", DirectTable);
            if (WhereBooleanVariable.HasValue)
                writer.WriteValue("WhereBooleanVariable", WhereBooleanVariable);
            if (Limit.HasValue)
                writer.WriteValue("Limit", Limit);
            if (HavingBooleanVariable.HasValue)
                writer.WriteValue("HavingBooleanVariable", HavingBooleanVariable);

            foreach (var item in JoinedTables)
            {
                item.Save(writer);
            }

            foreach (var item in Literals)
            {
                item.Save(writer);
            }

            foreach (var item in ColumnInputs)
            {
                item.Save(writer);
            }

            foreach (var item in Procedure)
            {
                item.Save(writer, false);
            }

            foreach (var item in Outputs)
            {
                item.Save(writer);
            }

            if (GroupByVariables.Count > 0)
            {
                using (writer.StartElement("GroupBy"))
                {
                    foreach (var item in GroupByVariables)
                    {
                        writer.WriteValue("Item", item);
                    }
                }
            }

            foreach (var item in HavingProcedure)
            {
                item.Save(writer, true);
            }
        }

        public SttpMarkup ToSttpMarkup()
        {
            var sml = new SttpMarkupWriter("SttpQuery");
            Save(sml);
            return sml.ToSttpMarkup();
        }
    }
}
